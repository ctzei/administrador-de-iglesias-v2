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
    public partial class AdministracionDeCache : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            elementosEliminados.Value = string.Join(Environment.NewLine, ZagueEF.Core.Cache.Instance.ObtenerLlaves());
            ZagueEF.Core.Cache.Instance.Limpiar();
        }
    }
}