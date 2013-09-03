using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;
using System.Linq.Expressions;
using LinqKit;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class Celula
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.CrearCelulas);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarCelulas); 
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.BorrarCelulas);
        }

        #region Busquedas

        #region Deprecated
        /// <summary>
        /// Busca todas las células disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <param name="margenDeCelulas">Es una lista de ids de celulas dentro de las que se quiere buscar, si es "null" busca en TODAS las celulas disponibles. Sirve para limitar el numero de celulas donde se desea buscar.</param>
        /// <returns></returns>
        [Obsolete("Reemplazada por la funcion que regresa una lista", false)]
        public static Expression<Func<Celula, bool>> BuscarV1(string[] palabrasClave, List<int> margenDeCelulas)
        {
            var predicate = PredicateBuilder.False<Celula>();

            predicate = predicate.Or(o => palabrasClave.Count() <= 0); //TODA las celulas si no se definen "palabras clave"
            foreach (string keyword in palabrasClave)
            {
                string temp = keyword;
                predicate = predicate.Or(o => o.Descripcion.Contains(temp));
                predicate = predicate.Or(o => o.CelulaCategoria.Descripcion.Contains(temp));
                predicate = predicate.Or(o => o.Direccion.Contains(temp));
                predicate = predicate.Or(o => o.DiaSemana.Descripcion.Contains(temp));
                predicate = predicate.Or(o => o.HoraDia.Descripcion.Contains(temp));
            }

            if (margenDeCelulas != null)
            {
                predicate = predicate.And(o => margenDeCelulas.Contains(o.CelulaId)); //Buscamos solo ENTRE el margen de celulas definido
            }

            predicate = predicate.And(o => o.Borrado == false); //SOLO los registros NO eliminados
            return predicate;
        }

        /// <summary>
        /// Busca todas las células disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <returns></returns>
        [Obsolete("Reemplazada por la funcion que regresa una lista", false)]
        public static Expression<Func<Celula, bool>> BuscarV1(string[] palabrasClave)
        {
            return BuscarV1(palabrasClave, null);
        }
        #endregion

        /// <summary>
        /// Busca todos los miembros disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <param name="margenDeCelulas">Es una lista de ids de celulas dentro de las que se quiere buscar, si es "null" busca en TODAS las celulas disponibles. Sirve para limitar el numero de celulas donde se desea buscar.</param>
        /// <returns></returns>
        public static List<Celula> Buscar(string[] palabrasClave, List<int> margenDeCelulas)
        {
            List<Celula> resultado = new List<Celula>();
            foreach (string keyword in palabrasClave)
            {
                var r = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                         where
                            (o.Descripcion.Contains(keyword) ||
                             o.CelulaCategoria.Descripcion.Contains(keyword) ||
                             o.Direccion.Contains(keyword) ||
                             o.DiaSemana.Descripcion.Contains(keyword) ||
                             o.HoraDia.Descripcion.Contains(keyword)) &&
                             o.Borrado == false
                         select o);

                if (margenDeCelulas != null)
                {
                    r = r.Where(o => margenDeCelulas.Contains(o.CelulaId));
                }

                resultado.AddRange(r);
            }

            resultado = resultado
                .GroupBy(i => i)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key).ToList<Celula>();

            return resultado;
        }

        /// <summary>
        /// Busca todos los miembros disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <returns></returns>
        public static List<Celula> Buscar(string[] palabrasClave)
        {
            return Buscar(palabrasClave, null);
        }


        #endregion 
    }
}
