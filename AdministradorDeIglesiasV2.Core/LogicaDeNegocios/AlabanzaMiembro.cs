using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;
using System.Linq.Expressions;
using LinqKit;
using System.Globalization;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class AlabanzaMiembro
    {
        #region Busquedas

        public static List<AlabanzaMiembro> Buscar(string[] palabrasClave)
        {
            List<AlabanzaMiembro> resultado = new List<AlabanzaMiembro>();
            foreach (string keyword in palabrasClave)
            {
                var r = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro
                                   where
                                      (o.Miembro.Primer_Nombre.Contains(keyword) ||
                                       o.Miembro.Segundo_Nombre.Contains(keyword) ||
                                       o.Miembro.Apellido_Paterno.Contains(keyword) ||
                                       o.Miembro.Apellido_Materno.Contains(keyword) ||
                                       o.Miembro.Email.Contains(keyword)) &&
                                       o.Miembro.Borrado == false &&
                                       o.Borrado == false
                                    orderby
                                        o.Miembro.Primer_Nombre,
                                        o.Miembro.Segundo_Nombre,
                                        o.Miembro.Apellido_Paterno,
                                        o.Miembro.Apellido_Materno
                                   select o);

                resultado.AddRange(r);
            }

            resultado = resultado
                .GroupBy(i => i)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key).ToList<AlabanzaMiembro>();

            return resultado;
        }

        #endregion 
    }
}
