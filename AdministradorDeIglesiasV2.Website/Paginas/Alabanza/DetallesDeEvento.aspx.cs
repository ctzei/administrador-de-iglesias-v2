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
    public partial class DetallesDeEvento : PaginaDeDetalle
    {
        ManejadorDeAlabanza manejadorDeAlabaza = new ManejadorDeAlabanza();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }
        }

        public void CargarControles()
        {
            int eventoId = this.ObtenerId();
            cargarDatosGenerales(eventoId);
            cargarMiembros(eventoId);
            cargarCanciones(eventoId);
            cargarEnsayos(eventoId);
        }

        private void cargarDatosGenerales(int eventoId)
        {
            AlabanzaEvento evento = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEvento where o.Id == eventoId select o).SingleOrDefault();

            registroId.Text = evento.Id.ToString();
            registroDescripcion.Text = evento.Descripcion;
            registroFecha.Value = evento.Fecha.ToFullDateString();
            registroHoraInicio.Value = evento.HoraDiaInicio.Descripcion;
            registroHoraFin.Value = evento.HoraDiaFin.Descripcion;
        }

        private void cargarMiembros(int eventoId)
        {
            StoreMiembros.Cargar(manejadorDeAlabaza.ObtenerMiembrosPorEvento(eventoId).Select(o =>
                new
                {
                    Id = o.Id,
                    MiembroId = o.AlabanzaMiembro.MiembroId,
                    Nombre = o.AlabanzaMiembro.Miembro.Primer_Nombre + " " + o.AlabanzaMiembro.Miembro.Segundo_Nombre + " " + o.AlabanzaMiembro.Miembro.Apellido_Paterno + " " + o.AlabanzaMiembro.Miembro.Apellido_Materno,
                    Email = o.AlabanzaMiembro.Miembro.Email,
                    Instrumento = "",
                    Asistencia = o.Asistencia,
                    Retraso = o.Retraso
                }));
        }

        private void cargarCanciones(int eventoId)
        {
            StoreCanciones.Cargar(manejadorDeAlabaza.ObtenerCancionesPorEvento(eventoId).Select(o =>
                new
                {
                    Id = o.Id,
                    CancionId = o.AlabanzaCancionId,
                    Titulo = o.AlabanzaCancion.Titulo,
                    Artista = o.AlabanzaCancion.Artista,
                    Disco = o.AlabanzaCancion.Disco,
                    Tono = o.AlabanzaCancion.Tono,
                    Liga = o.AlabanzaCancion.Liga
                }));
        }

        private void cargarEnsayos(int eventoId)
        {
            StoreEnsayos.Cargar(manejadorDeAlabaza.ObtenerEnsayosPorEvento(eventoId).Select(o =>
                 new
                 {
                     Id = o.Id,
                     EventoId = o.AlabanzaEventoId,
                     Fecha = o.Fecha,
                     HoraInicio = o.HoraDiaInicio.Descripcion,
                     HoraFin = o.HoraDiaFin.Descripcion
                 }));
        }
    }
}