
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;
using System.Reflection;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class MiembroPaso
    {
        static private List<RegistroBasicoExt> registros;
        static public List<RegistroBasicoExt> Obtener()
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name;
            registros = Cache.Instance.Obtener<List<RegistroBasicoExt>>(key);
            if (registros == null)
            {
                registros = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Paso
                         orderby o.PasoCategoria.Descripcion
                         select new RegistroBasicoExt
                        {
                            Id = o.PasoId,
                            Descripcion = o.Descripcion,
                            PadreId = o.PasoCategoria.PasoCategoriaId
                        }
                ).ToList<RegistroBasicoExt>();
                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }
    }
}
