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
    public partial class DetallesDeMiembro : PaginaDeDetalle
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }
        }

        public void CargarControles()
        {
            int miembroId = ObtenerId();
            Miembro miembro = miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroId select o).SingleOrDefault();
            if (miembro != null)
            {
                cargarDatosGenerales(miembro);
                cargarLideres(miembro);
                cargarAsistencias(miembro);
            }
        }

        private void cargarDatosGenerales(Miembro miembro)
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();

            registroId.Text = miembro.MiembroId.ToString();
            registroNombre.Text = miembro.Primer_Nombre + " " + miembro.Segundo_Nombre + " " + miembro.Apellido_Paterno + " " + miembro.Apellido_Materno;
            registroRed.Text = manejadorCelulas.ObtenerRedSuperior(miembro.Celula, " > ");
            registroEmail.Text = miembro.Email;
            registroMunicipio.Text = miembro.UbicacionMunicipio.Descripcion;
            registroColonia.Text = miembro.Colonia;
            registroDireccion.Text = miembro.Direccion;
            registroEstadoCivil.Text = miembro.EstadoCivil.Descripcion;
            registroFechaDeNacimiento.Text = string.Format("{0} ({1} años)", miembro.Fecha_Nacimiento.GetValueOrDefault(DateTime.Today).ToShortDateString(), ((DateTime.Today - miembro.Fecha_Nacimiento.GetValueOrDefault(DateTime.Today)).Days / 365).ToString());
            registroTelefonos.Text = string.Format("{0} | {1} | {2}", miembro.Tel_Casa, miembro.Tel_Movil, miembro.Tel_Trabajo);

            X.Call("cargarMapaDesdeDireccionEnPanel", gridDireccion.ClientID, miembro.UbicacionMunicipio.UbicacionEstado.UbicacionPais.Descripcion, miembro.UbicacionMunicipio.UbicacionEstado.Descripcion, miembro.UbicacionMunicipio.Descripcion, miembro.Colonia, miembro.Direccion);
            X.Call("cargarFoto", registroFoto.ClientID, miembro.MiembroId);
        }

        private void cargarLideres(Miembro miembro)
        {
            ManejadorDeLideresDeCelula manejadorDeLideresDeCelula = new ManejadorDeLideresDeCelula();
            StoreLideresDirectos.Cargar(manejadorDeLideresDeCelula.ObtenerLideresDirectosPorMiembro(miembro));
            StoreLiderzagoDirecto.Cargar(manejadorDeLideresDeCelula.ObtenerLiderazgoDirectoPorMiembro(miembro));
            StoreLiderzagoIndirecto.Cargar(manejadorDeLideresDeCelula.ObtenerLiderazgoIndirectoPorMiembro(miembro));
        }

        private void cargarAsistencias(Miembro miembro)
        {
            ManejadorDeReportesDeAsistencias manejadorDeReportesDeAsistencia = new ManejadorDeReportesDeAsistencias();
            ManejadorDeAsistenciasDeCelula manejadorDeAsistenciasDeCelula = new ManejadorDeAsistenciasDeCelula();

            registroUltimaAsistencia.Text = manejadorDeAsistenciasDeCelula.ObtenerUltimaAsistenciaPorMiembro(miembro.MiembroId).ToFullDateString();
            X.Call("crearGraficasDeAsistencias", manejadorDeReportesDeAsistencia.ObtenerReporteDeAsistenciasPorMiembro(miembro.CelulaId, miembro.MiembroId, DateTime.Now, true).ToJson(), miembro.Email);
        }

    }
}