
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;
using System.Reflection;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class CelulaCategoria
    {
        static private List<RegistroBasico> registros;
        static public List<RegistroBasico> Obtener()
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name;
            registros = Cache.Instance.Obtener<List<RegistroBasico>>(key);
            if (registros == null)
            {
                registros = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaCategoria
                            orderby o.Descripcion
                            select new RegistroBasico
                            {
                                Id = o.Id,
                                Descripcion = o.Descripcion
                            }).ToList<RegistroBasico>();
                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }
    }
}
