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


namespace AdministradorDeIglesiasV2.Website.Paginas.Alabanza
{
    public partial class Calendario : PaginaBase
    {

        ManejadorDeAlabanza manejadorDeAlabanza = new ManejadorDeAlabanza();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }
        }

        public void CargarControles()
        {
            int usuarioId = SesionActual.Instance.UsuarioId;
            StoreEnsayos.Cargar(manejadorDeAlabanza.ObtenerEnsayosPorMiembro(usuarioId).Select(o =>
                                new
                                {
                                    Id = o.Id,
                                    Fecha = o.Fecha,
                                    HoraInicio = o.HoraDiaInicio.Descripcion,
                                    HoraFin = o.HoraDiaFin.Descripcion
                                }));


            StoreEventos.Cargar(manejadorDeAlabanza.ObtenerEventosPorMiembro(usuarioId).Select(o =>
                                new
                                {
                                    Id = o.Id,
                                    Descripcion = o.Descripcion,
                                    Fecha = o.Fecha,
                                    HoraInicio = o.HoraDiaInicio.Descripcion,
                                    HoraFin = o.HoraDiaFin.Descripcion
                                }));

            StoreDiasNoDisponibles.Cargar(manejadorDeAlabanza.ObtenerDiasNoDisponiblesPorMiembro(usuarioId).Select(o =>
                               new
                               {
                                   Id = o.Id,
                                   Razon = o.Razon,
                                   FechaInicio = o.FechaInicio,
                                   FechaFin = o.FechaFin,
                                   Dias = (o.FechaFin - o.FechaInicio).Days + 1
                               }));
        }

        [DirectMethod(ShowMask = true)]
        public object ObtenerEventos()
        {
            try
            {
                int usuarioId = SesionActual.Instance.UsuarioId;
                var eventos = new
                {
                    eventos = (manejadorDeAlabanza.ObtenerEventosPorMiembro(usuarioId).Select(o =>
                               new
                               {
                                   id = o.Id,
                                   title = o.Descripcion,
                                   date = o.Fecha,
                                   startTime = o.HoraDiaInicio.Descripcion,
                                   endTime = o.HoraDiaFin.Descripcion,
                                   allDay = false,
                               })),

                    ensayos = (manejadorDeAlabanza.ObtenerEnsayosPorMiembro(usuarioId).Select(o =>
                               new
                               {
                                   id = o.Id,
                                   title = "Ensayo: " + o.AlabanzaEvento.Descripcion,
                                   date = o.Fecha,
                                   startTime = o.HoraDiaInicio.Descripcion,
                                   endTime = o.HoraDiaFin.Descripcion,
                                   allDay = false
                               })),

                    diasNoDisponibles = (manejadorDeAlabanza.ObtenerDiasNoDisponiblesPorMiembro(usuarioId).Select(o =>
                               new
                               {
                                   id = o.Id,
                                   title = o.Razon,
                                   start = o.FechaInicio,
                                   end = o.FechaFin,
                                   allDay = true
                               }))
                };

                return eventos;
            }
            catch (ExcepcionReglaNegocio ex)
            {
                return new { error = ex.Message };
            }
        }

        [DirectMethod(ShowMask = true)]
        public void AgregarDiasNoDisponibles()
        {
            try
            {
                AlabanzaMiembroDiaNoDisponible entidad = new AlabanzaMiembroDiaNoDisponible();
                entidad.AlabanzaMiembroId = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro where o.MiembroId == SesionActual.Instance.UsuarioId select o.Id).SingleOrDefault();
                entidad.FechaInicio = registroDiasNoDisponiblesFechaInicio.SelectedDate;
                entidad.FechaFin = registroDiasNoDisponiblesFechaFin.SelectedDate;
                entidad.Razon = registroDiasNoDisponiblesRazon.Text;
                entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                wndAgregarDiasNoDisponibles.Hide();
            }
            finally
            {
                CargarControles();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void BorrarDiaNoDisponible(int id)
        {
            try
            {
                AlabanzaMiembroDiaNoDisponible entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembroDiaNoDisponible where o.Id == id select o).SingleOrDefault();
                if (entidad != null)
                {
                    entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }
            }
            finally
            {
                CargarControles();
            }
        }

    }

}