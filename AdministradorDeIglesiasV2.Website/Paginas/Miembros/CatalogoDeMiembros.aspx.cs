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
    public partial class CatalogoDeMiembros : PaginaBase, ICatalogo
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.wndFoto.RenderTo = "extra-upload-form"; //Agregamos la ventana de de la foto a OTRO form para poder hacer post por separado
        }

        void ICatalogo.CargarControles()
        {
            Filtros.CargarControles();
            registroConyuge.Limpiar();
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
            registroGenero.Value = entidad.GeneroId;
            registroCelula.Value = entidad.CelulaId;
            registroAsisteIglesia.Checked = entidad.AsisteIglesia;
            registroPassword.Value = entidad.Contrasena;
            registroEstadoCivil.Value = entidad.EstadoCivilId;
            registroMunicipio.SeleccionarMunicipio(entidad.UbicacionMunicipioId, entidad.UbicacionMunicipio.Descripcion);
            registroColonia.Value = entidad.Colonia;
            registroDireccion.Value = entidad.Direccion;
            registroTelCasa.Value = entidad.Tel_Casa;
            registroTelMovil.Value = entidad.Tel_Movil;
            registroTelTrabajo.Value = entidad.Tel_Trabajo;
            registroFechaDeNacimiento.Value = entidad.Fecha_Nacimiento;
            registroConyuge.EstablecerId(entidad.ConyugeId);
        }

        void ICatalogo.Borrar(int id)
        {
            Miembro entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            Miembro entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new Miembro();
            }

            entidad.Email = registroEmail.Text;
            entidad.Primer_Nombre = registroPrimerNombre.Text;
            entidad.Segundo_Nombre = registroSegundoNombre.Text;
            entidad.Apellido_Paterno = registroApellidoPaterno.Text;
            entidad.Apellido_Materno = registroApellidoMaterno.Text;
            entidad.CelulaId = registroCelula.ObtenerId();
            entidad.GeneroId = registroGenero.ObtenerId();
            entidad.AsisteIglesia = registroAsisteIglesia.Checked;
            entidad.Contrasena = registroPassword.Text;
            entidad.EstadoCivilId = registroEstadoCivil.ObtenerId();
            entidad.UbicacionMunicipioId = registroMunicipio.ObtenerId();
            entidad.Colonia = registroColonia.Text;
            entidad.Direccion = registroDireccion.Text;
            entidad.Tel_Casa = registroTelCasa.Text;
            entidad.Tel_Movil = registroTelMovil.Text;
            entidad.Tel_Trabajo = registroTelTrabajo.Text;
            entidad.Fecha_Nacimiento = registroFechaDeNacimiento.SelectedDate;
            entidad.ConyugeId = registroConyuge.ObtenerId(true);
            entidad.Borrado = false;
            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }
    }
}