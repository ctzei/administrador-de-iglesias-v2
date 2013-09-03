using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZCompositeField : Ext.Net.CompositeField
    {
        public ZCompositeField()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MsgTarget = Ext.Net.MessageTarget.Side;
        }
    }
}
