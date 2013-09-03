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
    public partial class PruebaDeCorreo : JsonWebServiceBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PruebaDeCorreo));

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
            this.ValidarNumeroDeEjecuciones = false;
            this.OcupaIniciarSesion = false;
            base.OnInit(e);
        }

        public override string ManejadorBase()
        {
            ManejadorDeCorreos manejador = new ManejadorDeCorreos();

            string jsonResult;
            try
            {
                manejador.ProbarCorreoAsync(WebConfigurationManager.AppSettings["RemitenteDeCorreos"], WebConfigurationManager.AppSettings["ServidorDeCorreos"], "Esto es una prueba del correo", "Asunto de Prueba", "david.campos.d@gmail.com");
                
                jsonResult = (new
                {
                    success = true,
                    seEnviaranCorreos = true
                }).ToJson();

                log.InfoFormat("Prueba de correo existosa!");
            }
            catch (Exception ex)
            {
                jsonResult = (new
                {
                    success = false,
                    seEnviaranCorreos = false,
                    error = ex.Message + " [STACKTRACE] " + ex.StackTrace + (ex.InnerException != null ? " [INNEREXCEPTION] " + ex.InnerException.Message + " [INNEREXCEPTIONSTACKTRACE] " + ex.InnerException.StackTrace : string.Empty)
                }).ToJson();

                log.Error("Ocurrio algun problema al probar el envio de correos.", ex);
            }

            return jsonResult;
        }
    }
}