using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using ZagueEF.Core.Web.ExtNET;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;
using log4net;


using System.Web.Caching;

namespace AdministradorDeIglesiasV2.Website.WebServices.Notificaciones
{
    public partial class FaltaDeAsistencias : JsonWebServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FaltaDeAsistencias));

        protected override void OnLoad(EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");
            base.OnLoad(e);
        }

        protected override void OnInit(EventArgs e)
        {
            this.ValidarNumeroDeEjecuciones = true;
            base.OnInit(e);
        }

        public override System.Data.Objects.DataClasses.EntityObject IniciarSesion()
        {
            //Iniciamos sesion del usuario especificado para SOLO LECTURA del sistema
            ManejadorDeMiembros manejador = new ManejadorDeMiembros();
            return manejador.IniciarSesion(WebConfigurationManager.AppSettings["EmailDeUsuarioDeSoloLectura"], WebConfigurationManager.AppSettings["PwdDeUsuarioDeSoloLectura"]);
        }

        public override string ManejadorBase()
        {
            ManejadorDeReportesDeAsistencias manejador = new ManejadorDeReportesDeAsistencias();

            string jsonResult;
            try
            {
                bool correosEnviados = manejador.NotificarFaltaDeAsistenciasPorEmail();

                jsonResult = (new
                {
                    success = true,
                    seEnviaranCorreos = correosEnviados
                }).ToJson();

                log.InfoFormat("Reportes de falta de registro de asistencias procesados correctamente. Se enviaran correos? {0} ", correosEnviados.ToString().ToLowerInvariant());
            }
            catch (Exception ex)
            {
                jsonResult = (new
                {
                    success = false,
                    seEnviaranCorreos = false,
                    error = ex.Message + " [STACKTRACE] " + ex.StackTrace + (ex.InnerException != null ? " [INNEREXCEPTION] " + ex.InnerException.Message + " [INNEREXCEPTIONSTACKTRACE] " + ex.InnerException.StackTrace : string.Empty)
                }).ToJson();

                log.Info("Ocurrio algun problema al procesar los reportes de falta de registro de asistencias", ex);
            }

            return jsonResult;
        }
    }
}