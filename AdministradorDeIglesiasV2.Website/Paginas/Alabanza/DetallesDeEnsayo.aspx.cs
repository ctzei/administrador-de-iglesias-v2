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
    public partial class DetallesDeEnsayo : PaginaDeDetalle
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
            int ensayoId = this.ObtenerId();
            cargarDatosGenerales(ensayoId);
            cargarMiembros(ensayoId);
            cargarCanciones(ensayoId);
        }

        private void cargarDatosGenerales(int ensayoId)
        {
            AlabanzaEnsayo ensayo = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayo where o.Id == ensayoId select o).SingleOrDefault();

            registroId.Text = ensayo.Id.ToString();
            registroFecha.Value = ensayo.Fecha.ToFullDateString();
            registroHoraInicio.Value = ensayo.HoraDiaInicio.Descripcion;
            registroHoraFin.Value = ensayo.HoraDiaFin.Descripcion;
        }

        private void cargarMiembros(int ensayoId)
        {
            StoreMiembros.Cargar(manejadorDeAlabaza.ObtenerMiembrosPorEnsayo(ensayoId).Select(o =>
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

        private void cargarCanciones(int ensayoId)
        {
            StoreCanciones.Cargar(manejadorDeAlabaza.ObtenerCancionesPorEnsayo(ensayoId).Select(o =>
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

    }
}