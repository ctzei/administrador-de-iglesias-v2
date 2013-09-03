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
    public partial class DetallesDeCelula : PaginaDeDetalle
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
            int celulaId = this.ObtenerId();
            cargarDatosGenerales(celulaId);
            cargarMiembros(celulaId);
        }

        private void cargarDatosGenerales(int celulaId)
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();
            Celula celula = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == celulaId select o).SingleOrDefault();

            registroId.Text = celula.CelulaId.ToString();
            registroDescripcion.Text = celula.Descripcion;
            registroRed.Text = registroRed.Text = manejadorCelulas.ObtenerRedSuperior(celula, " > ");
            registroMunicipio.Text = celula.UbicacionMunicipio.Descripcion;
            registroColonia.Text = celula.Colonia;
            registroDireccion.Text = celula.Direccion;
            registroDia.Text = celula.DiaSemana.Descripcion;
            registroHora.Text = celula.HoraDia.Descripcion;

            X.Call("cargarMapaDesdeDireccionEnPanel", gridDireccion.ClientID, celula.UbicacionMunicipio.UbicacionEstado.UbicacionPais.Descripcion, celula.UbicacionMunicipio.UbicacionEstado.Descripcion, celula.UbicacionMunicipio.Descripcion, celula.Colonia, celula.Direccion);

            StoreLideres.Cargar(from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                                where
                                o.CelulaId == celulaId &&
                                o.Borrado == false &&
                                o.Miembro.Borrado == false
                                orderby o.Miembro.Primer_Nombre, o.Miembro.Segundo_Nombre, o.Miembro.Apellido_Paterno, o.Miembro.Apellido_Materno, o.Miembro.Email
                                select new
                                {
                                    Id = o.MiembroId,
                                    Nombre = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno,
                                    Email = o.Miembro.Email,
                                    TelMovil = o.Miembro.Tel_Movil,
                                    TelCasa = o.Miembro.Tel_Casa,
                                    TelTrabajo = o.Miembro.Tel_Trabajo
                                });
        }

        private void cargarMiembros(int celulaId)
        {
            var miembros = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro
                            where
                            o.CelulaId == celulaId &&
                            o.Borrado == false
                            orderby o.Primer_Nombre, o.Segundo_Nombre, o.Apellido_Paterno, o.Apellido_Materno, o.Email
                            select new
                            {
                                Id = o.MiembroId,
                                Nombre = o.Primer_Nombre + " " + o.Segundo_Nombre + " " + o.Apellido_Paterno + " " + o.Apellido_Materno,
                                Email = o.Email
                            });

            StoreMiembros.Cargar(miembros);

            registroNumeroDeMiembros.Text = string.Format(registroNumeroDeMiembros.Text, miembros.Count());
        }
    }
}