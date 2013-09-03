using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZMultiCombo : Ext.Net.MultiCombo
    {
        public ZMultiCombo()
            : base()
        {
            this.AnchorHorizontal = "97%";
            this.MsgTarget = Ext.Net.MessageTarget.Side;
            this.BlankText = Literales.CampoRequerido;
            this.EmptyText = Literales.Seleccione;
            this.DisplayField="Descripcion";
            this.ValueField = "Id";
        }
    }
}
