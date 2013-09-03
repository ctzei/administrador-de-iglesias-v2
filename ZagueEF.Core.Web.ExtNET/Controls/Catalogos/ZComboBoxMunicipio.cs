using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using ExtensionMethods;

namespace ZagueEF.Core.Web.ExtNET.Controls
{
    public class ZComboBoxMunicipio : ZComboBox
    {
        public ZComboBoxMunicipio()
            : base()
        {

            #region Creamos el STORE
            HttpProxy proxy = new HttpProxy
            {
                Method = HttpMethod.GET,
                Url = "~/WebServices/Generales/Municipios.aspx"
            };

            JsonReader reader = new JsonReader
            {
                Root = "municipios",
                Fields = {
                    new RecordField("Id"),
                    new RecordField("Municipio"),
                    new RecordField("Estado"),
                    new RecordField("Pais")
                }
            };

            Store store = new Store
            {
                Proxy = { proxy },
                Reader = { reader },
                AutoLoad = false
            };

            this.Store.Add(store);
            #endregion 

            this.FieldLabel = "Municipio";
            this.DisplayField = "Municipio";
            this.ValueField = "Id";
            this.TypeAhead = false;
            this.LoadingText = "Cargando...";
            this.EmptyText = Literales.Buscar;
            this.ValueNotFoundText = Literales.Buscar;
            this.HideTrigger = true;
            this.MinChars = 3;
            this.Mode = DataLoadMode.Remote;
            this.QueryParam = "municipio";
            this.ItemSelector = "div.search-item";

            System.Text.StringBuilder sb = new System.Text.StringBuilder(500);
            sb.Append(@"<tpl for=""."">");
            sb.Append(@"<div class=""search-item"">");
            sb.Append(@"<h3>{Municipio}</h3><span>[{Estado}][{Pais}]</span>");
            sb.Append(@"</div>");	  
            sb.Append(@"</tpl>");

            this.Template.Html = sb.ToString();
        }

        public void SeleccionarMunicipio(int municipioId, string municipioDescripcion)
        {
            this.Value = municipioId;
            this.SetRawValue(municipioDescripcion);

            var registro = new [] {new {Id = municipioId, Municipio = municipioDescripcion}};

            if (this.GetStore().DataSource == null)
            {
                this.GetStore().DataSource = registro;
                this.GetStore().DataBind();
            }
            else
            {
                this.GetStore().LoadData(registro, false);
            }

        }
    }
}
