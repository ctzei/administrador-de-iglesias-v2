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
    public partial class RegistroDeAsistencia : PaginaBase
    {
        ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
        ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }                                 
        }

        public void CargarControles()
        {
            StoreCelulas.Cargar(manejadorDeCelulas.ObtenerCelulasPermitidasPorMiembroComoCelulas(SesionActual.Instance.UsuarioId));

            // Precargamos la celula principal
            Celula celulaPrincipal = manejadorDeCelulas.ObtenerCelulaQueMiembroEsLider(SesionActual.Instance.UsuarioId);
            cboCelula.Value = celulaPrincipal.CelulaId;
            DateTime fechaPreseleccionada=  manejadorDeAsistencias.ObtenerFechaDeSiguienteAsistencia(celulaPrincipal.CelulaId);
            dtpFecha.Value = fechaPreseleccionada;
            mostrarAsistencias(celulaPrincipal.CelulaId, fechaPreseleccionada);
        }

        [DirectMethod(ShowMask = true)]
        public void MostrarAsistenciaClick()
        {
            int celulaSeleccionada;
            if ((int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada)) && (dtpFecha.SelectedDate.Year > 1900))
            {
                mostrarAsistencias(celulaSeleccionada, dtpFecha.SelectedDate);
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void MostrarUltimaAsistenciaClick()
        {
            try
            {
                int celulaSeleccionada;
                if (int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada))
                {
                    mostrarAsistencias(celulaSeleccionada, null);
                }
                else
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaNecesaria).Show();
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void GuardarAsistenciaClick(string jsonRegistrosModificados)
        {
            try
            {
                int celulaSeleccionada;
                if ((int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada)) && (dtpFecha.SelectedDate.Year > 1900))
                {
                    Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistencias = new Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada(JSON.Deserialize<List<Dictionary<string, string>>>(jsonRegistrosModificados));
                    manejadorDeAsistencias.GuardarAsistencia(celulaSeleccionada, dtpFecha.SelectedDate, asistencias, Convert.ToInt32(registroNumeroDeInvitados.Number), SesionActual.Instance.UsuarioId);
                    cargarAsistenciasEnGrid(celulaSeleccionada, dtpFecha.SelectedDate);
                    X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.AsistenciaGuardadaCorrectamente).Show();
                }
                else
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void CancelarAsistenciaClick()
        {
            try
            {
                int celulaSeleccionada;
                if ((int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada)) && (dtpFecha.SelectedDate.Year > 1900))
                {
                    manejadorDeAsistencias.CancelarAsistencia(celulaSeleccionada, dtpFecha.SelectedDate, txtRazonCancelacion.Text, SesionActual.Instance.UsuarioId);
                    cargarAsistenciasEnGrid(celulaSeleccionada, dtpFecha.SelectedDate);
                    X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.CancelacionDeAsistenciaGuardadaCorrectamente).Show();
                }
                else
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        private void mostrarAsistencias(int celulaId, DateTime? fecha)
        {
            if (fecha == null)
            {
                fecha = manejadorDeAsistencias.ObtenerFechaDeUltimaAsistencia(celulaId);
                dtpFecha.SelectedDate = fecha.Value; // Establecemos el valor para que el usuario "vea" que fecha fue
            }

            cargarAsistenciasEnGrid(celulaId, fecha.Value);

            if (manejadorDeAsistencias.CelulaFueCancelada(celulaId, fecha.Value))
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, string.Format(Resources.Literales.AsistenciaACelulaPreviamenteCancelada, manejadorDeAsistencias.ObtenerRazonDeLaCancelacion(celulaId, fecha.Value))).Show();
            }
        }

        private void cargarAsistenciasEnGrid(int celulaId, DateTime fecha)
        {
            Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistencias = manejadorDeAsistencias.ObtenerAsistencia(celulaId, fecha);
            StoreAsistencias.Cargar((from o in asistencias.Asistencias select new {
                o.Id,
                o.MiembroId,
                Nombre = o.PrimerNombre + " " + o.SegundoNombre + " " + o.ApellidoPaterno + " " + o.ApellidoMaterno,
                o.Asistencia,
                o.Estatus,
                o.Peticiones
            }));
            registroNumeroDeInvitados.Value = manejadorDeAsistencias.ObtenerNumeroDeInvitados(celulaId, fecha);
            registroNumeroDeAsistencias.Text = string.Format("{0} Asistencias", asistencias.CantidadDeAsistencias);
            registroNumeroDeMiembros.Text = string.Format("- {0} Miembros", asistencias.CantidadDeRegistros);
        }
    }
}