using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using ZagueEF.Core;
using System.Reflection;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;
using log4net;

namespace AdministradorDeIglesiasV2.Website
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            // log4net initialization
            log4net.Config.XmlConfigurator.Configure();

            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception err = Server.GetLastError();
            log.Error("An unhandled exception occurred", err);

            if ((err is HttpUnhandledException) && (!HttpContext.Current.Request.Url.Host.ToLower().Contains("localhost")))
            {
                HttpContext.Current.Session["LastError"] = err;
                Server.ClearError();
                Response.Redirect("~/Errores/Error.aspx", true);
            }
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            CargarUsuarioDesdeCookie();
        }

        private void CargarUsuarioDesdeCookie()
        {
            if (HttpContext.Current.CurrentHandler is ZagueEF.Core.Web.PaginaBase)
            {
                //Si el usuario NO tiene sesion en el servidor, PERO tiene sesion en el cliente (COOKIE), intentamos iniciar sesion
                if ((HttpContext.Current.User.Identity.IsAuthenticated) && (SesionActual.Instance.UsuarioId < 0))
                {
                    ManejadorDeMiembros manejador = new ManejadorDeMiembros();
                    SesionActual.Instance.UsuarioId = ((Miembro)manejador.IniciarSesion(HttpContext.Current.User.Identity.Name, string.Empty, true)).MiembroId;
                }
            }
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Main", "", "~/Paginas/Main.aspx");
            routes.MapPageRoute("Login", "Login/{*queryvalues}", "~/Paginas/Login.aspx");
            routes.MapPageRoute("Inscripciones", "Inscripcion", "~/PaginasExternas/Eventos/Inscripciones/Inscripcion.html");
            routes.MapPageRoute("Error", "Error", "~/Errores/Error.aspx");
            routes.MapPageRoute("404", "404", "~/Errores/404.aspx");
        }
    }
}