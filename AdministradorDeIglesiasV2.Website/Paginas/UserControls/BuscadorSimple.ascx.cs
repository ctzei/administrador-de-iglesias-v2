using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using ExtensionMethods;
using Ext.Net;
using LinqKit;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Enums;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;

using ZagueEF.Core.Web.ExtNET.Controls;
using System.Data.Objects.DataClasses;

namespace AdministradorDeIglesiasV2.Website.Paginas.UserControls
{
    public partial class BuscadorSimple : System.Web.UI.UserControl
    {
        #region Propiedades

        private ManejadorDeBusquedas.TipoDeObjeto _tipoDeObjeto = ManejadorDeBusquedas.TipoDeObjeto.Celula;
        private string _fieldLabel = "MiTitulo";
        private bool _allowBlank = true;
        private int _labelWidth = 100;

        [DefaultValue(typeof(ManejadorDeBusquedas.TipoDeObjeto), "Celula"), DesignOnly(true), Description("Determina los tipos de elementos que se desean buscar.")]
        public ManejadorDeBusquedas.TipoDeObjeto TipoDeObjeto { get { return _tipoDeObjeto; } set { _tipoDeObjeto = value; } }

        [DefaultValue("MiTitulo"), DesignOnly(true), Description("Titulo (label) del campo que contiene el objeto seleccionado.")]
        public string FieldLabel { get { return _fieldLabel; } set { _fieldLabel = value; } }

        [DefaultValue(false), DesignOnly(true), Description("Determina si el valor puede ir en blanco (vacio).")]
        public bool AllowBlank { get { return _allowBlank; } set { _allowBlank = value; } }

        [DefaultValue(100), DesignOnly(true), Description("Establece el valor de la longitud de la descripcion del campo.")]
        public int LabeLWidth { get { return _labelWidth; } set { _labelWidth = value; } }


        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this._fieldLabel))
            {
                this.objetoSeleccionado.FieldLabel = _fieldLabel;
            }

            _establecerTipoDeObjeto();
            this.objetoSeleccionado.AllowBlank = _allowBlank;
            this.BuscadorSimpleContenedor.LabelWidth = _labelWidth;
            base.OnPreRender(e);
        }

        protected void cmdBuscarConcepto_Click(object sender, DirectEventArgs e)
        {
            ManejadorDeBusquedas manejadorDeBuscador = new ManejadorDeBusquedas();
            IEnumerable<object> resultados = manejadorDeBuscador.Buscar(_tipoDeObjeto, registroConceptoABuscar.Text.Trim().Split(' '));

            if (resultados != null)
            {
                int numeroDeResultadosMax = 75; //Es el numero maximo de resultados a regresar al cliente...
                int numeroDeResultados = resultados.Count();
                if (numeroDeResultados > numeroDeResultadosMax)
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, string.Format(Resources.Literales.LimiteDeResultadosExcedido, numeroDeResultados, numeroDeResultadosMax)).Show();
                    numeroDeResultados = numeroDeResultadosMax;
                }
                StoreObjetosEncontrados.Cargar(resultados.Take(numeroDeResultadosMax));
                registroNumeroDeResultados.Text = string.Format("{0} Resultados", numeroDeResultados);
            }

        }

        public int ObtenerId()
        {
            return objetoSeleccionado.ObtenerId();
        }

        public int? ObtenerId(bool useNull)
        {
            return objetoSeleccionado.ObtenerId(useNull);
        }

        public void EstablecerId(int id)
        {
            RegistroBasico rtn = null;
            switch (_tipoDeObjeto){
                case ManejadorDeBusquedas.TipoDeObjeto.Celula:
                    rtn = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == id select new RegistroBasico{
                        Id = o.CelulaId,
                        Descripcion = o.Descripcion
                    }).FirstOrDefault();
                    break;
                case ManejadorDeBusquedas.TipoDeObjeto.Miembro:
                    rtn = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == id select new RegistroBasico{
                        Id = o.MiembroId,
                        Descripcion = o.Primer_Nombre + " " + o.Segundo_Nombre + " " + o.Apellido_Paterno + " " + o.Apellido_Materno + " (" + o.Email + ")"
                    }).FirstOrDefault();
                    break;
                case ManejadorDeBusquedas.TipoDeObjeto.AlabanzaMiembro:
                    rtn = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro where o.Id == id select new RegistroBasico{
                        Id = o.Id,
                        Descripcion = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno + " (" + o.Miembro.Email + ")"
                    }).FirstOrDefault();
                    break;
            }

            StoreObjetoSeleccionado.Cargar(new [] {rtn});
            objetoSeleccionado.Value = id;
        }

        public void EstablecerId(int? id)
        {
            if (id.HasValue)
            {
                EstablecerId(id.Value);
            }
        }

        private void _establecerTipoDeObjeto()
        {
            X.AddScript(string.Format("{0}.TipoDeObjeto = '{1}';", this.objetoSeleccionado.ClientID, this._tipoDeObjeto));
        }

        public void Limpiar()
        {
            registroConceptoABuscar.Text = "";
            registroNumeroDeResultados.Text = "";
            StoreObjetoSeleccionado.Limpiar();
            StoreObjetosEncontrados.Limpiar();
            this.objetoSeleccionado.Text = "";
        }
    }
}