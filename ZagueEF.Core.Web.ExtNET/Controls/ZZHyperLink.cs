using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZHyperLink : Ext.Net.HyperLink
    {
        public ZHyperLink()
            : base()
        {
        }

        public void CargarLiga(string liga)
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text = liga;
            }

            if (liga.StartsWith("www."))
            {
                liga = "http://" + liga;
            }

            this.NavigateUrl = liga;
        }
    }
}
