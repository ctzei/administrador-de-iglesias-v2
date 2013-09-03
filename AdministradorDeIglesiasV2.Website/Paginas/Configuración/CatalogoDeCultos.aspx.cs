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
    public partial class CatalogoDeCultos : PaginaBase, ICatalogo
    {
        void ICatalogo.CargarControles()
        {
            StoreDiasDeLaSemana.Cargar(DiaSemana.Obtener());
            StoreHorasDelDia.Cargar(HoraDia.Obtener());
        }

        void ICatalogo.Buscar()
        {
            List<int> idsDiaDeLaSemana = filtroDiaDeLaSemana.ObtenerIds();
            List<int> idsHoraDelDia = filtroHoraDelDia.ObtenerIds();

            StoreResultados.Cargar((
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().Culto
                where
                    (o.CultoId == (filtroId.Number > 0 ? filtroId.Number : o.CultoId)) &&
                    (idsDiaDeLaSemana.Contains(o.DiaSemanaId) || (idsDiaDeLaSemana.Count == 0)) &&
                    (idsHoraDelDia.Contains(o.HoraDiaId) || (idsHoraDelDia.Count == 0)) &&
                    (o.Descripcion.Contains(filtroDescripcion.Text)) &&
                    (o.Borrado == false) //Registros NO borrados
                orderby o.Descripcion
                select new {
                    Id = o.CultoId, 
                    Descripcion = o.Descripcion,
                    DiaSemanaDesc = o.DiaSemana.Descripcion,
                    HoraDiaDesc = o.HoraDia.Descripcion
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            Culto entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Culto where o.CultoId == id select o).FirstOrDefault();
            registroId.Text = entidad.CultoId.ToString();
            registroDiaDeLaSemana.Value = entidad.DiaSemanaId;
            registroHoraDelDia.Value = entidad.HoraDiaId;
            registroDescripcion.Text = entidad.Descripcion;
        }

        void ICatalogo.Borrar(int id)
        {
            Culto entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Culto where o.CultoId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            Culto entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Culto where o.CultoId == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new Culto();
            }

            entidad.DiaSemanaId = registroDiaDeLaSemana.ObtenerId();
            entidad.HoraDiaId = registroHoraDelDia.ObtenerId();
            entidad.Descripcion = registroDescripcion.Text;
            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }
    }
}