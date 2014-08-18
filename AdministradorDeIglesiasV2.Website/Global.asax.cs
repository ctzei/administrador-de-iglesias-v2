using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using System.Web.Routing;
using ZagueEF.Core;
using System.Reflection;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;
using log4net;
using Quartz;
using Quartz.Impl;
using AdministradorDeIglesiasV2.Core.ScheduledJobs;

namespace AdministradorDeIglesiasV2.Website
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Global));
        private IScheduler scheduler;

        protected void Application_Start(object sender, EventArgs e)
        {
            // log4net initialization
            log4net.Config.XmlConfigurator.Configure();

            // init jobs
            InitScheduledJobs();

            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            scheduler.Shutdown();
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

        private void InitScheduledJobs()
        {
            // Quartz.Net initialization
            log.Debug("Starting Quartz.Net engine");
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            #region NotificacionDeNuevasBoletasDeConsolidacion

            IJobDetail jobNotificacionDeNuevasBoletasDeConsolidacion = JobBuilder.Create<NotificacionDeNuevasBoletasDeConsolidacion>()
                .Build();

            ITrigger triggerNotificacionDeNuevasBoletasDeConsolidacion = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(x => x
                    .WithIntervalInHours(24)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(23, 0))) // Corre todos los dias a las 11PM
                .Build();

            scheduler.ScheduleJob(jobNotificacionDeNuevasBoletasDeConsolidacion, triggerNotificacionDeNuevasBoletasDeConsolidacion);

            #endregion

            #region NotificacionDeFaltaDeRegistroDeAsistencia

            IJobDetail jobNotificacionDeFaltaDeRegistroDeAsistencia = JobBuilder.Create<NotificacionDeFaltaDeRegistroDeAsistencia>()
                .Build();

            ITrigger triggerNotificacionDeFaltaDeRegistroDeAsistencia = TriggerBuilder.Create()
                .WithCronSchedule("0 0,30 * * * ?") // Corre cada hora y cada minuto 30 de cada hora
                .Build();

            scheduler.ScheduleJob(jobNotificacionDeFaltaDeRegistroDeAsistencia, triggerNotificacionDeFaltaDeRegistroDeAsistencia);

            #endregion

        }
    }
}