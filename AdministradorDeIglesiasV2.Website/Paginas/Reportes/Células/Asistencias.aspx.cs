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


namespace AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas
{
    public partial class Asistencias : PaginaBase
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

            DateTime now = DateTime.Now;
            DateTime primerDiaDelMesPasado = new DateTime(now.Year, now.Month, 1).AddMonths(-1);

            dtpFechaInicial.SelectedDate = primerDiaDelMesPasado;
            dtpFechaFinal.SelectedDate = now;
        }

        [DirectMethod(ShowMask = true, Timeout = 120000)]
        public string ObtenerInformacionGeneralPorRed()
        {
            int celulaSeleccionada;
            if (int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada) && dtpFechaInicial.SelectedDate.Year > 1900 && dtpFechaFinal.SelectedDate.Year > 1900)
            {
                ManejadorDeReportesDeAsistencias manejador = new ManejadorDeReportesDeAsistencias();
                return manejador.ObtenerInformacionGeneralPorRed(celulaSeleccionada).ToJson();
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
                return string.Empty;
            }
        }

        [DirectMethod(ShowMask = true, Timeout = 120000)]
        public static string ObtenerReporteDeAsistencias(int celulaId, string sFechaInicial, string sFechaFinal)
        {
            DateTime fechaInicial;
            DateTime fechaFinal;

            if (DateTime.TryParse(sFechaInicial, out fechaInicial) == false || DateTime.TryParse(sFechaFinal, out fechaFinal) == false)
            {
                fechaInicial = DateTime.MinValue;
                fechaFinal = DateTime.MinValue;
            }

            if ((celulaId > 0) && (fechaInicial.Year > 1900 && fechaFinal.Year > 1900))
            {
                ManejadorDeReportesDeAsistencias manejador = new ManejadorDeReportesDeAsistencias();
                return manejador.ObtenerReporteDeAsistenciasPorCelula(celulaId, fechaInicial, fechaFinal, false).ToJson();
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
                return string.Empty;
            }
        }
    }
}