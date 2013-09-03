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
    public partial class BorrarCelulaPermanentemente : PaginaBase
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

        [DirectMethod(ShowMask = true, Timeout = (10 * 60 * 1000))]
        public void BorrarCelulaPermanentementeClick()
        {
            int celulaSeleccionada;
            if (int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada))
            {
                ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
                manejadorDeCelulas.BorrarCelulaPermanentemente(celulaSeleccionada);
                X.Msg.Alert(Generales.nickNameDeLaApp, "Celula borrarda permanentemente!").Show();
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaNecesaria).Show();
            }
        }
    }
}