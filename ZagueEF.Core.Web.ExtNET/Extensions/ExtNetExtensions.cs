using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;

namespace ExtensionMethods
{
    public static class ExtNetExtensions
    {
        #region Store

        public static void Cargar(this Store s, object entidades)
        {
            if (entidades != null)
            {
                if (s.DataSource == null)
                {
                    s.DataSource = entidades;
                    s.DataBind();
                }
                else
                {
                    s.LoadData(entidades, false);
                }
            }
            else
            {
                s.LoadData(new { }, false);
            }
        }

        public static void Limpiar(this Store s)
        {
            s.LoadData(new { }, false);
            s.RejectChanges();
        }

        #endregion

        #region GridPanel

        public static void MarcarSucio(this GridPanel g)
        {
            //function setAllRowsAsModified(grid) {
            //    if (typeof (grid) === "string") {
            //        grid = Ext.getCmp(grid);
            //    }

            //    var records = grid.store.data.items;
            //    for (var i = 0; i < records.length; i++) {
            //        var record = records[i];

            //        record.dirty = true;
            //        if (!record.modified) {
            //            record.modified = {};
            //        }
            //        record.fields.each(function (f) {
            //            record.modified[f.name] = null;
            //        });
            //        if (record.store) {
            //            record.store.afterEdit(record);
            //        }
            //    }
            //}

            X.Call("(function(){var d=" + g.ClientID + ";var b=d.store.data.items;for(var c=0;c<b.length;c++){var a=b[c];a.dirty=true;if(!a.modified){a.modified={}}a.fields.each(function(e){a.modified[e.name]=null});if(a.store){a.store.afterEdit(a)}}})");
        }

        #endregion

        #region ComboBox

        public static void ForzarSeleccion(this ComboBox c, int id, string texto)
        {
            c.Value = id;
            c.SetRawValue(texto);
        }

        public static int ObtenerId(this ComboBox c){
            int rtn;
            if (!int.TryParse(c.SelectedItem.Value, out rtn))
            {
                return -1;
            }
            return rtn;
        }

        public static int? ObtenerId(this ComboBox c, bool useNull)
        {
            int rtn;
            if (!int.TryParse(c.SelectedItem.Value, out rtn))
            {
                if (useNull)
                {
                    return null;
                }
                else
                {
                    return -1;
                }
            }
            return rtn;
        }

        #endregion

        #region MultiCombo

        public static List<int> ObtenerIds(this MultiCombo c)
        {
            List<int> rtn;
            try
            {
                rtn = (from o in c.SelectedItems select int.Parse(o.Value)).ToList<int>();
            }
            catch
            {
                rtn = new List<int>();
            }
            return rtn;
        }

        #endregion
    }
}
