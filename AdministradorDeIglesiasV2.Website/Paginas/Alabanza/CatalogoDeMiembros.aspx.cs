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
    public partial class CatalogoDeMiembros : PaginaBase
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
            StoreMiembros.Cargar(from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro
                                 where o.Borrado == false
                                 orderby
                                     o.Miembro.Primer_Nombre,
                                     o.Miembro.Segundo_Nombre,
                                     o.Miembro.Apellido_Paterno,
                                     o.Miembro.Apellido_Materno
                                 select new
                                 {
                                     Id = o.Id,
                                     MiembroId = o.MiembroId,
                                     Nombre = o.Miembro.Primer_Nombre + " " +  o.Miembro.Segundo_Nombre + " " +  o.Miembro.Apellido_Paterno+ " " +  o.Miembro.Apellido_Materno,
                                     Email = o.Miembro.Email,
                                     Telefono = o.Miembro.Tel_Casa + " | " + o.Miembro.Tel_Movil + " | " + o.Miembro.Tel_Trabajo,
                                     Celula = o.Miembro.Celula.Descripcion,
                                     CelulaId = o.Miembro.CelulaId
                                 });

            // Limpiamos el popup
            ((FormPanel)wndAgregarMiembro.Items[0]).Reset();
            registroMiembro.Limpiar();
        }

        [DirectMethod(ShowMask = true)]
        public void AgregarMiembro()
        {
            try
            {
                int miembroId = registroMiembro.ObtenerId();
                if (miembroId > 0)
                {
                    AlabanzaMiembro alabanzaMiembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro where o.MiembroId == miembroId select o).SingleOrDefault() ?? new AlabanzaMiembro();
                    alabanzaMiembro.MiembroId = miembroId;
                    alabanzaMiembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    CargarControles();
                    wndAgregarMiembro.Hide();
                    X.Msg.Notify(Generales.nickNameDeLaApp, "Miembro agregadado correctamente").Show();
                }
                else
                {
                    throw new ExcepcionReglaNegocio("Favor de seleccionar algun miembro valido.");
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void BorrarMiembro(int id)
        {
            try
            {
                if (id > 0)
                {
                    AlabanzaMiembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro where o.Id == id select o).SingleOrDefault();
                    miembro.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    CargarControles();
                    X.Msg.Notify(Generales.nickNameDeLaApp, "Miembro eliminado correctamente").Show();
                }
                else
                {
                    throw new ExcepcionReglaNegocio("Id de miembro invalido.");
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }


    }
}