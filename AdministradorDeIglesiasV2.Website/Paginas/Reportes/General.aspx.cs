using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;
using Ext.Net;
using ExtensionMethods;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using ZagueEF.Core;
using ZagueEF.Core.Web;

namespace AdministradorDeIglesiasV2.Website.Paginas.Reportes
{
    public partial class General : PaginaBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(General));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }
        }

        public void CargarControles()
        {
            ManejadorDeCelulas manejador = new ManejadorDeCelulas();
            cboCelula.DataSource = manejador.ObtenerCelulasPermitidasPorMiembroComoCelulas(SesionActual.Instance.UsuarioId);
            cboCelula.DataBind();
        }

        [System.Web.Services.WebMethod]
        public static string ObtenerReporteGeneral(int celulaId)
        {

            if (celulaId > 0)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();

                List<int> celulasInferioresrDirectas = manejadorDeCelulas.ObtenerRedInferiorDirecta(celulaId);
                List<Celula> celulasDirectas = new List<Celula>() { (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == celulaId select o).SingleOrDefault() };
                celulasDirectas.AddRange((from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where celulasInferioresrDirectas.Contains(o.CelulaId) select o));

                List<dynamic> resultadosAnuales = new List<dynamic>();

                foreach (Celula celula in celulasDirectas.OrderBy(o => o.Descripcion))
                {
                    resultadosAnuales.Add(ObtenerResultadosAnualesParaGanar(celula));
                }

                stopwatch.Stop();
                log.Info("Segundos para generar el reporte: " + stopwatch.Elapsed.TotalSeconds);

                return resultadosAnuales.ToJson();
            }
            else
            {
                return (new { error =  Resources.Literales.CelulaYFechaNecesarias}).ToJson();
            }
        }

        private static dynamic ObtenerResultadosAnualesParaGanar(Celula celula)
        {
            dynamic rtn = new ExpandoObject();

            ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
            List<int> red = new List<int>() { celula.CelulaId };
            red.AddRange(manejadorDeCelulas.ObtenerRedInferior(celula.CelulaId));

            rtn.id = celula.CelulaId;
            rtn.nombre = celula.Descripcion;
            rtn.resultadoAnual = new List<dynamic>();

            DateTime fechaInicial = DateTime.Now.FirstMondayOfYear();
            DateTime fechaFinal = fechaInicial.AddYears(1).AddDays(-1);

            List<Celula> totalCelulas = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                                         where
                                         red.Contains(o.CelulaId) &&
                                         o.Creacion <= fechaFinal &&
                                         o.Borrado == false
                                         select o).ToList();

            List<CelulaMiembroAsistencia> totalAsistencias = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia
                                                             where
                                                             red.Contains(o.CelulaId) &&
                                                             EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fechaInicial &&
                                                             EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaFinal
                                                             select o).ToList();

            List<CelulaInvitadosAsistencia> totalInvitados = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaInvitadosAsistencia
                                                              where
                                                              red.Contains(o.CelulaId) &&
                                                              EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fechaInicial &&
                                                              EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaFinal
                                                              select o).ToList();

            DateTime fecha = fechaInicial;
            while (fecha < DateTime.Now)
            {
                dynamic resultadoSemanal = new ExpandoObject();

                DateTime fechaSiguiente = fecha.Date.AddDays(7);

                resultadoSemanal.semana = fecha.GetWeekNumber();

                resultadoSemanal.activas = (from o in totalCelulas
                                            where
                                            o.Creacion <= fechaSiguiente
                                            select o).Count();

                resultadoSemanal.realizadas = (from o in totalAsistencias
                                               where
                                               new DateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fecha &&
                                               new DateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaSiguiente
                                               select o.CelulaId).Distinct().Count();


                resultadoSemanal.invitados = (from o in totalInvitados
                                              where
                                              new DateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fecha &&
                                              new DateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaSiguiente
                                              select (int?)o.NumeroDeInvitados).Sum() ?? 0;

                resultadoSemanal.asistencia = (from o in totalAsistencias
                                               where
                                               new DateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fecha &&
                                               new DateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaSiguiente
                                               select o).Count();

                // Agregamos el resultado semanal al resultado anual
                rtn.resultadoAnual.Add(resultadoSemanal);

                // Una semana mas
                fecha = fecha.AddDays(7);
            }

            return rtn;
        }
    }
}