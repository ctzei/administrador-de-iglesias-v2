
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;
using System.Reflection;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class HoraDia
    {
        static private List<RegistroBasico> registros;
        static public List<RegistroBasico> Obtener()
        {
            if (registros == null)
            {
                string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name;
                registros = Cache.Instance.Obtener<List<RegistroBasico>>(key);
                registros = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().HoraDia
                            orderby o.HoraDiaId
                            select new RegistroBasico
                            {
                                Id = o.HoraDiaId,
                                Descripcion = o.Descripcion
                            }).ToList<RegistroBasico>();
                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }
    }
}
