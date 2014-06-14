using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZDateField : Ext.Net.DateField
    {
        public ZDateField()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MsgTarget = Ext.Net.MessageTarget.Side;
            this.BlankText = Literales.CampoRequerido;
            //this.Format = "dd/MMM/yyyy";
        }
    }
}
