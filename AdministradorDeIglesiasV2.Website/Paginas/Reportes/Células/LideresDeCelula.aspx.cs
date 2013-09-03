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


namespace AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas
{
    public partial class LideresDeCelula : PaginaBase
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
            StoreCelulas.Cargar(manejador.ObtenerRedesPermitidasPorMiembro(SesionActual.Instance.UsuarioId));
        }

        [DirectMethod(ShowMask = true)]
        public void ObtenerLideresDeCelulaClick()
        {
            int celulaSeleccionada;
            if (int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada))
            {
                ManejadorDeLideresDeCelula manejador = new ManejadorDeLideresDeCelula();
                StoreLideres.Cargar(manejador.ObtenerLideresDeCelulaPorRed(celulaSeleccionada));
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
            }
        }
    }
}