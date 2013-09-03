using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZagueEF.Core;
using System.Reflection;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;
using log4net;

namespace AdministradorDeIglesiasV2.Website.Mobile
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            // log4net initialization
            log4net.Config.XmlConfigurator.Configure();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception err = Server.GetLastError();
            log.Error("An unhandled exception occurred", err);
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            CargarUsuarioDesdeCookie();
        }

        private void CargarUsuarioDesdeCookie()
        {
            //if (HttpContext.Current.CurrentHandler is ZagueEF.Core.Web.ControladorBase)
            if (HttpContext.Current.Session != null)
            {
                //Si el usuario NO tiene sesion en el servidor, PERO tiene sesion en el cliente (COOKIE), intentamos iniciar sesion
                if ((HttpContext.Current.User.Identity.IsAuthenticated) && (SesionActual.Instance.UsuarioId < 0))
                {
                    ManejadorDeMiembros manejador = new ManejadorDeMiembros();
                    SesionActual.Instance.UsuarioId = ((Miembro)manejador.IniciarSesion(HttpContext.Current.User.Identity.Name, string.Empty, true)).MiembroId;
                }
            }
        }
    }
}