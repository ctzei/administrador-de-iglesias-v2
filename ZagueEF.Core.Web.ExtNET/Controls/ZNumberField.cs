using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZNumberField : Ext.Net.NumberField
    {
        public ZNumberField()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MinHeight = 22;
            this.MsgTarget = Ext.Net.MessageTarget.Side;
            this.BlankText = Literales.CampoRequerido;
        }
    }
}
