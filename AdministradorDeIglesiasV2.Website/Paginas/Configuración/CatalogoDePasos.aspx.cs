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
    public partial class CatalogoDePasos : PaginaBase, ICatalogo
    {
        void ICatalogo.CargarControles()
        {
            StoreCategorias.Cargar((from o in SesionActual.Instance.getContexto<IglesiaEntities>().PasoCategoria where o.Borrado == false select o));
        }

        void ICatalogo.Buscar()
        {
            List<int> idsCategorias = filtroCategoria.ObtenerIds();

            StoreResultados.Cargar((
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().Paso
                where
                    (o.PasoId == (filtroId.Number > 0 ? filtroId.Number : o.PasoId)) &&
                    (idsCategorias.Contains(o.PasoCategoriaId) || (idsCategorias.Count == 0)) &&
                    (o.Descripcion.Contains(filtroDescripcion.Text)) &&
                    (o.Borrado == false) //Registros NO borrados
                select new {
                    Id = o.PasoId, 
                    Descripcion = o.Descripcion,
                    Categoria = o.PasoCategoria.Descripcion
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            Paso entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Paso where o.PasoId == id select o).FirstOrDefault();
            registroId.Value = entidad.PasoId;
            registroDescripcion.Value = entidad.Descripcion;
            registroCategoria.Value = entidad.PasoCategoriaId;
        }

        void ICatalogo.Borrar(int id)
        {
            Paso entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Paso where o.PasoId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            Paso entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Paso where o.PasoId == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new Paso();
            }

            entidad.Descripcion = registroDescripcion.Text;
            entidad.PasoCategoriaId = registroCategoria.ObtenerId();
            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }
    }
}