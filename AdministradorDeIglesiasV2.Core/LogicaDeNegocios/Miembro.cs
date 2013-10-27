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
    public partial class Miembro
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.CrearMiembros);
            validarEntidad(entry);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarMiembros);
            validarEntidad(entry);
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.BorrarMiembros);
        }

        public string NombreCompleto { get { return this.Primer_Nombre + " " + this.Segundo_Nombre + " " + this.Apellido_Paterno + " " + this.Apellido_Materno + " (" + this.Email + ")"; } }

        #region Validacion de Reglas de Negocio

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            Miembro entidad = (Miembro)entry.Entity;
            Validaciones.ValidarEmail(entidad.Email);
            validarConyuge(entidad);
            validarTelefonos(entidad);
            validarPassword(entidad);
            validarLiderzagoDeMiembro(entidad);
            validarUnicidad(entry);
        }

        private void validarUnicidad(System.Data.Objects.ObjectStateEntry entry)
        {
            Miembro entidadActual = ((Miembro)entry.Entity);

            // Validamos si no existe ya un miembro con el mismo correo
            Miembro entidadPreexistente = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.Email == entidadActual.Email select o).SingleOrDefault();
                  
            if (entidadPreexistente != null)
            {
                string registroExistenteMsg = string.Format("Ya existe algún miembro registrado con ese email [{0}] cuyo Id es [{1}] y el cual se encuentra asignado a la célula [{2}] [{3}].", entidadPreexistente.Email, entidadPreexistente.MiembroId, entidadPreexistente.Celula.Descripcion, entidadPreexistente.CelulaId);
              
                if (entry.State == System.Data.EntityState.Added)
                {
                    if (entidadPreexistente.Borrado == false)
                    {
                        throw new ExcepcionReglaNegocio(registroExistenteMsg);
                    }
                    else
                    {
                        // Modificamos la entidad BORRADA con los nuevos datos, en vez de crear una nueva
                        entidadActual.MiembroId = entidadPreexistente.MiembroId;
                        SesionActual.Instance.getContexto<IglesiaEntities>().Miembro.Detach(entidadActual);
                        SesionActual.Instance.getContexto<IglesiaEntities>().ObjectStateManager.ChangeObjectState(entidadActual, System.Data.EntityState.Detached);
                        System.Data.Objects.ObjectStateEntry state = SesionActual.Instance.getContexto<IglesiaEntities>().ObjectStateManager.GetObjectStateEntry(entidadPreexistente);
                        state.ApplyCurrentValues(entidadActual);
                        state.ChangeState(System.Data.EntityState.Modified);
                    }
                }
                else if (entry.State == System.Data.EntityState.Modified)
                {
                    if (Validaciones.ValidarCambiosEnCampo(entry, "email"))
                    {
                        throw new ExcepcionReglaNegocio(registroExistenteMsg);
                    }
                }
            }
        }

        private void validarConyuge(Miembro entidad)
        {
            if (entidad.ConyugeId != null)
            {
                Miembro conyuge = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == entidad.ConyugeId.Value && o.Borrado == false select o).SingleOrDefault();
                if (conyuge != null && (conyuge.ConyugeId == null || conyuge.ConyugeId.Value != entidad.MiembroId))
                {
                    conyuge.ConyugeId = entidad.MiembroId;
                }
            }
        }

        private void validarTelefonos(Miembro entidad)
        {
            if ((entidad.Tel_Casa.Trim().Length <= 0) && (entidad.Tel_Movil.Trim().Length <= 0) && (entidad.Tel_Trabajo.Trim().Length <= 0))
            {
                throw new ExcepcionReglaNegocio(Literales.MinimoUnTelefonoRequerido);
            }
        }

        private void validarPassword(Miembro entidad)
        {
            int minLongitudPwd = 6;
            if (entidad.Contrasena != null)
            {
                if ((entidad.Contrasena.Trim().Length > 0) && (entidad.Contrasena.Trim().Length < minLongitudPwd))
                {
                    throw new ExcepcionReglaNegocio(String.Format("La contraseña no cumple con el mínimo requerido de {0} caracteres.", minLongitudPwd));
                }

                if (entidad.Contrasena.Equals(entidad.Email, StringComparison.OrdinalIgnoreCase))
                {
                    throw new ExcepcionReglaNegocio("La contraseña no puede ser lo mismo que el email.");
                }
            }
            else
            {
                entidad.Contrasena = string.Empty;
            }
        }

        private void validarLiderzagoDeMiembro(Miembro entidad)
        {
            //Validamos que el usuario NO sea lider de la celula a la que esta asignado
            if (entidad.CelulaLider.Any(o => o.Borrado == false && o.CelulaId == entidad.CelulaId))
            {
                throw new ExcepcionReglaNegocio(Literales.MiembroNoPuedeSerLiderDeCelulaQueAsiste);
            }

            //Validamos que no existan lideres de una misma celula que no asistan a la misma celula
            List<int> celulasQueEsLider = (from o in entidad.CelulaLider where o.Borrado == false select o.CelulaId).ToList<int>();
            if (SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider.Any(o =>
                celulasQueEsLider.Contains(o.CelulaId) &&
                o.Borrado == false &&
                o.Miembro.Borrado == false &&
                o.Miembro.CelulaId != entidad.CelulaId &&
                o.MiembroId != entidad.MiembroId
                ))
            {
                throw new ExcepcionReglaNegocio(Literales.LideresEnDistintasCelulas);
            }
        }

        #endregion

        #region Busquedas

        #region Deprecated
        /// <summary>
        /// Busca todos los miembros disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <param name="margenDeCelulas">Es una lista de ids de celulas dentro de las que se quiere buscar, si es "null" busca en TODAS las celulas disponibles. Sirve para limitar el numero de celulas donde se desea buscar.</param>
        /// <returns></returns>
        [Obsolete("Reemplazada por la funcion que regresa una lista", false)]
        public static Expression<Func<Miembro, bool>> BuscarV1(string[] palabrasClave, List<int> margenDeCelulas)
        {
            var predicate = PredicateBuilder.False<Miembro>();

            predicate = predicate.Or(o => palabrasClave.Count() <= 0); //TODA las celulas si no se definen "palabras clave"
            foreach (string keyword in palabrasClave)
            {
                string temp = keyword;
                predicate = predicate.Or(o => o.Primer_Nombre.Contains(temp));
                predicate = predicate.Or(o => o.Segundo_Nombre.Contains(temp));
                predicate = predicate.Or(o => o.Apellido_Paterno.Contains(temp));
                predicate = predicate.Or(o => o.Apellido_Materno.Contains(temp));
                predicate = predicate.Or(o => o.Email.Contains(temp));
                predicate = predicate.Or(o => o.EstadoCivil.Descripcion.Contains(temp));
                //predicate = predicate.Or(o => o.Celula.Descripcion.Contains(temp));
                predicate = predicate.Or(o => o.UbicacionMunicipio.Descripcion.Contains(temp));
                predicate = predicate.Or(o => o.Colonia.Contains(temp));
                predicate = predicate.Or(o => o.Direccion.Contains(temp));
            }

            if (margenDeCelulas != null)
            {
                predicate = predicate.And(o => margenDeCelulas.Contains(o.CelulaId)); //Buscamos solo ENTRE el margen de celulas definido
            }

            predicate = predicate.And(o => o.Borrado == false); //SOLO los registros NO eliminados
            return predicate;
        }

        /// <summary>
        /// Busca todos los miembros disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <returns></returns>
        [Obsolete("Reemplazada por la funcion que regresa una lista", false)]
        public static Expression<Func<Miembro, bool>> BuscarV1(string[] palabrasClave)
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
        public static List<Miembro> Buscar(string[] palabrasClave, List<int> margenDeCelulas)
        {
            List<Miembro> resultado = new List<Miembro>();
            foreach (string keyword in palabrasClave)
            {
                var r = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro
                                   where
                                      (o.Primer_Nombre.Contains(keyword) ||
                                       o.Segundo_Nombre.Contains(keyword) ||
                                       o.Apellido_Paterno.Contains(keyword) ||
                                       o.Apellido_Materno.Contains(keyword) ||
                                       o.Email.Contains(keyword)) &&
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
                .Select(g => g.Key).ToList<Miembro>();

            return resultado;
        }

        /// <summary>
        /// Busca todos los miembros disponibles en el sistema, usando cada una de las palabras clave en todos los campos disponibles
        /// </summary>
        /// <param name="palabrasClave">Palabras claves usadas para buscar</param>
        /// <returns></returns>
        public static List<Miembro> Buscar(string[] palabrasClave)
        {
            return Buscar(palabrasClave, null);
        }

        #endregion 
    }
}
