using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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

namespace AdministradorDeIglesiasV2.Website.Paginas.Foli
{
    public partial class CatalogoDeGrupos : PaginaBase, ICatalogo
    {
        ManejadorDeFoli manejadorFoli;

        void ICatalogo.CargarControles()
        {
            StoreDiasDeLaSemana.Cargar(DiaSemana.Obtener());
            StoreHorasDelDia.Cargar(HoraDia.Obtener());
            StoreCiclos.Cargar(Ciclo.Obtener());
            BuscadorAlumnos.LimpiarControles();
            BuscadorMaestros.LimpiarControles();
        }

        void ICatalogo.Buscar()
        {
            List<int> idsDiasDeLaSemana = filtroDiaDeLaSemana.ObtenerIds();
            List<int> idsHorasDelDia = filtroHoraDelDia.ObtenerIds();
            List<int> idsCiclos = filtroCiclo.ObtenerIds();

            StoreResultados.Cargar((
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliGrupo
                where
                    (o.Id == (filtroId.Number > 0 ? filtroId.Number : o.Id)) &&
                    (o.Descripcion.Contains(filtroDescripcion.Text)) &&
                    (o.Fecha_Inicio_Modulo1 == (filtroFechaInicioModulo1.SelectedDate.Year > 1900 ? filtroFechaInicioModulo1.SelectedDate : o.Fecha_Inicio_Modulo1)) &&
                    (o.Fecha_Inicio_Modulo2 == (filtroFechaInicioModulo2.SelectedDate.Year > 1900 ? filtroFechaInicioModulo2.SelectedDate : o.Fecha_Inicio_Modulo2)) &&
                    (o.Fecha_Inicio_Modulo3 == (filtroFechaInicioModulo3.SelectedDate.Year > 1900 ? filtroFechaInicioModulo3.SelectedDate : o.Fecha_Inicio_Modulo3)) &&
                    (o.Fecha_Inicio_Modulo4 == (filtroFechaInicioModulo4.SelectedDate.Year > 1900 ? filtroFechaInicioModulo4.SelectedDate : o.Fecha_Inicio_Modulo4)) &&
                    (o.Fecha_Fin == (filtroFechaFin.SelectedDate.Year > 1900 ? filtroFechaFin.SelectedDate : o.Fecha_Fin)) &&
                    (idsDiasDeLaSemana.Contains(o.DiaSemanaId) || (idsDiasDeLaSemana.Count == 0)) &&
                    (idsHorasDelDia.Contains(o.HoraDiaId) || (idsHorasDelDia.Count == 0)) &&
                    (idsCiclos.Contains(o.CicloId) || (idsCiclos.Count == 0)) &&
                    (o.Borrado == false) //Registros NO borrados
                select new {
                    Id = o.Id, 
                    Descripcion = o.Descripcion,
                    FechaInicioMod1 = o.Fecha_Inicio_Modulo1,
                    FechaInicioMod2 = o.Fecha_Inicio_Modulo2,
                    FechaInicioMod3 = o.Fecha_Inicio_Modulo3,
                    FechaInicioMod4 = o.Fecha_Inicio_Modulo4,
                    FechaFin = o.Fecha_Fin,
                    DiaSemana = o.DiaSemana.Descripcion,
                    HoraDia = o.HoraDia.Descripcion,
                    Ciclo = o.Ciclo.Descripcion
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            FoliGrupo entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliGrupo where o.Id == id select o).FirstOrDefault();
            registroId.Text = entidad.CicloId.ToString();
            registroDescripcion.Text = entidad.Descripcion;
            registroFechaInicioModulo1.Value = entidad.Fecha_Inicio_Modulo1;
            registroFechaInicioModulo2.Value = entidad.Fecha_Inicio_Modulo2;
            registroFechaInicioModulo3.Value = entidad.Fecha_Inicio_Modulo3;
            registroFechaInicioModulo4.Value = entidad.Fecha_Inicio_Modulo4;
            registroFechaFin.Value = entidad.Fecha_Fin;
            registroDiaDeLaSemana.Value = entidad.DiaSemanaId;
            registroHoraDelDia.Value = entidad.HoraDiaId;
            registroCiclo.Value = entidad.CicloId;

            manejadorFoli = new ManejadorDeFoli();
            BuscadorMaestros.CargarListado(manejadorFoli.ObtenerMaestrosPorGrupo(id));
            BuscadorAlumnos.CargarListado(manejadorFoli.ObtenerAlumnosPorGrupo(id));
        }

        void ICatalogo.Borrar(int id)
        {
            Ciclo entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Ciclo where o.CicloId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            FoliGrupo entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliGrupo where o.Id == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new FoliGrupo();
            }

            entidad.Descripcion = registroDescripcion.Text;
            entidad.Fecha_Inicio_Modulo1 = registroFechaInicioModulo1.SelectedDate;
            entidad.Fecha_Inicio_Modulo2 = registroFechaInicioModulo2.SelectedDate;
            entidad.Fecha_Inicio_Modulo3 = registroFechaInicioModulo3.SelectedDate;
            entidad.Fecha_Inicio_Modulo4 = registroFechaInicioModulo4.SelectedDate;
            entidad.Fecha_Fin = registroFechaFin.SelectedDate;
            entidad.DiaSemanaId = registroDiaDeLaSemana.ObtenerId();
            entidad.HoraDiaId = registroHoraDelDia.ObtenerId();
            entidad.CicloId = registroCiclo.ObtenerId();
            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

            manejadorFoli = new ManejadorDeFoli();
            manejadorFoli.GuardarMaestrosYAlumnosEnGrupo(entidad.Id, listaDeRegistrosDeDatos.Obtener(BuscadorMaestros.GridDeListadoDeObjetos.ClientID), listaDeRegistrosDeDatos.Obtener(BuscadorAlumnos.GridDeListadoDeObjetos.ClientID));
        }
    }
}