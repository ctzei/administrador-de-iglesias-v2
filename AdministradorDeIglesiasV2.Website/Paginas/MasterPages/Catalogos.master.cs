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

namespace AdministradorDeIglesiasV2.Website.Paginas.MasterPages
{
    public partial class Catalogos : MasterPageBase
    {
        private ICatalogo catalogo;

        protected void Page_Load(object sender, EventArgs e)
        {
            catalogo = (ICatalogo)this.Page;

            if (!X.IsAjaxRequest)
            {
                catalogo.CargarControles();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void Buscar()
        {
            catalogo.Buscar();
        }

        [DirectMethod(ShowMask = true)]
        public void Mostrar(int id)
        {
            try
            {
                try
                {
                    catalogo.Mostrar(id);
                }
                catch (Exception ex)
                {
                    throw new ExcepcionReglaNegocio(Resources.Literales.ErrorAlCargar, ex);
                }
            }
            catch (ExcepcionReglaNegocio exRegla)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, exRegla.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void Borrar(int id)
        {
            try
            {
                catalogo.Borrar(id);
                catalogo.Buscar();
                X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.CambiosAplicados).Show();
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, "Registro no eliminado - " + ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void Guardar(int id, string sListaDeRegistrosDeDatos)
        {
            try
            {
                RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos;
                if (sListaDeRegistrosDeDatos.Trim().Length > 0)
                {
                    listaDeRegistrosDeDatos = new RegistrosHelper.ListaDeRegistrosDeDatos();
                    foreach (KeyValuePair<string, List<Dictionary<string, string>>> grid in JSON.Deserialize<Dictionary<string, List<Dictionary<string, string>>>>(sListaDeRegistrosDeDatos))
                    {
                        listaDeRegistrosDeDatos.Agregar(RegistrosHelper.ObtenerRegistrosDiferenciados(grid.Value), grid.Key);
                    }
                }
                else
                {
                    listaDeRegistrosDeDatos = null;
                }

                catalogo.Guardar(id, listaDeRegistrosDeDatos); //Los registros extra son los registros de los GRIDS que son parte de la EDICION del registro
                catalogo.Buscar();
                X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.CambiosAplicados).Show();
                X.Call("cancelarClick");
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, "Cambios no guardados - " + ex.Message).Show();
            }
        }

        /// <summary>
        /// Este metodo es usado UNICAMENTE cuando en el panel de Edicion se cuentan con "Grids", para poder recargar la informacion cada que se "Cancela"
        /// </summary>
        [DirectMethod(ShowMask = true)]
        public void RecargarControles()
        {
            catalogo.CargarControles();
        }

    }
}