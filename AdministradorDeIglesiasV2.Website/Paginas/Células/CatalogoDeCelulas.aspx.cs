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
    public partial class CatalogoDeCelulas : PaginaBase, ICatalogo
    {
        void ICatalogo.CargarControles()
        {
            StoreDiasDeLaSemana.Cargar(DiaSemana.Obtener());
            StoreHorasDelDia.Cargar(HoraDia.Obtener());
            StoreCelulaCategorias.Cargar(CelulaCategoria.Obtener());
        }

        void ICatalogo.Buscar()
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();

            List<int> idsDiasDeLaSemana = filtroDiaDeLaSemana.ObtenerIds();
            List<int> idsHorasDelDia = filtroHoraDelDia.ObtenerIds();
            List<int> idCategorias = filtroCategoria.ObtenerIds();
            int idMunicipio = filtroMunicipio.ObtenerId();
            List<int> idsCelulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(SesionActual.Instance.UsuarioId);
            List<int> idsCelulasSinLider = manejadorCelulas.ObtenerCelulasSinLideresComoCelulas().Select(o => o.CelulaId).ToList<int>();

            StoreResultados.Cargar((
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                where
                    ((o.CelulaId == (filtroId.Number > 0 ? filtroId.Number : o.CelulaId)) &&
                    ((o.Descripcion.Contains(filtroDescripcion.Text)) || (o.Descripcion == null)) &&
                    (idsDiasDeLaSemana.Contains(o.DiaSemanaId) || (idsDiasDeLaSemana.Count == 0)) &&
                    (idsHorasDelDia.Contains(o.HoraDiaId) || (idsHorasDelDia.Count == 0)) &&
                    (idCategorias.Contains(o.CategoriaId) || (idCategorias.Count == 0)) &&
                    (o.UbicacionMunicipioId == (idMunicipio > 0 ? idMunicipio : o.UbicacionMunicipioId)) &&
                    ((o.Colonia.Contains(filtroColonia.Text)) || (o.Colonia == null)) &&
                    ((o.Direccion.Contains(filtroDireccion.Text)) || (o.Direccion == null)) &&
                    (o.Borrado == false) && //Registros NO borrados
                    (idsCelulasPermitidas.Contains(o.CelulaId) || idsCelulasSinLider.Contains(o.CelulaId))) //Dentro de las celulas permitidas para el usuario actual... o aquellas celulas sin lider asignado (para poderlas BORRAR)
                orderby
                    o.Descripcion
                select new {
                    Id = o.CelulaId, 
                    Descripcion = o.Descripcion,
                    DiaSemanaDesc = o.DiaSemana.Descripcion,
                    HoraDiaDesc = o.HoraDia.Descripcion,
                    Municipio = o.UbicacionMunicipio.Descripcion,
                    Colonia = o.Colonia,
                    Direccion = o.Direccion,
                    Categoria = o.CelulaCategoria.Descripcion,
                    Coordenadas = o.Coordenadas,
                    RowColor = idsCelulasSinLider.Contains(o.CelulaId) ? "red" : string.Empty
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            Celula entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == id select o).FirstOrDefault();
            registroDescripcion.Text = entidad.Descripcion;
            registroId.Text = entidad.CelulaId.ToString();
            registroDiaDeLaSemana.Value = entidad.DiaSemanaId;
            registroHoraDelDia.Value = entidad.HoraDiaId;
            registroCategoria.Value = entidad.CategoriaId;
            registroMunicipio.ForzarSeleccion(entidad.UbicacionMunicipioId, entidad.UbicacionMunicipio.Descripcion);
            registroColonia.Value = entidad.Colonia;
            registroDireccion.Value = entidad.Direccion;
            registroCoordenadas.Value = entidad.Coordenadas;
        }

        void ICatalogo.Borrar(int id)
        {
            Celula entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            Celula entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new Celula();
            }

            entidad.Descripcion = registroDescripcion.Text;
            entidad.DiaSemanaId = registroDiaDeLaSemana.ObtenerId();
            entidad.HoraDiaId = registroHoraDelDia.ObtenerId();
            entidad.CategoriaId = registroCategoria.ObtenerId();
            entidad.UbicacionMunicipioId = registroMunicipio.ObtenerId();
            entidad.Colonia = registroColonia.Text;
            entidad.Direccion = registroDireccion.Text;
            entidad.Coordenadas = registroCoordenadas.Text;
            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

            this.RestablecerCacheDeSesion();
        }
    }
}