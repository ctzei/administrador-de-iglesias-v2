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


namespace AdministradorDeIglesiasV2.Website.Paginas.Foli
{
    public partial class AvanceDeMiembros : PaginaBase
    {
        ManejadorDeFoli manejadorDeFoli;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }                                 
        }

        public void CargarControles()
        {
            manejadorDeFoli = new ManejadorDeFoli();
            StoreGrupos.Cargar(manejadorDeFoli.ObtenerGruposPorMiembroActual());
        }

        [DirectMethod(ShowMask = true)]
        public void MostrarAvanceClick()
        {
            int grupoSeleccionado;
            if ((int.TryParse(cboGrupo.SelectedItem.Value, out grupoSeleccionado)) && (dtpFecha.SelectedDate.Year > 1900))
            {
                manejadorDeFoli = new ManejadorDeFoli();
                cargarAvances(grupoSeleccionado, dtpFecha.SelectedDate);
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void GuardarAvanceClick(string jsonRegistrosModificados)
        {
            try
            {
                int grupoSeleccionado;
                if ((int.TryParse(cboGrupo.SelectedItem.Value, out grupoSeleccionado)) && (dtpFecha.SelectedDate.Year > 1900))
                {
                    List<Dictionary<string, string>> asistencias = JSON.Deserialize<List<Dictionary<string, string>>>(jsonRegistrosModificados);
                    manejadorDeFoli = new ManejadorDeFoli();
                    manejadorDeFoli.GuardarAvances(grupoSeleccionado, dtpFecha.SelectedDate, asistencias, SesionActual.Instance.UsuarioId);
                    cargarAvances(grupoSeleccionado, dtpFecha.SelectedDate);
                    X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.AsistenciaGuardadaCorrectamente).Show();
                }
                else
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.GrupoYFechaNecesarias).Show();
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        private void cargarAvances(int grupoId, DateTime fecha)
        {
            if (manejadorDeFoli == null) { manejadorDeFoli = new ManejadorDeFoli(); }
            Core.Modelos.Retornos.AvanceDeFoliSumarizado avances = manejadorDeFoli.ObtenerAvances(grupoId, fecha);
            StoreAvances.Cargar(avances.Avances);
            registroNumeroDeAsistencias.Text = string.Format("{0} Asistencias", avances.CantidadDeAsistencias);
            registroNumeroDeAlumnos.Text = string.Format("- {0} Miembros", avances.CantidadDeRegistros);
        }
    }
}