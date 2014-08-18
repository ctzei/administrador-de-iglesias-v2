using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using Quiksoft.FreeSMTP;
using ZagueEF.Core;
using ExtensionMethods;
using log4net;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeReportesDeAsistencias
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ManejadorDeReportesDeAsistencias));
        private IglesiaEntities contexto = new IglesiaEntities(); // Este manejador se puede llamar desde un thread secundario

        #region Notificacion de AsistenciasPorSemana por Email

        public enum TipoDeReporte
        {
            /*
             SIMPLE = Celulas SOLO de primer nivel
             DETALLADA = Todas las celulas de la red
             CON CORREOS = En el reporte se copiaran a TODOS los lideres de celulas involucradas
             SIN CORREOS = En el reporte NO se copiaran a los lideres de celula; solo sera enviado al subscriptor de la notificacion
             */

            SimpleConCorreos = 1,
            DetalladaConCorreos = 2,
            SimpleSinCorreos = 3,
            DetalladaSinCorreos = 4
        }
        public EmailMessage GenerarCorreoSemanalDeFaltaDeAsistenciasPorRed(DateTime fechaInicial, Miembro miembro, TipoDeReporte tipoDeReporte, string tituloDelReporte)
        {
            if (miembro.Borrado == false)
            {
                Validaciones.ValidarEmail(miembro.Email);
                DateTime fechaFinal = fechaInicial.AddDays(7);

                bool esReporteDeRed;
                bool agregarCorreosDeLideres;

                //Determinamos el tipo de reporte a enviar
                switch (tipoDeReporte)
                {
                    case TipoDeReporte.SimpleConCorreos:
                        esReporteDeRed = false;
                        agregarCorreosDeLideres = true;
                        break;

                    case TipoDeReporte.DetalladaConCorreos:
                        esReporteDeRed = true;
                        agregarCorreosDeLideres = true;
                        break;

                    case TipoDeReporte.SimpleSinCorreos:
                        esReporteDeRed = false;
                        agregarCorreosDeLideres = false;
                        break;

                    case TipoDeReporte.DetalladaSinCorreos:
                        esReporteDeRed = true;
                        agregarCorreosDeLideres = false;
                        break;

                    default:
                        throw new ExcepcionReglaNegocio("Tipo de reporte NO MANEJADO. Favor de contactar al administrador del sistema. Tipo de Reporte: " + tipoDeReporte);
                }

                //Buscamos todas las celulas en las que el miembro de la inscripcion es lider directo
                List<int> celulasALasQueEsLider = (from o in miembro.CelulaLider
                                                   where
                                                       o.Celula.Borrado == false &&
                                                       o.Borrado == false
                                                   orderby o.Celula.Descripcion
                                                   select o.CelulaId).ToList<int>();

                //Esta variable determina si alguna de las celulas a las que el usuario es lider carece de asistencia, de ser asi SI se envia el correo
                bool tieneCelulasSinAsistencia = false;

                //Mandamos el correo, solo si el miembro es lider de alguna celula; sino el correo iria vacio
                if (celulasALasQueEsLider.Count > 0)
                {
                    EmailMessage email = new EmailMessage();
                    StringBuilder contenido = new StringBuilder(750);

                    //Inicializamos el contenido del correo
                    contenido.Append(string.Format("<h3>{0}</h3>", tituloDelReporte));
                    contenido.Append("<p>A continuacion se mostraran las faltas de registro de asistencia, separadas por redes, de las cuales el usuario es lider directo. Las siguientes celulas aun no han registrado asistencias en la semana anterior inmediata:</p>");

                    //Por cada celular que sea lider directo se va a buscar la asistencia de sus "celulas" de primer orden o TODA la red, dependiendo del tipo de reporte
                    foreach (int celulaId in celulasALasQueEsLider)
                    {
                        //El listado de celulas sin asistencia es por RED; asi que se reinicia por cada celula a la que el miembro es lider directo
                        List<int> celulasSinAsistenciaRegistrada = new List<int>();

                        Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado reporteDeAsistencias = this.ObtenerReporteDeAsistenciasPorCelula(celulaId, fechaInicial, fechaFinal, esReporteDeRed);
                        foreach (Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado.AsistenciaDeCelula asistenciasPorCelula in reporteDeAsistencias.AsistenciasTotales.OrderBy(o => o.Descripcion))
                        {
                            if (asistenciasPorCelula.CelulaId > 0)
                            {
                                foreach (Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado.Asistencia asistencia in asistenciasPorCelula.Asistencias)
                                {
                                    if ((asistencia.Asistencias <= 0) && (asistencia.Cancelaciones <= 0))
                                    {
                                        celulasSinAsistenciaRegistrada.Add(asistenciasPorCelula.CelulaId);
                                    }
                                }
                            }
                        }

                        //Marcamos que SI existe cuando menos una celula a quien reportarle la falta de asistencia
                        if (celulasSinAsistenciaRegistrada.Count > 0)
                        {
                            tieneCelulasSinAsistencia = true;
                        }

                        contenido.Append("<br/><table border='1' width='500px'>");
                        contenido.AppendFormat("<tr><th>{0}</th><th>{1}</th></tr>", "Id", "Descripcion");

                        foreach (int c in celulasSinAsistenciaRegistrada)
                        {
                            Celula celula = (from o in contexto.Celula where o.CelulaId == c && o.Borrado == false select o).SingleOrDefault();
                            if (celula != null)
                            {
                                if (agregarCorreosDeLideres)
                                {
                                    foreach (CelulaLider celulaLider in celula.CelulaLider.Where(o => o.Celula.Borrado == false && o.Miembro.Borrado == false && o.Borrado == false))
                                    {
                                        // Solo agregamos al correo los usuarios con correos NO falsos... y que no sea el usuario principal a quien se mandara el reporte
                                        if ((!celulaLider.Miembro.Email.Trim().ToLower().EndsWith("correo-e.com")) && (celulaLider.MiembroId != miembro.MiembroId))
                                        {
                                            email.Recipients.Add(celulaLider.Miembro.Email, celulaLider.Miembro.NombreCompleto, RecipientType.CC);
                                        }
                                    }
                                }

                                contenido.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", celula.CelulaId, celula.Descripcion);
                            }
                        }
                        contenido.Append("</table>");
                    }

                    if (tieneCelulasSinAsistencia)
                    {
                        //Preparamos los ultimos detalles del correo a enviar
                        email.BodyParts.Add(contenido.ToString(), BodyPartFormat.HTML);
                        email.Recipients.Add(miembro.Email, miembro.NombreCompleto, RecipientType.To);
                        email.Subject = tituloDelReporte;
                        return email;
                    }
                    else
                    {
                        email.Reset();
                        return null;
                    }
                }
            }

            return null;
        }

        #endregion

        #region Reportes de Asistencia

        #region Por Celula

        public Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado ObtenerReporteDeAsistenciasPorCelula(int celulaId, DateTime fechaInicial, DateTime fechaFinal, bool mostrarRed)
        {
            string cacheKey = MethodInfo.GetCurrentMethod().Name + "_" + celulaId + "_" + fechaInicial.Ticks + "_" + fechaFinal.Ticks + "_" + mostrarRed;
            Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado reporte = Cache.Instance.Obtener<Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado>(cacheKey);

            if (reporte == null)
            {
                List<ObtenerReporteDeAsistenciasPorCelulas_Result> asistencias = contexto.ObtenerReporteDeAsistenciasPorCelulas(celulaId, fechaInicial, fechaFinal, mostrarRed).ToList<ObtenerReporteDeAsistenciasPorCelulas_Result>();
                reporte = new Modelos.Retornos.ReporteDeAsistenciasDeCelulaSumarizado();

                foreach (ObtenerReporteDeAsistenciasPorCelulas_Result asistencia in asistencias)
                {
                    reporte.AgregarAsistencia(asistencia.CelulaId.Value, asistencia.Descripcion, asistencia.FechaInicial.Value, asistencia.FechaFinal.Value, asistencia.Asistencias.Value, asistencia.Cancelaciones.Value, asistencia.Faltas.Value, asistencia.Gente.Value, asistencia.Total.Value);
                }
                Cache.Instance.Guardar(cacheKey, reporte, 48);
            }

            return reporte;
        }

        #endregion

        #region Por Miembro

        public List<ObtenerReporteDeAsistenciasPorMiembro_Result> ObtenerReporteDeAsistenciasPorMiembro(int celulaId, int miembroId, DateTime fechaInicial, DateTime fechaFinal, bool esSemanal)
        {
            return contexto.ObtenerReporteDeAsistenciasPorMiembro(celulaId, miembroId, fechaInicial.GetFirstDateOfWeek(), fechaFinal, esSemanal).ToList<ObtenerReporteDeAsistenciasPorMiembro_Result>();
        }

        public List<ObtenerReporteDeAsistenciasPorMiembro_Result> ObtenerReporteDeAsistenciasPorMiembro(int celulaId, int miembroId, DateTime fechaFinal, bool esSemanal)
        {
            //Obtenemos la primera asistencia y en base a ella iniciamos el reporte completo
            CelulaMiembroAsistencia primeraAsistencia = (from o in contexto.CelulaMiembroAsistencia
                                                         where
                                                             o.CelulaId == celulaId &&
                                                             o.MiembroId == miembroId
                                                         orderby o.Anio, o.Mes, o.Dia
                                                         select o
                                       ).FirstOrDefault();

            DateTime fechaInicial;
            if (primeraAsistencia != null)
            {
                fechaInicial = new DateTime(primeraAsistencia.Anio, primeraAsistencia.Mes, primeraAsistencia.Dia).GetFirstDateOfWeek();
            }
            else
            {
                fechaInicial = DateTime.Now; //Si aun no tiene asistencias se toma le fecha de HOY, de todas maneras el reporte debera de salir en blanco
            }

            return contexto.ObtenerReporteDeAsistenciasPorMiembro(celulaId, miembroId, fechaInicial.GetFirstDateOfWeek(), fechaFinal, esSemanal).ToList<ObtenerReporteDeAsistenciasPorMiembro_Result>();
        }

        #endregion

        #endregion

        #region Informacion General Por Red

        public Modelos.Retornos.InformacionGeneralPorRed ObtenerInformacionGeneralPorRed(int celulaId)
        {
            string cacheKey = MethodInfo.GetCurrentMethod().Name + "_" + celulaId;
            Modelos.Retornos.InformacionGeneralPorRed reporte = Cache.Instance.Obtener<Modelos.Retornos.InformacionGeneralPorRed>(cacheKey);

            if (reporte == null)
            {
                ObjectParameter numeroCelulas = new ObjectParameter("numeroCelulas", typeof(int));
                ObjectParameter lideresCelula = new ObjectParameter("lideresCelula", typeof(int));
                ObjectParameter estacas = new ObjectParameter("estacas", typeof(int));
                ObjectParameter miembros = new ObjectParameter("miembros", typeof(int));
                ObjectParameter miembrosHombres = new ObjectParameter("miembrosHombres", typeof(int));
                ObjectParameter miembrosMujeres = new ObjectParameter("miembrosMujeres", typeof(int));
                ObjectParameter miembrosAsistenIglesia = new ObjectParameter("miembrosAsistenIglesia", typeof(int));
                ObjectParameter miembrosAsistenIglesiaHombres = new ObjectParameter("miembrosAsistenIglesiaHombres", typeof(int));
                ObjectParameter miembrosAsistenIglesiaMujeres = new ObjectParameter("miembrosAsistenIglesiaMujeres", typeof(int));
                ObjectParameter folis = new ObjectParameter("folis", typeof(int));

                contexto.ObtenerInformacionGeneralPorRed(celulaId, 6, 9, numeroCelulas, lideresCelula, estacas, miembros, miembrosHombres, miembrosMujeres, miembrosAsistenIglesia, miembrosAsistenIglesiaHombres, miembrosAsistenIglesiaMujeres, folis);
                reporte = new Modelos.Retornos.InformacionGeneralPorRed();
                reporte.CantidadDeCelulas = Convert.ToInt32(numeroCelulas.Value);
                reporte.CantidadDeLideresDeCelula = Convert.ToInt32(lideresCelula.Value);
                reporte.CantidadDeEstacas = Convert.ToInt32(estacas.Value);
                reporte.CantidadDeMiembros = Convert.ToInt32(miembros.Value);
                reporte.CantidadDeMiembrosHombres = Convert.ToInt32(miembrosHombres.Value);
                reporte.CantidadDeMiembrosMujeres = Convert.ToInt32(miembrosMujeres.Value);
                reporte.CantidadDeMiembrosQueAsistenIglesia = Convert.ToInt32(miembrosAsistenIglesia.Value);
                reporte.CantidadDeMiembrosQueAsistenIglesiaHombres = Convert.ToInt32(miembrosAsistenIglesiaHombres.Value);
                reporte.CantidadDeMiembrosQueAsistenIglesiaMujeres = Convert.ToInt32(miembrosAsistenIglesiaMujeres.Value);
                reporte.CantidadDeFolis = Convert.ToInt32(folis.Value);
                Cache.Instance.Guardar(cacheKey, reporte, 2);
            }

            return reporte;
        }

        #endregion
    }
}
