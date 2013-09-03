using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZCheckbox : Ext.Net.Checkbox
    {
        public ZCheckbox()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MsgTarget = Ext.Net.MessageTarget.Side;
        }
    }
}
