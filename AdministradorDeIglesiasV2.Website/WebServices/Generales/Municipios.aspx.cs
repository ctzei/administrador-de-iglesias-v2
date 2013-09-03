using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using ZagueEF.Core.Web.ExtNET;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;


using System.Web.Caching;

namespace AdministradorDeIglesiasV2.Website.WebServices.Generales
{
    public partial class Municipios : JsonWebServiceBase
    {
        protected override void OnInit(EventArgs e)
        {
            this.ValidarNumeroDeEjecuciones = false;
            this.OcupaIniciarSesion = false;
            base.OnInit(e);
        }

        public override string ManejadorBase()
        {
            //Parametros obtenidos del request
            string sMunicipio = Request["municipio"];
            string sEstado = Request["estado"];
            string sEstadoId = Request["estadoId"];

            int estadoId;
            int.TryParse(sEstadoId, out estadoId);

            object municipios = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().UbicacionMunicipio
                                 where 
                                    (!string.IsNullOrEmpty(sMunicipio) && o.Descripcion.Contains(sMunicipio)) ||
                                    (!string.IsNullOrEmpty(sEstado) && o.UbicacionEstado.Descripcion.Contains(sEstado)) ||
                                    (estadoId > 0 && o.UbicacionEstadoId == estadoId)
                                 select new
                                 {
                                     Id = o.UbicacionMunicipioId,
                                     Municipio = o.Descripcion,
                                     Estado = o.UbicacionEstado.Descripcion,
                                     Pais = o.UbicacionEstado.UbicacionPais.Descripcion
                                 }
                                    ).OrderBy(o => new { o.Pais, o.Estado, o.Municipio });

            return (new { municipios = municipios }).ToJson();
        }
    }
}