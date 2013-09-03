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
    public partial class CatalogoDeLideresDeCelula : PaginaBase, ICatalogo
    {
        private ManejadorDeLideresDeCelula manejadorDeLideresdeCelulas;

        void ICatalogo.CargarControles()
        {
            Filtros.CargarControles();
            Buscador.LimpiarControles();
        }

        void ICatalogo.Buscar()
        {
            Filtros.BuscarMiembros();
        }

        void ICatalogo.Mostrar(int id)
        {
            Miembro entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == id select o).FirstOrDefault();
            registroId.Value = entidad.MiembroId.ToString();
            registroEmail.Value = entidad.Email;
            registroPrimerNombre.Value = entidad.Primer_Nombre;
            registroSegundoNombre.Value = entidad.Segundo_Nombre;
            registroApellidoPaterno.Value = entidad.Apellido_Paterno;
            registroApellidoMaterno.Value = entidad.Apellido_Materno;

            manejadorDeLideresdeCelulas = new ManejadorDeLideresDeCelula();
            Buscador.CargarListado(manejadorDeLideresdeCelulas.ObtenerLiderazgoDeCelulas(id));
        }

        void ICatalogo.Borrar(int id)
        {
            throw new ExcepcionReglaNegocio(Resources.Literales.OperacionNovalidaBorrar);
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            manejadorDeLideresdeCelulas = new ManejadorDeLideresDeCelula();
            manejadorDeLideresdeCelulas.GuardarLiderzagoDeCelulas(id, listaDeRegistrosDeDatos.Obtener(Buscador.GridDeListadoDeObjetos.ClientID));
            this.RestablecerCacheDeSesion();
        }
    }
}