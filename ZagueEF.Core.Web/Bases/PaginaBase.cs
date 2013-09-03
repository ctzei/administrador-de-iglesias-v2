using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZagueEF.Core;
using log4net;

namespace ZagueEF.Core.Web
{
    public class PaginaBase : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger("Usuario");

        /// <summary>
        /// Este metodo sirve para desde cualquier pagina terminar la sesion (eliminando la cookie del lado del cliente, etc)
        /// </summary>
        public void TerminarSesion()
        {
            Session.Abandon();
            System.Web.Security.FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Este metodo sirve para eliminar las variables de la sesion, para borrar el cache para cuando se agregan nuevos registros (excepto el usuario actual)
        /// </summary>
        public void RestablecerCacheDeSesion()
        {
            List<string> keys = Session.Keys.Cast<string>().ToList();
            string[] keysPersistentes = 
            {
                SesionActual.Instance.UsuarioIdKey, 
                SesionActual.Instance.PantallasPermitidasKey, 
                SesionActual.Instance.PermisosEspecialesAsignadosKey,
                SesionActual.Instance.PermisosEspecialesKey
            };
            
            foreach (string key in keys)
            {
                if (!keysPersistentes.Contains(key))
                {
                    Session.Remove(key);
                }
            }

            Core.Cache.Instance.Limpiar();
        }
    }
}
