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
    public partial class CatalogoDeCapitanes : PaginaBase, ICatalogo
    {
        ManejadorDeServidores manejadorServidores; 

        void ICatalogo.CargarControles()
        {
            Filtros.CargarControles();
            BuscadorCapitan.LimpiarControles();
            BuscadorIntegrantes.LimpiarControles();
        }

        void ICatalogo.Buscar()
        {
            Filtros.BuscarMiembros();
        }

        void ICatalogo.Mostrar(int id)
        {
            ManejadorDeServidores manejadorServidores = new ManejadorDeServidores();
            BuscadorCapitan.CargarListado(new [] {manejadorServidores.ObtenerCapitan(id)});
            BuscadorIntegrantes.CargarListado(manejadorServidores.ObtenerIntegrantesPorCapitan(id));
        }

        void ICatalogo.Borrar(int id)
        {
            manejadorServidores = new ManejadorDeServidores();
            manejadorServidores.BorrarCapitan(id);
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            //Obtenemos el id del capitan...
            if (id <= 0) {
                id = listaDeRegistrosDeDatos.Obtener(BuscadorCapitan.GridDeListadoDeObjetos.ClientID).RegistrosNuevosId[0];
            }

            manejadorServidores = new ManejadorDeServidores();
            manejadorServidores.GuardarIntegrantesPorCapitan(id, listaDeRegistrosDeDatos.Obtener(BuscadorIntegrantes.GridDeListadoDeObjetos.ClientID));
        }
    }
}