using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZSpinnerField : Ext.Net.SpinnerField
    {
        public ZSpinnerField()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MsgTarget = Ext.Net.MessageTarget.Side;
            this.BlankText = Literales.CampoRequerido;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.MinText = string.Format(Literales.ValorMinimoPermitido, this.MinValue);
            this.MaxText = string.Format(Literales.ValorMaximoPermitido, this.MaxValue); 
            base.OnPreRender(e);
        }
    }
}
