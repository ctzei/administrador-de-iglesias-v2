using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Modelos.Retornos;
using AdministradorDeIglesiasV2.Core.Manejadores;
using System.Dynamic;
using System.Data.Objects;


namespace AdministradorDeIglesiasV2.Website.Paginas.Reportes
{
    public partial class General : PaginaBase
    {
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
            StoreCelulas.DataSource = manejador.ObtenerCelulasPermitidasPorMiembroComoCelulas(SesionActual.Instance.UsuarioId);
            StoreCelulas.DataBind();
        }

        [DirectMethod(ShowMask = true, Timeout = 120000)]
        public static string ObtenerReporteGeneral(int celulaId)
        {

            if (celulaId > 0)
            {
                ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
                List<int> red = new List<int>() { celulaId };
                red.AddRange(manejadorDeCelulas.ObtenerRedInferior(celulaId));

                List<dynamic> resultadoAnual = new List<dynamic>();

                DateTime fecha = DateTime.Now.FirstMondayOfYear();
                while (fecha < DateTime.Now)
                {
                    dynamic resultadoSemanal = new ExpandoObject();

                    DateTime fechaSiguiente = fecha.Date.AddDays(7);

                    resultadoSemanal.semana = fecha.GetWeekNumber();

                    resultadoSemanal.activas = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                                                            where
                                                            red.Contains(o.CelulaId) &&
                                                            o.Creacion <= fechaSiguiente &&
                                                            o.Borrado == false
                                                            select o).Count();

                    resultadoSemanal.realizadas = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia
                                                where
                                                red.Contains(o.CelulaId) &&
                                                EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fecha &&
                                                EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaSiguiente
                                                select o.CelulaId).Distinct().Count();


                    resultadoSemanal.invitados = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaInvitadosAsistencia
                                                  where
                                                  red.Contains(o.CelulaId) &&
                                                  EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fecha &&
                                                  EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaSiguiente
                                                  select (int?)o.NumeroDeInvitados).Sum() ?? 0;

                    resultadoSemanal.asistencia = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaMiembroAsistencia
                                                   where
                                                   red.Contains(o.CelulaId) &&
                                                   EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) > fecha &&
                                                   EntityFunctions.CreateDateTime(o.Anio, o.Mes, o.Dia, 0, 0, 0) <= fechaSiguiente
                                                   select o).Count();

                    //resultadoSemanal.fechaInicial = fecha;
                    //resultadoSemanal.fechaFinal = fechaSiguiente;


                    // Agregamos el resultado semanal al resultado anual
                    resultadoAnual.Add(resultadoSemanal);

                    // Una semana mas
                    fecha = fecha.AddDays(7);
                }

                return resultadoAnual.ToJson();
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
                return string.Empty;
            }
        }
    }
}