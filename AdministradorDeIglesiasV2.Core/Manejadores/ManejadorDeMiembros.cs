using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ZagueEF.Core;
using log4net;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeMiembros
    {
        private static readonly ILog log = LogManager.GetLogger("Usuario");

        public static Miembro ObtenerMiembroActual()
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == SesionActual.Instance.UsuarioId select o).SingleOrDefault();
        }

        #region Inicio de Sesion

        public Miembro IniciarSesion(string email, string password)
        {
            return IniciarSesion(email, password, false);
        }

        public Miembro IniciarSesion(string email, string password, bool usarSoloEmail)
        {
            Validaciones.ValidarEmail(email);

            Miembro usuario;
            try
            {
                usuario = (from u in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where u.Borrado == false && u.Email == email && ((usarSoloEmail == false) ? u.Contrasena == password : 1 == 1) select u).FirstOrDefault();
            }
            catch (System.Data.EntityException ex)
            {
                throw new ExcepcionAplicacion("Ocurrio un error con la conexion a la BD; es posible que no se encuentre inicializada.", ex);
            }

            if (usuario == null)
            {
                log.InfoFormat("Inicio de sesion NO exitoso del usuario: {0}", email);
                throw new ExcepcionReglaNegocio(Literales.UsuarioYPasswordInvalidos);
            }
            else
            {
                SesionActual.Instance.UsuarioId = usuario.MiembroId;
                SesionActual.Instance.PantallasPermitidas = (from p in ObtenerPantallasPermitidasPorMiembro(SesionActual.Instance.UsuarioId, "WebV2").Union(ObtenerPantallasPermitidasPorMiembro(SesionActual.Instance.UsuarioId, "WebMobileV2")) select p.Nombre_Tecnico).ToList<string>();
                SesionActual.Instance.CargarPermisosEspeciales(
                    (from p in ObtenerPermisosEspecialesPorMiembro(SesionActual.Instance.UsuarioId) select p.PermisoEspecialId).ToList<int>(),
                    ObtenerPermisosEspecialesPorSistema());
            }

            log.InfoFormat("Inicio de sesion exitoso del usuario: {0} [{1}] {2}", email, SesionActual.Instance.UsuarioId, (usarSoloEmail == true ? "(soloEmail)" : string.Empty));
            return usuario;
        }

        #endregion

        #region Cambiar Contraseña

        public void CambiarContrasena(string email, string oldPassword, string newPassword)
        {
            Miembro miembro = IniciarSesion(email, oldPassword);
            if (miembro != null)
            {
                miembro.Contrasena = newPassword;
                miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                log.InfoFormat("Cambio de contraseña exitoso del usuario: {0} [{1}]", email, miembro.MiembroId);
            }
        }

        #endregion

        #region Pasos

        /// <summary>
        /// Su funcion es regresar la lista de todos los pasos dados para un miembro en especifico
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public List<Modelos.Retornos.PasoPorMiembro> ObtenerPasosPorMiembro(int miembroId)
        {
            return (
                from pasosPorMiembros in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroPaso
                orderby pasosPorMiembros.Paso.PasoCategoria.Descripcion, pasosPorMiembros.Paso.Descripcion
                where pasosPorMiembros.MiembroId == miembroId && pasosPorMiembros.Borrado == false
                select new Modelos.Retornos.PasoPorMiembro
                {
                    CicloId = pasosPorMiembros.CicloId,
                    Ciclo = pasosPorMiembros.Ciclo.Descripcion,
                    PasoId = pasosPorMiembros.PasoId,
                    Paso = pasosPorMiembros.Paso.Descripcion,
                    CategoriaId = pasosPorMiembros.Paso.PasoCategoriaId,
                    Categoria = pasosPorMiembros.Paso.PasoCategoria.Descripcion
                }).ToList<Modelos.Retornos.PasoPorMiembro>();
        }

        public bool GuardarPasosPorMiembro(int miembroId, RegistrosHelper.RegistrosDeDatos pasosNuevosYEliminados)
        {
            bool rtn = false;

            if (miembroId > 0 && (pasosNuevosYEliminados.RegistrosNuevosId.Count > 0 || pasosNuevosYEliminados.RegistrosEliminadosId.Count > 0))
            {
                Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroId select o).FirstOrDefault();
                MiembroPaso miembroPaso;

                if (miembro == null) { throw new ExcepcionReglaNegocio(Literales.RegistroInexistente); }

                //Si tenemos registros nuevos...
                if (pasosNuevosYEliminados.RegistrosNuevosId.Count > 0)
                {
                    //Agregamos los nuevos pasos celulas (siempre y cuando no existan previamente...)
                    foreach (KeyValuePair<int, Dictionary<string, string>> pasoObj in pasosNuevosYEliminados.RegistrosNuevos)
                    {
                        if (!miembro.MiembroPaso.Any(o => o.PasoId == pasoObj.Key && o.Borrado == false))
                        {
                            int cicloId;
                            if (!int.TryParse(pasoObj.Value["CicloId"], out cicloId))
                            {
                                Paso paso = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Paso where o.PasoId == pasoObj.Key select o).FirstOrDefault();

                                if (paso != null)
                                {
                                    throw new ExcepcionReglaNegocio(string.Format("El paso [{0}] no cuenta con un ciclo valido; favor de corregirlo y volver a intentar.", paso.Descripcion));
                                }
                                else
                                {
                                    throw new ExcepcionReglaNegocio(string.Format("El paso cuyo id es [{0}] es inexistente; favor de corregirlo y volver a intentar.", pasoObj.Key));
                                }
                            }

                            miembroPaso = new MiembroPaso();
                            miembroPaso.MiembroId = miembroId;
                            miembroPaso.PasoId = pasoObj.Key;
                            miembroPaso.CicloId = cicloId;
                            miembro.MiembroPaso.Add(miembroPaso);
                        }
                    }

                    //Guardamos los cambios, antes de eliminar registros
                    miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }

                //Eliminamos los pasos eliminados
                foreach (int pasoId in pasosNuevosYEliminados.RegistrosEliminadosId)
                {
                    miembroPaso = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroPaso where o.PasoId == pasoId && o.MiembroId == miembroId && o.Borrado == false select o).FirstOrDefault();
                    if (miembroPaso != null)
                    {
                        miembroPaso.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    }
                }
            }

            return rtn;
        }

        #endregion

        #region Seguridad

        public List<int> ObtenerRolesPorMiembro(int miembroId)
        {
            return (from mr in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroRol where mr.MiembroId == miembroId select mr.RolId).ToList<int>();
        }

        public List<PantallaPermitida> ObtenerPantallasPermitidasPorMiembro(int miembroId, string appId)
        {
            List<int> rolesPorMiembro = this.ObtenerRolesPorMiembro(miembroId);

            return (from rpp in SesionActual.Instance.getContexto<IglesiaEntities>().RolPantallaPermitida
                    where
                        rolesPorMiembro.Contains(rpp.RolId) &&
                        rpp.PantallaPermitida.AppId == appId
                    orderby
                        rpp.PantallaPermitida.Categoria ascending,
                        rpp.PantallaPermitida.Nombre ascending
                    select rpp.PantallaPermitida).Distinct().OrderBy(o => o.Categoria).ThenBy(o => o.Nombre).ToList<PantallaPermitida>();
        }

        /// <summary>
        /// Con este metodo obtenemos unicamente los permisos especiales asignados a dicho usuario
        /// </summary>
        /// <param name="miembroId"></param>
        /// <returns></returns>
        public List<PermisoEspecial> ObtenerPermisosEspecialesPorMiembro(int miembroId)
        {
            List<int> rolesPorMiembro = this.ObtenerRolesPorMiembro(miembroId);

            return (from rpe in SesionActual.Instance.getContexto<IglesiaEntities>().RolPermisoEspecial
                    where
                        rolesPorMiembro.Contains(rpe.RolId)
                    orderby
                        rpe.PermisoEspecialId
                    select rpe.PermisoEspecial).ToList<PermisoEspecial>();
        }

        /// <summary>
        /// Con este metodo obtenemos TODOS los permisos especiales, ya sea que el usuario actual tenga asignado o no dichos permisos
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> ObtenerPermisosEspecialesPorSistema()
        {
            return (from pe in SesionActual.Instance.getContexto<IglesiaEntities>().PermisoEspecial
                    orderby
                        pe.Descripcion
                    select pe).ToDictionary(x => x.PermisoEspecialId, x => x.Descripcion);
        }

        #endregion

    }

}
