using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects.DataClasses;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;

namespace ZagueEF.Core.Web.ExtNET
{
    public class JsonWebServiceBase : PaginaBase
    {
        #region Constantes
        private const string HANDLER_NO_ESTABLECIDO = "No se encontro ningun parametro [handler] con el metodo a ejecutar. Es necesario establecer un metodo [handler] a ejecutar para poder continuar.";
        private const string HANDLER_NO_EXISTENTE = "El metodo establecido como parametro [handler], NO existe o no es un metodo accessible [publico].";
        private const int MINUTOS_ENTRE_EJECUCIONES_CONSECUTIVAS = 30;
        private const int NUMERO_MAX_DE_EJECUCIONES_POR_IP = 3;
        private string NUMERO_MAX_DE_EJECUCIONES_POR_IP_SOBREPASADO = string.Format("Para poder volver a usar este servicio necesita esperar por lo menos {0} min.", MINUTOS_ENTRE_EJECUCIONES_CONSECUTIVAS);
        #endregion

        public bool ValidarNumeroDeEjecuciones = true;
        public bool OcupaIniciarSesion = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/json";

            try
            {

                string handler = Request["handler"];
                string callback = Request["callback"];

                try
                {
                    validarNumeroDeEjecuciones();
                    if (this.OcupaIniciarSesion == true)
                    {
                        IniciarSesion();
                    }

                    string json;
                    if (string.IsNullOrEmpty(handler))
                    {
                        json = ManejadorBase();
                    }
                    else
                    {
                        MethodInfo minfo = this.GetType().GetMethod(handler);
                        if (minfo != null)
                        {
                            json = minfo.Invoke(this, null).ToString();
                        }
                        else
                        {
                            throw new ExcepcionReglaNegocio(HANDLER_NO_EXISTENTE);
                        }
                    }


                    #region Generamos el RESPONSE

                    if (string.IsNullOrEmpty(callback))
                    {
                        Response.Write(string.Format(@"{0}", json)); //JSON
                    }
                    else
                    {
                        Response.Write(string.Format(@"{0}({1})", callback, json)); //JSONP
                    }

                    #endregion
                }
                catch (ExcepcionReglaNegocio ex)
                {
                    string json = string.Format(@"{0}", (new { success = false, error = ex.Message }).ToJson());
                    if (string.IsNullOrEmpty(callback))
                    {
                        Response.Write(string.Format(@"{0}", json)); //JSON
                    }
                    else
                    {
                        Response.Write(string.Format(@"{0}({1})", callback, json)); //JSONP
                    }
                }

                if (this.OcupaIniciarSesion == true)
                {
                    this.TerminarSesion();
                }
            }
            finally
            {
                Response.End();
            }
        }

        public virtual EntityObject IniciarSesion()
        {
            throw new ExcepcionAplicacion("Es necesario que la clase tenga su propia implementacion del metodo IniciarSesion()");
        }

        public virtual string ManejadorBase()
        {
            throw new ExcepcionAplicacion("Es necesario que la clase tenga su propia implementacion del metodo ManejadorBase() o se pase el parametro por el REQUEST [handler]");
        }

        private void validarNumeroDeEjecuciones()
        {
            if (this.ValidarNumeroDeEjecuciones)
            {
                string cacheKey = string.Format("NUMERO_DE_EJECUCIONES-CLASE[{0}]-IP[{1}]", this.GetType().FullName, Request.UserHostAddress);
                int numeroDeEjecuciones = 0;

                if (Cache[cacheKey] != null)
                {
                    int.TryParse(Cache[cacheKey].ToString(), out numeroDeEjecuciones);
                }

                if (numeroDeEjecuciones < NUMERO_MAX_DE_EJECUCIONES_POR_IP)
                {
                    Cache.Remove(cacheKey);
                    Cache.Add(cacheKey, numeroDeEjecuciones + 1, null, DateTime.Now.AddMinutes(MINUTOS_ENTRE_EJECUCIONES_CONSECUTIVAS), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
                else
                {
                    throw new ExcepcionReglaNegocio(NUMERO_MAX_DE_EJECUCIONES_POR_IP_SOBREPASADO);
                }
            }
        }
    }
}