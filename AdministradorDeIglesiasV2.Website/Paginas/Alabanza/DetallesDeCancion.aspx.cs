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
    public partial class DetallesDeCancion : PaginaDeDetalle
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
            int cancionId = this.ObtenerId();
            cargarDatosGenerales(cancionId);
        }

        private void cargarDatosGenerales(int cancionId)
        {
            AlabanzaCancion cancion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion where o.Id == cancionId select o).SingleOrDefault();

            registroId.Value = cancion.Id;
            registroTitulo.Value = cancion.Titulo;
            registroArtista.Value = cancion.Artista;
            registroDisco.Value = cancion.Disco;
            registroTituloAlternativo.Value = cancion.TituloAlternativo;
            registroArtistaAlternativo.Value = cancion.ArtistaAlternativo;
            registroDiscoAlternativo.Value = cancion.DiscoAlternativo;
            registroLiga.CargarLiga(cancion.Liga);
            registroLigaAlternativa.CargarLiga(cancion.LigaAlternativa);
            registroTono.Value = cancion.Tono;
            registroLetra.Value = cancion.Letra;
        }
    }
}