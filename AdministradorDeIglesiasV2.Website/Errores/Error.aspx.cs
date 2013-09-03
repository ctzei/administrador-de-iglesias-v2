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
    public partial class Error : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception serverError = Server.GetLastError();
            Exception sessionError = (Exception)Session["LastError"];

            if (serverError != null)
            {
                errorMsg.Value = _getErrorMsg(serverError);
            }
            else if (sessionError != null)
            {
                errorMsg.Value = _getErrorMsg(sessionError);
            }
        }

        private string _getErrorMsg(Exception ex)
        {
            return ex.Message + " - " + ex.StackTrace + " - " + ((ex.InnerException != null) ? ex.InnerException.Message : string.Empty);
        }
    }
}