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
    public partial class CatalogoDeCiclos : PaginaBase, ICatalogo
    {
        void ICatalogo.CargarControles()
        {
        }

        void ICatalogo.Buscar()
        {
            StoreResultados.Cargar((
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().Ciclo
                where
                    (o.CicloId == (filtroId.Number > 0 ? filtroId.Number : o.CicloId)) &&
                    (o.Descripcion.Contains(filtroDescripcion.Text)) &&
                    (o.Fecha_Inicio == (filtroFechaInicio.SelectedDate.Year > 1900 ? filtroFechaInicio.SelectedDate : o.Fecha_Inicio)) &&
                    (o.Fecha_Max == (filtroFechaMax.SelectedDate.Year > 1900 ? filtroFechaMax.SelectedDate : o.Fecha_Max)) &&
                    (o.Fecha_Fin == (filtroFechaFin.SelectedDate.Year > 1900 ? filtroFechaFin.SelectedDate : o.Fecha_Fin)) &&
                    (o.Borrado == false) //Registros NO borrados
                select new {
                    Id = o.CicloId, 
                    Descripcion = o.Descripcion,
                    FechaInicio = o.Fecha_Inicio,
                    FechaMax = o.Fecha_Max,
                    FechaFin = o.Fecha_Fin
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            Ciclo entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Ciclo where o.CicloId == id select o).FirstOrDefault();
            registroId.Text = entidad.CicloId.ToString();
            registroDescripcion.Text = entidad.Descripcion;
            registroFechaInicio.Value = entidad.Fecha_Inicio;
            registroFechaMax.Value = entidad.Fecha_Max;
            registroFechaFin.Value = entidad.Fecha_Fin;
        }

        void ICatalogo.Borrar(int id)
        {
            Ciclo entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Ciclo where o.CicloId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            Ciclo entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Ciclo where o.CicloId == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new Ciclo();
            }

            entidad.Descripcion = registroDescripcion.Text;
            entidad.Fecha_Inicio = registroFechaInicio.SelectedDate;
            entidad.Fecha_Max = registroFechaMax.SelectedDate;
            entidad.Fecha_Fin = registroFechaFin.SelectedDate;
            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }
    }
}