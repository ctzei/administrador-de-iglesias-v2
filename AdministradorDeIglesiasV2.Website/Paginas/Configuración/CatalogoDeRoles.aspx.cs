using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;

namespace AdministradorDeIglesiasV2.Website.Paginas
{
    public partial class CatalogoDeRoles : PaginaBase, ICatalogo
    {
        ManejadorDeRoles manejadorRoles;

        void ICatalogo.CargarControles()
        {
            manejadorRoles = new ManejadorDeRoles();
            StorePantallasPermitidas.Cargar(manejadorRoles.ObtenerPantallasPermitidasPorRol(-1));
            StorePermisosEspeciales.Cargar(manejadorRoles.ObtenerPermisosEspecialesPorRol(-1));
            StoreRolesAsignables.Cargar(manejadorRoles.ObtenerRolesAsignablesPorRol(-1));
        }

        void ICatalogo.Buscar()
        {
            StoreResultados.Cargar((
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().Rol
                where
                    (o.RolId == (filtroId.Number > 0 ? filtroId.Number : o.RolId)) &&
                    (o.Descripcion.Contains(filtroDescripcion.Text))
                orderby o.Descripcion
                select new {
                    Id = o.RolId, 
                    Descripcion = o.Descripcion,
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            Rol entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Rol where o.RolId == id select o).FirstOrDefault();
            registroId.Text = entidad.RolId.ToString();
            registroDescripcion.Text = entidad.Descripcion;

            manejadorRoles = new ManejadorDeRoles();
            StorePantallasPermitidas.Cargar(manejadorRoles.ObtenerPantallasPermitidasPorRol(id));
            StorePermisosEspeciales.Cargar(manejadorRoles.ObtenerPermisosEspecialesPorRol(id));
            StoreRolesAsignables.Cargar(manejadorRoles.ObtenerRolesAsignablesPorRol(id));
        }

        void ICatalogo.Borrar(int id)
        {
            Rol entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Rol where o.RolId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            manejadorRoles = new ManejadorDeRoles();
            manejadorRoles.GuardarRol(id, registroDescripcion.Text, listaDeRegistrosDeDatos.Obtener(registroPantallasPermitidas.ClientID), listaDeRegistrosDeDatos.Obtener(registroPermisosEspeciales.ClientID), listaDeRegistrosDeDatos.Obtener(registroRolesAsignables.ClientID));
        }
    }
}