using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ExtensionMethods;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;
using log4net;

namespace AdministradorDeIglesiasV2.Website
{
    public partial class MainMasterPageBootstrap : MasterPageBase
    {
        private static readonly ILog log = LogManager.GetLogger("Usuario");

        protected override void OnInit(EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                validarPaginasPermitidas();
            }
            base.OnInit(e);
        }

        private void validarPaginasPermitidas()
        {
            bool esPaginaDeContenido = Regex.IsMatch(Request.Url.AbsolutePath, @"/Paginas/([^""]+)/", RegexOptions.Compiled);
            if (esPaginaDeContenido)
            {
                bool estaPermitida = SesionActual.Instance.PantallasPermitidas.Contains(Request.Url.Segments.Last());
                if (!estaPermitida)
                {
                    log.WarnFormat("El usuario [{0}] intento accessar una pantalla no permitida: {1}", Page.User.Identity.Name, Request.Url);

                    ((PaginaBase)this.Page).TerminarSesion();

                    //Obtenemos la URL de la pagina de login, sino de la misma pagina original, MAS el parametro de pantalla no permitida
                    string rtnUrl = Page.ResolveUrl(WebConfigurationManager.AppSettings["UrlDePaginaDeLogin"]);
                    if (string.IsNullOrEmpty(rtnUrl)){
                        rtnUrl = Request.Url.OriginalString;
                    }

                    Uri uri = new Uri(rtnUrl, UriKind.RelativeOrAbsolute);
                    Response.Redirect(uri.ToString() + "?pantallaNoPermitida=1");
                }
            }
        }
    }
}