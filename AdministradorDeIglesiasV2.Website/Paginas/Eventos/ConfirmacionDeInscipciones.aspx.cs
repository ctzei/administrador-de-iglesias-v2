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
    public partial class ConfirmacionDeInscripciones : PaginaBase
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
            ManejadorDeEventos manejador = new ManejadorDeEventos();
            StoreEventos.Cargar(manejador.ObtenerEventosActivos());
        }

        [DirectMethod(ShowMask = true)]
        public void MostrarFichasClick()
        {
            int eventoSeleccionado;
            if ((int.TryParse(cboEvento.SelectedItem.Value, out eventoSeleccionado)) && (dtpFechaInicial.SelectedDate.Year > 1900) && (dtpFechaFinal.SelectedDate.Year > 1900))
            {
                ManejadorDeEventos manejador = new ManejadorDeEventos();
                StoreFichas.Cargar(manejador.ObtenerFichasDeInscripcion(eventoSeleccionado, chkFichasCerradas.Checked, dtpFechaInicial.SelectedDate, dtpFechaFinal.SelectedDate));
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.FiltrosConDatosInvalidos).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void GuardarCambiosClick(string jsonRegistrosModificados)
        {
            try
            {
                int eventoSeleccionado;
                if ((int.TryParse(cboEvento.SelectedItem.Value, out eventoSeleccionado)) && (dtpFechaInicial.SelectedDate.Year > 1900) && (dtpFechaFinal.SelectedDate.Year > 1900))
                {
                    List<Dictionary<string, string>> fichas = JSON.Deserialize<List<Dictionary<string, string>>>(jsonRegistrosModificados);
                    ManejadorDeEventos manejador = new ManejadorDeEventos();
                    manejador.GuardarFichasDeInscripcion(fichas);
                    StoreFichas.Cargar(manejador.ObtenerFichasDeInscripcion(eventoSeleccionado, chkFichasCerradas.Checked, dtpFechaInicial.SelectedDate, dtpFechaFinal.SelectedDate));
                    X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.CambiosAplicados).Show();
                }
                else
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.FiltrosConDatosInvalidos).Show();
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

    }
}