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
using AdministradorDeIglesiasV2.Core.Manejadores;


namespace AdministradorDeIglesiasV2.Website.Paginas
{
    public partial class ReinicioDeAsistenciaDeCelula : PaginaBase
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
            StoreCelulas.Cargar(manejador.ObtenerCelulasPermitidasPorMiembro(SesionActual.Instance.UsuarioId));
        }

        [DirectMethod(ShowMask = true)]
        public void ReinciarAsistenciaDeCelulaClick()
        {
            int celulaSeleccionada;
            if ((int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada)) && (dtpFecha.SelectedDate.Year > 1900))
            {
                ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();
                manejadorDeAsistencias.ReiniciarAsistenciaDeCelula(celulaSeleccionada, dtpFecha.SelectedDate);
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.AsistenciaDeCelulaReiniciada).Show();
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
            }
        }
    }
}