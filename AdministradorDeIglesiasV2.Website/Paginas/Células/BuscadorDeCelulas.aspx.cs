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
    public partial class BuscadorDeCelulas : PaginaBase
    {
        ManejadorDeCelulas manejadorDeCelulas;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }                                 
        }

        public void CargarControles()
        {
            StoreCelulaCategorias.Cargar(CelulaCategoria.Obtener());
        }

        [DirectMethod(ShowMask = true)]
        public List<Core.Modelos.Retornos.CelulaProxima> ObtenerCelulasProximas(double latitud, double longitud)
        {
            manejadorDeCelulas = new ManejadorDeCelulas();
            return manejadorDeCelulas.ObtenerCelulasProximas(latitud, longitud, Convert.ToInt32(filtroKilometros.Number), filtroCategoria.ObtenerIds());
        }
    }
}