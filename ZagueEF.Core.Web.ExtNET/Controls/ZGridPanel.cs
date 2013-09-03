using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using System.Xml;
using System.Xml.Xsl;
using System.ComponentModel;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZGridPanel : Ext.Net.GridPanel
    {
        private bool mostrarInformacionExtra = false;
        private bool permitirExportar = true;
        private bool colorearFilas = false;
        private bool agregarColumnaParaBorrar = false;
        private string manejadorColumnaParaBorrar = null;
        private bool agregarColumnaParaEditar = false;
        private string manejadorColumnaParaEditar = null;

        [DefaultValue(true), DesignOnly(true), Description("Determina si se creara o no un menu contextual sobre el grid para mostrar informacion extra sobre el registro.")]
        public bool MostrarInformacionExtra { get { return mostrarInformacionExtra; } set { mostrarInformacionExtra = value; } }

        [DefaultValue(true), DesignOnly(true), Description("Determina si se creara o no un menu contextual sobre el grid para permitir exportar.")]
        public bool PermitirExportar { get { return permitirExportar; } set { permitirExportar = value; } }

        [DefaultValue(false), DesignOnly(false), Description("Determina si se van a colorear las filas de una manera especifica (con una columna llamada RowColor determinando el color).")]
        public bool ColorearFilas { get { return colorearFilas; } set { colorearFilas = value; } }

        [DefaultValue(false), DesignOnly(false), Description("Determina si se va a agregar una columna para poder borrar cada fila (NOTA: No campatible con grids con columnas de accion preexistentes).")]
        public bool AgregarColumnaParaBorrar { get { return agregarColumnaParaBorrar; } set { agregarColumnaParaBorrar = value; } }

        [DefaultValue(null), DesignOnly(false), Description("Determina la funcion que se va a ejecutar cuando se presione la columna de borrar")]
        public string ManejadorColumnaParaBorrar { get { return manejadorColumnaParaBorrar; } set { manejadorColumnaParaBorrar = value; } }

        [DefaultValue(false), DesignOnly(false), Description("Determina si se va a agregar una columna para poder editar cada fila (NOTA: No campatible con grids con columnas de accion preexistentes).")]
        public bool AgregarColumnaParaEditar { get { return agregarColumnaParaEditar; } set { agregarColumnaParaEditar = value; } }

        [DefaultValue(null), DesignOnly(false), Description("Determina la funcion que se va a ejecutar cuando se presione la columna de editar")]
        public string ManejadorColumnaParaEditar { get { return manejadorColumnaParaEditar; } set { manejadorColumnaParaEditar = value; } }

        public ZGridPanel(): base()
        {
            this.StripeRows = true;
            this.AnchorHorizontal = "97%";

            GridView gv = new GridView();
            gv.AutoFill = true;
            gv.ForceFit = true;
            this.View.Add(gv);
        }

        protected override void PageLoadComplete(object sender, EventArgs e)
        {
            crearMenuContextual();
            agregarManejadorParaColorearFilas();
            crearColumnasDeAccion();
            base.PageLoadComplete(sender, e);
        }

        protected override void OnLoad(EventArgs e)
        {
            iniciarlizarGridParaExportar();
            base.OnLoad(e);
        }

        #region Colorear Fila

        void agregarManejadorParaColorearFilas()
        {
            if ((this.ColorearFilas) && (!Ext.Net.X.IsAjaxRequest))
            {
                this.View.View.GetRowClass.Handler = "if ((record.data.RowColor) && (record.data.RowColor.trim().length > 0)){return 'x-grid-row-' + record.data.RowColor.toLowerCase();}";
            }
        }

        #endregion

        #region Exportar

        void iniciarlizarGridParaExportar()
        {
            if (this.PermitirExportar)
            {
                Store store = this.GetStore();
                if (store != null)
                {
                    store.SubmitData += new Ext.Net.Store.AjaxSubmitDataEventHandler(ZGridPanel_SubmitData);
                    store.DirectEventConfig.IsUpload = true;
                }
            }
        }

        void ZGridPanel_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            XmlNode xml = e.Xml;
            this.Page.Response.Clear();
            this.Page.Response.ContentType = "application/octet-stream";
            this.Page.Response.AddHeader("Content-Disposition", "attachment; filename=exportado.csv");
            XslCompiledTransform xtExcel = new XslCompiledTransform();
            xtExcel.Load(typeof(CsvXsl));
            xtExcel.Transform(xml, null, this.Page.Response.OutputStream);
            this.Page.Response.End();
        }

        #endregion

        #region Menu Contextual

        void crearMenuContextual()
        {
            if (((this.PermitirExportar) || (this.MostrarInformacionExtra)) && (!Ext.Net.X.IsAjaxRequest))
            {
                Menu m = new Menu();
                MenuItem mi;

                if (this.MostrarInformacionExtra)
                {
                    mi = new MenuItem("Mostrar Informacion Extra...");
                    mi.ID = this.ClientID + "_mnuInformacionExtra";
                    mi.OnClientClick = string.Format("mostrarInformacionExtra(Ext.getCmp('{0}'), Ext.getCmp('{0}').selModel.getSelected());", this.ClientID);
                    mi.Icon = Ext.Net.Icon.Magnifier;
                    m.Items.Add(mi);
                }

                if (this.PermitirExportar)
                {
                    mi = new MenuItem("Exportar...");
                    mi.ID = this.ClientID + "_mnuExportar";
                    mi.OnClientClick = string.Format("{0}.submitData(false);", this.ClientID);
                    mi.Icon = Ext.Net.Icon.PageExcel;
                    m.Items.Add(mi);
                }

                m.ID = this.ClientID + "_contextMenu";
                this.Page.Controls.Add(m);
                this.ContextMenuID = m.ID;
            }
        }

        #endregion

        #region Crear Columnas de Accion

        private void crearColumnasDeAccion()
        {
            if ((agregarColumnaParaBorrar || agregarColumnaParaEditar) && (!Ext.Net.X.IsAjaxRequest))
            {
                CommandColumn column = new CommandColumn();
                column.Width = 30;

                if (agregarColumnaParaBorrar)
                {
                    if (string.IsNullOrEmpty(manejadorColumnaParaBorrar))
                    {
                        this.Listeners.Command.Handler += " if (command == 'Borrar') { this.deleteRecord(record); } ";
                    }
                    else
                    {
                        this.Listeners.Command.Handler += string.Format(" if (command == 'Borrar') {{ {0}.call(this, record); }} ", manejadorColumnaParaBorrar);
                    }

                    GridCommand command = new GridCommand();
                    command.CommandName = "Borrar";
                    command.Icon = Ext.Net.Icon.Delete;
                    column.Commands.Add(command);
                }

                if (agregarColumnaParaEditar)
                {
                    if (string.IsNullOrEmpty(manejadorColumnaParaEditar))
                    {
                        throw new ExcepcionAplicacion("El paramatero ManejadorColumnaParaEditar no puede estar vacio si el parametro AgregarColumnaParaEditar es TRUE.");
                    }
                    else
                    {
                        this.Listeners.Command.Handler += string.Format(" if (command == 'Editar') {{ {0}.call(this, record); }} ", manejadorColumnaParaEditar);
                    }

                    GridCommand command = new GridCommand();
                    command.CommandName = "Editar";
                    command.Icon = Ext.Net.Icon.TableEdit;
                    column.Commands.Add(command);
                }

                this.ColumnModel.Columns.Add(column);
            }
        }

       
        #endregion
    }
}
