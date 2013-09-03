using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeRoles
    {
        #region General

        /// <summary>
        /// Su funcion es regresar la lista de todas las pantallas dadas de alta en la BD, marcando si esta o no permitida para dicho rol
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public List<Modelos.Retornos.PantallaPermitida> ObtenerPantallasPermitidasPorRol(int rolId)
        {
            return (
                from pantallas in SesionActual.Instance.getContexto<IglesiaEntities>().PantallaPermitida
                join roles in SesionActual.Instance.getContexto<IglesiaEntities>().RolPantallaPermitida on
                    new { pantallas.PantallaPermitidaId, RolId = rolId }
                    equals
                    new { roles.PantallaPermitidaId, roles.RolId }
                    into joined
                from roles in joined.DefaultIfEmpty()
                orderby pantallas.AppId, pantallas.Categoria, pantallas.Nombre
                select new Modelos.Retornos.PantallaPermitida
                {
                    Id = pantallas.PantallaPermitidaId,
                    Marcado = (roles != null ? true : false),
                    Nombre = pantallas.Nombre,
                    Categoria = pantallas.Categoria,
                    AppId = pantallas.AppId
                }).ToList<Modelos.Retornos.PantallaPermitida>();
        }

        /// <summary>
        /// Su funcion es regresar la lista de todos los permisos especiales dados de alta en la BD, marcando si esta o no permitida para dicho rol
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public List<RegistroBasicoExt> ObtenerPermisosEspecialesPorRol(int rolId)
        {
            return (
                from permisos in SesionActual.Instance.getContexto<IglesiaEntities>().PermisoEspecial
                join roles in SesionActual.Instance.getContexto<IglesiaEntities>().RolPermisoEspecial on
                    new { permisos.PermisoEspecialId, RolId = rolId }
                    equals
                    new { roles.PermisoEspecialId, roles.RolId }
                    into joined
                from roles in joined.DefaultIfEmpty()
                orderby permisos.Descripcion
                select new RegistroBasicoExt
                {
                    Id = permisos.PermisoEspecialId,
                    Marcado = (roles != null ? true : false),
                    Descripcion = permisos.Descripcion
                }).ToList<RegistroBasicoExt>();
        }

        /// <summary>
        /// Su funcion es regresar la lista de todos los roles dados de alta en la BD, marcando si esta o no permitido (asignar a otro usuario...) para dicho rol
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public List<RegistroBasicoExt> ObtenerRolesAsignablesPorRol(int rolId)
        {
            return (
            from roles in SesionActual.Instance.getContexto<IglesiaEntities>().Rol
            join rolesAsignablesPorRol in SesionActual.Instance.getContexto<IglesiaEntities>().RolAsignable on
                new { RolHijoId = roles.RolId, RolPadreId = rolId }
                equals
                new { rolesAsignablesPorRol.RolHijoId, rolesAsignablesPorRol.RolPadreId }
                into joined
            from rolesPermitidos in joined.DefaultIfEmpty()
            orderby roles.Descripcion
            select new RegistroBasicoExt
            {
                Id = roles.RolId,
                Marcado = (rolesPermitidos != null ? true : false),
                Descripcion = roles.Descripcion
            }).ToList<RegistroBasicoExt>();
        }

        public List<int> ObtenerRolesAsignablesPorRoles(List<int> rolesId)
        {
            return (
                from rolesAsignablesPorRol in SesionActual.Instance.getContexto<IglesiaEntities>().RolAsignable
                where rolesId.Contains(rolesAsignablesPorRol.RolPadreId)
                orderby rolesAsignablesPorRol.RolHijoId
                select rolesAsignablesPorRol.RolHijoId
                ).ToList<int>().Distinct().ToList<int>();
        }

        public bool GuardarRol(int rolId, string descripcion, RegistrosHelper.RegistrosDeDatos pantallasNuevasYEliminadas, RegistrosHelper.RegistrosDeDatos permisosEspecialesNuevosYEliminados, RegistrosHelper.RegistrosDeDatos rolesAsignablesNuevosYEliminados)
        {
            bool rtn = false;

            if (descripcion.Trim().Length > 0 && (pantallasNuevasYEliminadas.RegistrosNuevosId.Count > 0 || pantallasNuevasYEliminadas.RegistrosEliminadosId.Count > 0 || permisosEspecialesNuevosYEliminados.RegistrosNuevosId.Count > 0 || permisosEspecialesNuevosYEliminados.RegistrosEliminadosId.Count > 0 || rolesAsignablesNuevosYEliminados.RegistrosNuevosId.Count > 0 || rolesAsignablesNuevosYEliminados.RegistrosEliminadosId.Count > 0))
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                {
                    Rol rol = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Rol where o.RolId == rolId select o).FirstOrDefault();
                    RolPantallaPermitida rpp;
                    RolPermisoEspecial rpe;
                    RolAsignable ra;

                    if (rol == null)
                    {
                        rol = new Rol();
                        rtn = true;
                    }

                    rol.Descripcion = descripcion;

                    //Agregamos las nuevas pantallas permitidas (siempre y cuando no existan previamente...)
                    foreach (int pantallaPermitidaId in pantallasNuevasYEliminadas.RegistrosNuevosId)
                    {
                        rpp = new RolPantallaPermitida();
                        rpp.RolId = rolId;
                        rpp.PantallaPermitidaId = pantallaPermitidaId;
                        rol.RolPantallaPermitida.Add(rpp);
                    }

                    //Agregamos los nuevos permisos especiales (siempre y cuando no existan previamente...)
                    foreach (int permisoEspecialId in permisosEspecialesNuevosYEliminados.RegistrosNuevosId)
                    {
                        rpe = new RolPermisoEspecial();
                        rpe.RolId = rolId;
                        rpe.PermisoEspecialId = permisoEspecialId;
                        rol.RolPermisoEspecial.Add(rpe);
                    }

                    //Agregamos los nuevos roles asignables (siempre y cuando no existan previamente...)
                    foreach (int rolPermitidoId in rolesAsignablesNuevosYEliminados.RegistrosNuevosId)
                    {
                        ra = new RolAsignable();
                        ra.RolPadreId = rolId;
                        ra.RolHijoId = rolPermitidoId;
                        rol.RolAsignableHijos.Add(ra);
                    }

                    //Guardamos los cambios, antes de eliminar registros
                    rol.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                    //Eliminamos las pantallas permitidas
                    foreach (int pantallaPermitidaId in pantallasNuevasYEliminadas.RegistrosEliminadosId)
                    {
                        rpp = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().RolPantallaPermitida where o.PantallaPermitidaId == pantallaPermitidaId && o.RolId == rolId select o).SingleOrDefault();
                        if (rpp != null)
                        {
                            rpp.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                        }
                    }

                    //Eliminamos las permisos especiales
                    foreach (int permisoEspecialId in permisosEspecialesNuevosYEliminados.RegistrosEliminadosId)
                    {
                        rpe = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().RolPermisoEspecial where o.PermisoEspecialId == permisoEspecialId && o.RolId == rolId select o).SingleOrDefault();
                        if (rpe != null)
                        {
                            rpe.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                        }
                    }

                    //Eliminamos los roles asignables
                    foreach (int rolAsignableId in rolesAsignablesNuevosYEliminados.RegistrosEliminadosId)
                    {
                        ra = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().RolAsignable where o.RolHijoId == rolAsignableId && o.RolPadreId == rolId select o).SingleOrDefault();
                        if (ra != null)
                        {
                            ra.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                        }
                    }

                    //Marcamos como finalizadas todas las operaciones, para que la transaccion se lleve a cabo en la BD
                    transactionScope.Complete();
                }
            }

            return rtn;
        }

        #endregion

        #region Roles por Miembro

        public List<int> ObtenerRolesPorMiembroActual()
        {
            return (
                from rolesPermitidos in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroRol 
                where rolesPermitidos.MiembroId == SesionActual.Instance.UsuarioId
                orderby rolesPermitidos.RolId
                select rolesPermitidos.RolId).ToList<int>();
        }

        /// <summary>
        /// Su funcion es regresar la lista de todos los roles dados de alta en la BD, marcando si esta o no permitido para dicho usuario (dependiendo de los roles asignables del usuario actual)
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>
        public List<RegistroBasicoExt> ObtenerRolesPorMiembro(int miembroId)
        {
            List<int> rolesAsignablesParaMiembroActual = ObtenerRolesAsignablesPorRoles(ObtenerRolesPorMiembroActual());

            return (
                from roles in SesionActual.Instance.getContexto<IglesiaEntities>().Rol
                join rolesPermitidos in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroRol on
                    new { roles.RolId, MiembroId = miembroId }
                    equals
                    new { rolesPermitidos.RolId, rolesPermitidos.MiembroId }
                    into joined
                from rolesPermitidos in joined.DefaultIfEmpty()
                where rolesAsignablesParaMiembroActual.Contains(roles.RolId)
                orderby roles.Descripcion
                select new RegistroBasicoExt
                {
                    Id = roles.RolId,
                    Marcado = (rolesPermitidos != null ? true : false),
                    Descripcion = roles.Descripcion
                }).ToList<RegistroBasicoExt>();
        }

        public bool GuardarRolesPorMiembro(int miembroId, RegistrosHelper.RegistrosDeDatos rolesNuevosYEliminados)
        {
            bool rtn = false;

            if (miembroId > 0 && (rolesNuevosYEliminados.RegistrosNuevosId.Count > 0 || rolesNuevosYEliminados.RegistrosEliminadosId.Count > 0))
            {
                Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroId select o).FirstOrDefault();
                MiembroRol miembroRol;

                if (miembro == null) { throw new ExcepcionReglaNegocio(Literales.RegistroInexistente); }

                //Agregamos los nuevos roles (siempre y cuando no existan previamente...)
                foreach (int rolId in rolesNuevosYEliminados.RegistrosNuevosId)
                {
                    miembroRol = new MiembroRol();
                    miembroRol.MiembroId = miembroId;
                    miembroRol.RolId = rolId;
                    miembro.MiembroRol.Add(miembroRol);
                }

                //Guardamos los cambios, antes de eliminar registros
                miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                //Eliminamos los roles
                foreach (int rolId in rolesNuevosYEliminados.RegistrosEliminadosId)
                {
                    miembroRol = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().MiembroRol where o.RolId == rolId && o.MiembroId == miembroId select o).SingleOrDefault();
                    if (miembroRol != null)
                    {
                        miembroRol.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    }
                }
            }

            return rtn;
        }

        #endregion
    }
}
