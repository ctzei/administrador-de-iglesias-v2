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
    public partial class CatalogoDePasosPorMiembro : PaginaBase, ICatalogo
    {
        ManejadorDeMiembros manejadorDeMiembros;

        void ICatalogo.CargarControles()
        {
            manejadorDeMiembros = new ManejadorDeMiembros();

            Filtros.CargarControles();
            StoreCiclos.Cargar(Ciclo.Obtener());
            StorePasos.Cargar(MiembroPaso.Obtener());
            StoreCategorias.Cargar(PasoCategoria.Obtener());
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

            manejadorDeMiembros = new ManejadorDeMiembros();
            StorePasosPorMiembro.Cargar(manejadorDeMiembros.ObtenerPasosPorMiembro(id));
        }

        void ICatalogo.Borrar(int id)
        {
            throw new ExcepcionReglaNegocio(Resources.Literales.OperacionNovalidaBorrar);
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            manejadorDeMiembros = new ManejadorDeMiembros();
            manejadorDeMiembros.GuardarPasosPorMiembro(id, listaDeRegistrosDeDatos.Obtener(registroPasos.ClientID));
        }
    }
}