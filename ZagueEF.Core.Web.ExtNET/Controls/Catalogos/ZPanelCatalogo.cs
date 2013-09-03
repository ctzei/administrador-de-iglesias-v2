using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZPanelCatalogo : Ext.Net.FormPanel
    {
        public ZPanelCatalogo()
            : base()
        {
            this.HideBorders = true;
            this.BodyBorder = false;
            this.Layout = "form";
            this.Width = 800;
            this.Height = 250;
            this.LabelWidth = 105;
        }
    }
}
