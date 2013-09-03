using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Web;

namespace ZagueEF.Core
{
    public sealed class SesionActual
    {

        private static volatile SesionActual instance;
        private static object syncRoot = new Object();

        public string UsuarioIdKey = "ZagueEF.Core.UsuarioId";
        public string PantallasPermitidasKey = "ZagueEF.Core.PantallasPermitidas";
        public string PermisosEspecialesAsignadosKey = "ZagueEF.Core.PermisosEspecialesAsignados";
        public string PermisosEspecialesKey = "ZagueEF.Core.PermisosEspeciales";

        /// <summary>
        /// Aqui se inicializan TODAS las variables cuando se instancia por primera vez la "Sesion Actual" 
        /// </summary>
        private SesionActual()
        {
        }

        public static SesionActual Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SesionActual();
                    }
                }

                return instance;
            }
        }

        public T getContexto<T>() where T : new()
        {
            validarRequest();
            string objectContextKey = "CONTEXTO_" + HttpContext.Current.GetHashCode().ToString("x");
            if (!HttpContext.Current.Items.Contains(objectContextKey))
            {
                HttpContext.Current.Items.Add(objectContextKey, new T());
            }
            return (T)HttpContext.Current.Items[objectContextKey];
        }

        #region Propiedades Publicas

        public int UsuarioId
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return -1;
                }

                int usuarioId;
                object oUsuarioId = HttpContext.Current.Session[UsuarioIdKey];

                if (oUsuarioId == null)
                {
                    oUsuarioId = -1;
                }

                if (int.TryParse(oUsuarioId.ToString(), out usuarioId) == false)
                {
                    throw new ExcepcionAplicacion(Literales.UsuarioIdSinFormatoCorrecto);
                }
                return usuarioId;
            }
            set
            {
                validarRequest();
                HttpContext.Current.Session[UsuarioIdKey] = value;
            }
        }

        public List<string> PantallasPermitidas
        {
            get
            {
                validarRequest();
                object oPantallasPermitidas = HttpContext.Current.Session[PantallasPermitidasKey];

                if (oPantallasPermitidas == null)
                {
                    oPantallasPermitidas = new List<string>();
                }

                return (List<string>)oPantallasPermitidas;
            }
            set
            {
                validarRequest();
                HttpContext.Current.Session[PantallasPermitidasKey] = value;
            }
        }

        #endregion

        #region Permisos Especiales

        #region Propiedades Privadas

        /// <summary>
        /// Esta propiedad guarda SOLO los IDs de los permisos asignados, para el usuario actual
        /// </summary>
        private List<int> PermisosEspecialesAsignados
        {
            get
            {
                object oPermisosEspeciales = HttpContext.Current.Session[PermisosEspecialesAsignadosKey];

                if (oPermisosEspeciales == null)
                {
                    oPermisosEspeciales = new List<int>();
                }

                return (List<int>)oPermisosEspeciales;
            }
            set
            {
                HttpContext.Current.Session[PermisosEspecialesAsignadosKey] = value;
            }
        }

        /// <summary>
        /// Esta propiedad guarda TODOS los permisos especiales que se encuentran en la BD
        /// </summary>
        private Dictionary<int, string> PermisosEspeciales
        {
            get
            {
                object oPermisosEspeciales = HttpContext.Current.Session[PermisosEspecialesKey];

                if (oPermisosEspeciales == null)
                {
                    oPermisosEspeciales = new Dictionary<int, string>();
                }

                return (Dictionary<int, string>)oPermisosEspeciales;
            }
            set
            {
                HttpContext.Current.Session[PermisosEspecialesKey] = value;
            }
        }

        #endregion

        /// <summary>
        /// Con este metodo se INICIALIZAN los permisos especiales
        /// </summary>
        /// <param name="permisosEspecialesAsignados">Es una lista de los IDs de los permisos especiales que tiene asignado el usuario actual</param>
        /// <param name="permisosEspeciales">Es la lista de TODOS los permisos especiales del sistema, con ID y DESCRIPCION</param>
        public void CargarPermisosEspeciales(List<int> permisosEspecialesAsignados, Dictionary<int, string> permisosEspeciales)
        {
            if ((permisosEspecialesAsignados != null) && (permisosEspeciales != null))
            {
                this.PermisosEspecialesAsignados = permisosEspecialesAsignados;
                this.PermisosEspeciales = permisosEspeciales;
            }
            else
            {
                throw new ExcepcionAplicacion("Al momento de cargar los permisos especiales, ninguno de los parametros de entrada puede ser nulo.");
            }
        }

        public bool ValidarPermisoEspecial(int idPermisoEspecial)
        {
            return this.ValidarPermisoEspecial(idPermisoEspecial, true);
        }

        public bool ValidarPermisoEspecial(int idPermisoEspecial, bool mostrarExcepcion)
        {
            if (this.PermisosEspecialesAsignados.Contains(idPermisoEspecial) == false)
            {
                string permisoFaltante;
                if (this.PermisosEspeciales.TryGetValue(idPermisoEspecial, out permisoFaltante))
                {
                    if (mostrarExcepcion)
                    {
                        throw new ExcepcionReglaNegocio(string.Format("El usuario actual no cuenta con los permisos suficientes para realizar dicha acción. Permiso faltante: [{0}]", permisoFaltante));
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    throw new ExcepcionAplicacion(string.Format("Permiso especial inexistente en la BD; es necesario agregarlo. Id del permiso especial requerido: {0}", idPermisoEspecial));
                }
            }
            return true;
        }

        #endregion

        private static void validarRequest()
        {
            if (HttpContext.Current == null)
            {
                throw new ExcepcionAplicacion(string.Format("El objeto {0} no puede ser usado fuera del 'request' principal (no se puede usar dentro de 'threads'...)", typeof(SesionActual).FullName));
            }
        }

    }
}
