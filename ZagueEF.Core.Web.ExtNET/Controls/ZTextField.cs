using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZTextField : Ext.Net.TextField
    {
        public ZTextField()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MinHeight = 22;
            this.MsgTarget = Ext.Net.MessageTarget.Side;
            this.BlankText = Literales.CampoRequerido;
            this.VtypeText = Literales.FormatoDeEmailIncorrecto;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.MinLengthText = string.Format(Literales.LongitudMinimaRequerida, this.MinLength);
            base.OnPreRender(e);
        }
    }
}
