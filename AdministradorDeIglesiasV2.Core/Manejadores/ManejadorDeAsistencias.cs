using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Net.Mail;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Modelos.Retornos;
using ZagueEF.Core;
using ExtensionMethods;
using log4net;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeAsistenciasDeCelula
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ManejadorDeAsistenciasDeCelula));

        public int ObtenerSemanasDeInasistenciasPorMiembro(int miembroId)
        {
            Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroId select o).SingleOrDefault();
            if (miembro != null)
            {
                int celulaId = miembro.CelulaId;

                DateTime ultimaAsistenciaDeCelula = this.ObtenerFechaDeUltimaAsistencia(celulaId);
                DateTime ultimaAsistenciaDeMiembro = this.ObtenerUltimaAsistenciaPorMiembro(miembroId);

                TimeSpan timespan = ultimaAsistenciaDeCelula - ultimaAsistenciaDeMiembro;
                return ((int)timespan.TotalDays / 7);
            }
            else
            {
                return -1;
            }
        }

        public DateTime ObtenerUltimaAsistenciaPorMiembro(int miembroId)
        {
            return ObtenerUltimaAsistenciaPorMiembro(miembroId, false);
        }

        public DateTime ObtenerUltimaAsistenciaPorMiembro(int miembroId, bool considerarCancelaciones)
        {
            Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroId select o).SingleOrDefault();
            if (miembro != null)
            {
                int celulaId = miembro.CelulaId;

                var ultimaAsistenciaACelula = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia where o.CelulaId == celulaId && o.MiembroId == miembroId orderby o.Anio descending, o.Mes descending, o.Dia descending select new { year = o.Anio, month = o.Mes, day = o.Dia }).FirstOrDefault();
                if (ultimaAsistenciaACelula != null)
                {
                    DateTime asistencia = new DateTime(ultimaAsistenciaACelula.year, ultimaAsistenciaACelula.month, ultimaAsistenciaACelula.day);
                    if (considerarCancelaciones)
                    {
                        var ultimaCancelacionDeCelula = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia where o.CelulaId == celulaId orderby o.Anio descending, o.Mes descending, o.Dia descending select new { year = o.Anio, month = o.Mes, day = o.Dia }).FirstOrDefault();
                        if (ultimaCancelacionDeCelula != null)
                        {
                            DateTime cancelacion = new DateTime(ultimaCancelacionDeCelula.year, ultimaCancelacionDeCelula.month, ultimaCancelacionDeCelula.day);
                            return (asistencia > cancelacion) ? asistencia : cancelacion;
                        }
                        else
                        {
                            return asistencia;
                        }
                    }
                    else
                    {
                        return asistencia;
                    }
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        #region Obtener y Guardar AsistenciasPorSemana a las Celulas

        private int diasPermitidosEntreAsistencias = 7;
        private int diasDeMargenEntreAsistencias = 2;

        public bool CelulaFueCancelada(int celulaId, DateTime fecha)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia
                    where
                    o.CelulaId == celulaId &&
                    o.Anio == fecha.Year &&
                    o.Mes == fecha.Month &&
                    o.Dia == fecha.Day
                    select o).Any();
        }

        public string ObtenerRazonDeLaCancelacion(int celulaId, DateTime fecha)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia where o.CelulaId == celulaId && o.Dia == diaSeleccionado && o.Mes == mesSeleccionado && o.Anio == anioSeleccionado select o.Descripcion).SingleOrDefault();
        }

        public bool CancelarAsistencia(int celulaId, DateTime fecha, string razonDeLaCancelacion, int usuarioIdQueRegistra)
        {
            determinarSiFechaEsPermitida(fecha, celulaId);

            CelulaCancelacionAsistencia cancelacion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia
                                                       where
                                                       o.CelulaId == celulaId &&
                                                       o.Anio == fecha.Year &&
                                                       o.Mes == fecha.Month &&
                                                       o.Dia == fecha.Day
                                                       select o).SingleOrDefault();

            if (cancelacion == null)
            {
                cancelacion = new CelulaCancelacionAsistencia();
            }

            // Agregamos o actualizamos el registro de la cancelacion
            cancelacion.CelulaId = celulaId;
            cancelacion.Anio = fecha.Year;
            cancelacion.Mes = fecha.Month;
            cancelacion.Dia = fecha.Day;
            cancelacion.MiembroQueRegistraId = usuarioIdQueRegistra;
            cancelacion.Descripcion = razonDeLaCancelacion;
            cancelacion.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

            // Borramos cualquier otra asistencia registrada para ese mismo dia
            foreach (CelulaMiembroAsistencia asistencia in (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia
                                                            where
                                                            o.CelulaId == celulaId &&
                                                            o.Anio == fecha.Year &&
                                                            o.Mes == fecha.Month &&
                                                            o.Dia == fecha.Day
                                                            select o).ToList())
            {
                SesionActual.Instance.getContexto<IglesiaEntities>().DeleteObject(asistencia);
            }

            //Las eliminaciones se hacen en grupo
            SesionActual.Instance.getContexto<IglesiaEntities>().SaveChanges();

            log.InfoFormat("Asistencia de la celula [{0}] del dia {1} cancelada correctamente por [{2}]", celulaId, fecha.ToFullDateString(), usuarioIdQueRegistra);
            return true;
        }

        #region Obtener AsistenciasPorSemana

        public Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada ObtenerAsistencia(int celulaId, DateTime fecha)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            IglesiaEntities contexto = new IglesiaEntities();

            List<Modelos.Retornos.AsistenciaDeCelulaPorMiembro> asistencias = (
                from m in contexto.Miembro
                join a in contexto.CelulaMiembroAsistencia on
                    new { MiembroId = m.MiembroId, Anio = anioSeleccionado, Mes = mesSeleccionado, Dia = diaSeleccionado }
                    equals
                    new { MiembroId = a.MiembroId, Anio = a.Anio, Mes = a.Mes, Dia = a.Dia }
                    into ps
                from a in ps.DefaultIfEmpty()
                where m.CelulaId == celulaId && m.Borrado == false
                orderby m.Primer_Nombre ascending, m.Apellido_Paterno ascending
                select new Modelos.Retornos.AsistenciaDeCelulaPorMiembro
                {
                    Id = (a.CelulaMiembroAsistenciaId != null ? a.CelulaMiembroAsistenciaId : -1),
                    MiembroId = m.MiembroId,
                    PrimerNombre = m.Primer_Nombre,
                    SegundoNombre = m.Segundo_Nombre,
                    ApellidoPaterno = m.Apellido_Paterno,
                    ApellidoMaterno = m.Apellido_Materno,
                    Asistencia = (a.CelulaMiembroAsistenciaId != null ? true : false),
                    Estatus =
                        (from o in contexto.ConsolidacionBoleta where o.Email == m.Email select o.Email).Any() && !(from o in contexto.CelulaMiembroAsistencia where o.CelulaId == celulaId && o.MiembroId == m.MiembroId select o.MiembroId).Any() ? "Consolidacion" :
                        (from o in contexto.CelulaMiembroAsistencia where o.CelulaId == celulaId && o.MiembroId == m.MiembroId select o.MiembroId).Count() < 4 ? "Nuevo" : null,
                    Peticiones = a.Peticiones
                }).ToList<Modelos.Retornos.AsistenciaDeCelulaPorMiembro>();

            Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada rtn = new Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada();
            rtn.Asistencias = asistencias;
            rtn.CantidadDeRegistros = asistencias.Count();
            rtn.CantidadDeAsistencias = asistencias.Count(o => o.Asistencia == true);

            return rtn;
        }

        public int ObtenerNumeroDeInvitados(int celulaId, DateTime fecha)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            CelulaInvitadosAsistencia invitados = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaInvitadosAsistencia
                                                   where
                                                       o.CelulaId == celulaId &&
                                                       o.Anio == anioSeleccionado &&
                                                       o.Mes == mesSeleccionado &&
                                                       o.Dia == diaSeleccionado
                                                   select o).FirstOrDefault();
            if (invitados != null)
            {
                return invitados.NumeroDeInvitados;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        public bool GuardarAsistencia(int celulaId, DateTime fecha, Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistencias, int usuarioIdQueRegistra)
        {
            return GuardarAsistencia(celulaId, fecha, asistencias, 0, usuarioIdQueRegistra);
        }

        public bool GuardarAsistencia(int celulaId, DateTime fecha, Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistencias, int numeroDeInvitados, int usuarioIdQueRegistra)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            determinarSiFechaEsPermitida(fecha, celulaId); // Determinanos si la fecha es correcta y permitida

            Dictionary<int, string> asistenciasNuevas = new Dictionary<int, string>();
            List<int> asistenciasABorrar = new List<int>();
            Dictionary<int, string> asistenciasAActualizar = new Dictionary<int, string>();

            foreach (Modelos.Retornos.AsistenciaDeCelulaPorMiembro asistencia in asistencias.Asistencias)
            {
                //Registros existentes a eliminar
                if (!asistencia.Asistencia && (asistencia.Id > 0))
                {
                    asistenciasABorrar.Add(asistencia.Id);
                }
                //Registros nuevos a crear
                else if (asistencia.Asistencia && (asistencia.Id < 0))
                {
                    asistenciasNuevas.Add(asistencia.MiembroId, asistencia.Peticiones);
                }
                //Registros a actualizar
                else if (asistencia.Id > 0)
                {
                    asistenciasAActualizar.Add(asistencia.Id, asistencia.Peticiones);
                }
            }

            //Eliminamos las asistencias preexistentes
            if (asistenciasABorrar.Count > 0)
            {
                foreach (CelulaMiembroAsistencia asistencia in (from a in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia where asistenciasABorrar.Contains(a.CelulaMiembroAsistenciaId) select a))
                {
                    SesionActual.Instance.getContexto<IglesiaEntities>().DeleteObject(asistencia);
                }
            }

            //Creamos las nuevas asistencias
            if (asistenciasNuevas.Count > 0)
            {
                CelulaMiembroAsistencia asistencia;
                foreach (KeyValuePair<int, string> a in asistenciasNuevas)
                {
                    asistencia = new CelulaMiembroAsistencia();
                    asistencia.CelulaId = celulaId;
                    asistencia.MiembroId = a.Key;
                    asistencia.Anio = anioSeleccionado;
                    asistencia.Mes = mesSeleccionado;
                    asistencia.Dia = diaSeleccionado;
                    asistencia.Peticiones = a.Value;
                    asistencia.MiembroQueRegistraId = usuarioIdQueRegistra;
                    SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia.AddObject(asistencia);
                }
            }

            //Las inserciones y las eliminaciones si se pueden hacer en grupo... las actualizaciones no...
            SesionActual.Instance.getContexto<IglesiaEntities>().SaveChanges();

            //Actualizamos las asistencias
            if (asistenciasAActualizar.Count > 0)
            {
                CelulaMiembroAsistencia asistencia;
                foreach (KeyValuePair<int, string> a in asistenciasAActualizar)
                {
                    asistencia = (from q in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia where q.CelulaMiembroAsistenciaId == a.Key select q).FirstOrDefault();
                    asistencia.Peticiones = a.Value;

                    //Cada actualizacion tiene que tener su propia llamada a la BD
                    SesionActual.Instance.getContexto<IglesiaEntities>().SaveChanges();
                }
            }

            #region Invitados

            // Borramos cualquier registro de invitados anterior
            CelulaInvitadosAsistencia invitados = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaInvitadosAsistencia
                                                   where
                                                       o.CelulaId == celulaId &&
                                                       o.Anio == anioSeleccionado &&
                                                       o.Mes == mesSeleccionado &&
                                                       o.Dia == diaSeleccionado
                                                   select o).FirstOrDefault();

            if (invitados != null)
            {
                invitados.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }

            // Si hubo invitados guardamos el registro
            if (numeroDeInvitados > 0)
            {
                invitados = new CelulaInvitadosAsistencia();
                invitados.CelulaId = celulaId;
                invitados.MiembroQueRegistraId = usuarioIdQueRegistra;
                invitados.Anio = anioSeleccionado;
                invitados.Mes = mesSeleccionado;
                invitados.Dia = diaSeleccionado;
                invitados.NumeroDeInvitados = numeroDeInvitados;
                invitados.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }

            #endregion

            #region Cancelacion

            //Borramos la "cancelacion" de la celula si llegase a existir
            CelulaCancelacionAsistencia cancelacion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia
                                                       where
                                                           o.CelulaId == celulaId &&
                                                           o.Anio == anioSeleccionado &&
                                                           o.Mes == mesSeleccionado &&
                                                           o.Dia == diaSeleccionado
                                                       select o).FirstOrDefault();
            if (cancelacion != null)
            {
                cancelacion.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }

            #endregion

            log.InfoFormat("Asistencia de la celula [{0}] del dia {1} registrada correctamente por [{2}]", celulaId, fecha.ToFullDateString(), usuarioIdQueRegistra);
            return true;
        }

        public DateTime ObtenerFechaDeUltimaAsistencia(int celulaId)
        {
            // Obtenemos la fecha de creacion de la celula
            DateTime fechaDeCreacion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == celulaId select o.Creacion).SingleOrDefault();

            // Obtenemos las asistencias registradas (primera y ultima)
            var ultimaAsistencia = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia where o.CelulaId == celulaId orderby o.Anio descending, o.Mes descending, o.Dia descending select new { Anio = o.Anio, Mes = o.Mes, Dia = o.Dia }).FirstOrDefault();
            var primeraAsistencia = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia where o.CelulaId == celulaId orderby o.Anio ascending, o.Mes ascending, o.Dia ascending select new { Anio = o.Anio, Mes = o.Mes, Dia = o.Dia }).FirstOrDefault();

            // Obtenemos las cancelaciones registradas (primera y ultima)
            var ultimaCancelacion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia where o.CelulaId == celulaId orderby o.Anio descending, o.Mes descending, o.Dia descending select new { Anio = o.Anio, Mes = o.Mes, Dia = o.Dia }).FirstOrDefault();
            var primeraCancelacion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCancelacionAsistencia where o.CelulaId == celulaId orderby o.Anio ascending, o.Mes ascending, o.Dia ascending select new { Anio = o.Anio, Mes = o.Mes, Dia = o.Dia }).FirstOrDefault();

            // Obtenemos las fechas de las asistencias registradas (primera y ultima)
            DateTime fechaDeUltimaAsistencia = ultimaAsistencia != null ? new DateTime(ultimaAsistencia.Anio, ultimaAsistencia.Mes, ultimaAsistencia.Dia) : DateTime.MinValue;
            DateTime fechaDePrimeraAsistencia = primeraAsistencia != null ? new DateTime(primeraAsistencia.Anio, primeraAsistencia.Mes, primeraAsistencia.Dia) : DateTime.MinValue;

            // Obtenemos las fechas de las cancelaciones registradas (primera y ultima)
            DateTime fechaDeUltimaCancelacion = ultimaCancelacion != null ? new DateTime(ultimaCancelacion.Anio, ultimaCancelacion.Mes, ultimaCancelacion.Dia) : DateTime.MinValue;
            DateTime fechaDePrimeraCancelacion = primeraCancelacion != null ? new DateTime(primeraCancelacion.Anio, primeraCancelacion.Mes, primeraCancelacion.Dia) : DateTime.MinValue;

            // Si no tiene registradas asistencias o cancelaciones, la fecha de la ultima asistencia es la fecha de creacion de la celula en si...
            if (primeraAsistencia == null && primeraCancelacion == null)
            {
                return fechaDeCreacion.Date;
            }
            else
            {
                return new DateTime(Math.Max(fechaDeUltimaAsistencia.Ticks, fechaDeUltimaCancelacion.Ticks)).Date;
            }
        }

        public DateTime ObtenerFechaDeSiguienteAsistencia(int celulaId)
        {
            return this.ObtenerFechaDeUltimaAsistencia(celulaId).AddDays(7);
        }

        public List<UltimasAsistenciasPorCelula> ObtenerUltimasAsistenciasPorCelula(int celulaId)
        {
            List<UltimasAsistenciasPorCelula> asistencias = new List<UltimasAsistenciasPorCelula>();

            ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
            List<int> celulas = manejadorDeCelulas.ObtenerRedInferior(celulaId);
            celulas.Add(celulaId); //Agregamos la celula actual para incluir sus lideres

            ManejadorDeAsistenciasDeCelula manejadorDeAsistenciasDeCelula = new ManejadorDeAsistenciasDeCelula();
            foreach (int celula in celulas)
            {
                Celula c = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == celula select o).SingleOrDefault();
                asistencias.Add(new UltimasAsistenciasPorCelula
                {
                    Id = c.CelulaId,
                    Descripcion = c.Descripcion,
                    Fecha = manejadorDeAsistenciasDeCelula.ObtenerFechaDeUltimaAsistencia(c.CelulaId)
                });
            }

            return asistencias;
        }

        private void determinarSiFechaEsPermitida(DateTime fecha, int celulaId)
        {

            fecha = fecha.Date; //Borramos la HORA
            DateTime fechaDeUltimaAsistencia = ObtenerFechaDeUltimaAsistencia(celulaId);
            DateTime fechaMinimaPermitida = fechaDeUltimaAsistencia.AddDays(diasPermitidosEntreAsistencias - diasDeMargenEntreAsistencias).Date;
            DateTime fechaMaximaPermitida = fechaDeUltimaAsistencia.AddDays(diasPermitidosEntreAsistencias + diasDeMargenEntreAsistencias).Date;

            // Corte anual
            if (fechaMinimaPermitida.Year < fecha.Year)
            {
                fechaMinimaPermitida = new DateTime(fecha.Year, 1, 1).Date;
                fechaMaximaPermitida = fecha.Date;
            }

            if (fecha != fechaDeUltimaAsistencia) //Si es la fecha de la ultima asistencia SI te deja modificar... asi se puede modificar solo lo ultimo registrado
            {
                if ((fecha < fechaDeUltimaAsistencia) && !(SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarBorrarAsistencias, false)))
                {
                    throw new ExcepcionReglaNegocio(string.Format(Literales.AsistenciasAntesDeUltimaAsistenciaNoValidas, fechaDeUltimaAsistencia.ToFullDateString()));
                }
                else if ((fecha < fechaMinimaPermitida) && !(SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarBorrarAsistencias, false)))
                {
                    throw new ExcepcionReglaNegocio(string.Format(Literales.AsistenciasContiguasNoPermitidas, fechaMinimaPermitida.ToFullDateString()));
                }
                else if (fecha > fechaMaximaPermitida)
                {
                    throw new ExcepcionReglaNegocio(string.Format(Literales.AsistenciasPasadasFaltantes, fechaDeUltimaAsistencia.AddDays(diasPermitidosEntreAsistencias).ToFullDateString()));
                }
            }
        }

        #endregion

        #region Reiniciar Asistencias de Celula

        /// <summary>
        /// Esta funcion reinicia la celula a la fecha determinada, es decir, borra todas las asistencias/cancelaciones registradas y cambia la fecha de creacion de la celula
        /// </summary>
        /// <param name="celulaId"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool ReiniciarAsistenciaDeCelula(int celulaId, DateTime fecha)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            SesionActual.Instance.getContexto<IglesiaEntities>().ReiniciarAsistenciaDeCelula(celulaId, anioSeleccionado, mesSeleccionado, diaSeleccionado, SesionActual.Instance.UsuarioId);
            return true;
        }

        #endregion
    }
}
