using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZModalWindow : Ext.Net.Window
    {
        public ZModalWindow()
            : base()
        {
            this.Width = 640;
            this.Height = 480;
            this.Padding = 5;
            this.Collapsible = false;
            this.Draggable = false;
            this.Resizable = false;
            this.Hidden = true;
            this.Modal = true;
        }
    }
}
