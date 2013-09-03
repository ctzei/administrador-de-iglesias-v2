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
    public partial class UltimasAsistencias : PaginaBase
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
        public void ObtenerUltimasAsistenciasPorCelulaClick()
        {
            int celulaSeleccionada;
            if (int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada))
            {
                ManejadorDeAsistenciasDeCelula manejadorDeAsistenciasDeCelula = new ManejadorDeAsistenciasDeCelula();
                StoreAsistencias.Cargar(manejadorDeAsistenciasDeCelula.ObtenerUltimasAsistenciasPorCelula(celulaSeleccionada).OrderBy(o => o.Fecha));
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
            }
        }

    }
}