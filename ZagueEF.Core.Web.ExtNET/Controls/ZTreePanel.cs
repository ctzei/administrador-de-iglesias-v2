using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZTreePanel : Ext.Net.TreePanel
    {
        private bool mostrarExpanderYContraer = true;

        [DefaultValue(true), DesignOnly(true), Description("Determina si se agregara o no los iconos que sirven para expander y/o contraer el arbol.")]
        public bool MostrarExpanderYContraer { get { return mostrarExpanderYContraer; } set { mostrarExpanderYContraer = value; } }

        public ZTreePanel()
            : base()
        {
            this.AutoScroll = true;   
        }

        protected override void PageLoadComplete(object sender, EventArgs e)
        {
            CrearHerramientasExtras();
            base.PageLoadComplete(sender, e);
        }

        private void CrearHerramientasExtras()
        {
            if ((this.mostrarExpanderYContraer) && (!Ext.Net.X.IsAjaxRequest))
            {
                this.Tools.Add(new Ext.Net.Tool(Ext.Net.ToolType.Plus, string.Format("{0}.expandAll();", this.ClientID), "Expander Todo"));
                this.Tools.Add(new Ext.Net.Tool(Ext.Net.ToolType.Minus, string.Format("{0}.collapseAll();", this.ClientID), "Contraer Todo"));
            }
        }
    }
}
