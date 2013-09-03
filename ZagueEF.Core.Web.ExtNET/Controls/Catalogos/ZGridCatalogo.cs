using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZGridCatalogo : ZGridPanel
    {
        public ZGridCatalogo()
            : base()
        {
            this.HideBorders = true;
            this.BodyBorder = true;
            this.AnchorHorizontal = "right";
            this.AutoHeight = true;
            this.Listeners.RowDblClick.Handler = "mostrarClick();";

            Ext.Net.RowSelectionModel sModel = new Ext.Net.RowSelectionModel();
            sModel.SingleSelect = true;

            this.SelectionModel.Add(sModel);
        }
    }
}
