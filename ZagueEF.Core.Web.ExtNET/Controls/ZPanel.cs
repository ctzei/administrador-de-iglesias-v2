using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZPanel : Ext.Net.FormPanel
    {
        public ZPanel()
            : base()
        {
            this.AnchorHorizontal = "100%";
            this.BodyBorder = false;
        }
    }
}
