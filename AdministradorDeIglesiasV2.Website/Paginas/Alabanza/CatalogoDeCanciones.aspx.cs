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
    public partial class CatalogoDeCanciones : PaginaBase, ICatalogo
    {
        void ICatalogo.CargarControles()
        {
        }

        void ICatalogo.Buscar()
        {
            StoreResultados.Cargar((
               from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion
               where
                   (o.Id == (filtroId.Number > 0 ? filtroId.Number : o.Id)) &&
                   (o.Titulo.Contains(filtroTitulo.Text)) &&
                   (o.Artista.Contains(filtroArtista.Text)) &&
                   (o.Disco.Contains(filtroDisco.Text)) &&
                   (o.TituloAlternativo.Contains(filtroTituloAlternativo.Text)) &&
                   (o.ArtistaAlternativo.Contains(filtroArtistaAlternativo.Text)) &&
                   (o.DiscoAlternativo.Contains(filtroDiscoAlternativo.Text)) &&
                   (o.Liga.Contains(filtroLiga.Text)) &&
                   (o.LigaAlternativa.Contains(filtroLigaAlternativa.Text)) &&
                   (o.Tono.Contains(filtroTono.Text)) &&
                   (o.Letra.Contains(filtroLetra.Text))
               select new
               {
                   Id = o.Id,
                   Titulo = o.Titulo,
                   Artista = o.Artista,
                   Disco = o.Disco,
                   TituloAlternativo = o.TituloAlternativo,
                   ArtistaAlternativo = o.ArtistaAlternativo,
                   DiscoAlternativo = o.DiscoAlternativo,
                   Liga = o.Liga,
                   LigaAlternativa = o.LigaAlternativa,
                   Tono = o.Tono
               }));
        }

        void ICatalogo.Mostrar(int id)
        {
            AlabanzaCancion entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion where o.Id == id select o).FirstOrDefault();
            registroId.Text = entidad.Id.ToString();
            registroTitulo.Text = entidad.Titulo;
            registroArtista.Text = entidad.Artista;
            registroDisco.Text = entidad.Disco;
            registroTituloAlternativo.Text = entidad.TituloAlternativo;
            registroArtistaAlternativo.Text = entidad.ArtistaAlternativo;
            registroDiscoAlternativo.Text = entidad.DiscoAlternativo;
            registroLiga.Text = entidad.Liga;
            registroLigaAlternativa.Text = entidad.LigaAlternativa;
            registroTono.Text = entidad.Tono;
            registroLetra.Text = entidad.Letra;
        }

        void ICatalogo.Borrar(int id)
        {
            AlabanzaCancion entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion where o.Id == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            AlabanzaCancion entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion where o.Id == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new AlabanzaCancion();
            }

            entidad.Titulo = registroTitulo.Text;
            entidad.Artista = registroArtista.Text;
            entidad.Disco = registroDisco.Text;
            entidad.TituloAlternativo = registroTituloAlternativo.Text;
            entidad.ArtistaAlternativo = registroArtistaAlternativo.Text;
            entidad.DiscoAlternativo = registroDiscoAlternativo.Text;
            entidad.Liga = registroLiga.Text;
            entidad.LigaAlternativa = registroLigaAlternativa.Text;
            entidad.Tono = registroTono.Text;
            entidad.Letra = registroLetra.Text;

            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }
    }
}