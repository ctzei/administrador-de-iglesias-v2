using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class DiaSemana
    {
        static private List<RegistroBasico> registros;
        static public List<RegistroBasico> Obtener()
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name;
            registros = Cache.Instance.Obtener<List<RegistroBasico>>(key);
            if (registros == null)
            {
                registros = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().DiaSemana
                         orderby o.DiaSemanaId
                         select new RegistroBasico
                         {
                             Id = o.DiaSemanaId,
                             Descripcion = o.Descripcion
                         }).ToList<RegistroBasico>();
                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }

        public DayOfWeek DayOfWeek { 
            get{
                if (this.DiaSemanaId <= 6)
                {
                    return (System.DayOfWeek)this.DiaSemanaId;
                }
                else
                {
                    return System.DayOfWeek.Sunday;
                }
            }
        }
    }
}
