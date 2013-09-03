using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZTextArea : Ext.Net.TextArea
    {
        public ZTextArea()
            : base()
        {
            this.MsgTarget = Ext.Net.MessageTarget.Side;
            this.BlankText = Literales.CampoRequerido;
            this.MinLength = 30;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.MinLengthText = string.Format(Literales.LongitudMinimaRequerida, this.MinLength);
            base.OnPreRender(e);
        }
    }
}
