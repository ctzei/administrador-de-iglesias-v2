using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZagueEF.Core;

namespace ZagueEF.Core.Web
{
    public class PaginaDeDetalle : PaginaBase
    {
        /// <summary>
        /// Este metodo sirve para obtener el parametro "Id" del querystring
        /// </summary>
        public int ObtenerId()
        {
            int id;
            string idKey = "id";

            if (!int.TryParse(Request[idKey], out id))
            {
                throw new ExcepcionReglaNegocio(string.Format("Es necesario establecer el parametro [{0}] y que su valor sea numerico.", idKey));
            }
            else if (id <= 0)
            {
                throw new ExcepcionReglaNegocio(string.Format("Es necesario establecer el parametro [{0}] con un valor mayor a cero.", idKey));
            }

            return id;
        }
    }
}
