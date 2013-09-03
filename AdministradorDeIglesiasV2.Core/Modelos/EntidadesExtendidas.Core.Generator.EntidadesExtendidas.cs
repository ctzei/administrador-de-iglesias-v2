

//------------------------------------------------------------------------------
// Esta clase a sido autogenerada, no se debe de modificar o se podran perder
// los cambios
// Creada por: Zague
//------------------------------------------------------------------------------

using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    
    public partial class IglesiaEntities : ObjectContext
    {
    	public int SaveChangesDirectly(SaveOptions options)
        {
    		return base.SaveChanges(options);
    	}
    
        public override int SaveChanges(SaveOptions options)
        {
            //Buscamos todas las entidades que sean IEntidadModificable y que hayan sufrido cambios
            IEnumerable<ObjectStateEntry> entidadesConCambios = obtenerEntidadesConCambios();
            IEnumerable<ObjectStateEntry> entidadesYaProcesadas = new List<ObjectStateEntry>();
    
            //Solo procesamos si las entidades en verdad sufireron algun cambio, de lo contrario es inecesario volver entrar a este ciclo
            while (entidadesConCambios.Count() > 0)
            {
                foreach (var entry in entidadesConCambios)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            agregarEntidad(entry);
                            break;
                        case EntityState.Modified:
                            modificarEntidad(entry);
                            break;
                        case EntityState.Deleted:
                            borrarEntidad(entry);
                            break;
                    }
                }
                entidadesYaProcesadas = entidadesYaProcesadas.Concat(entidadesConCambios);
                entidadesConCambios = obtenerEntidadesConCambios().Except(entidadesYaProcesadas);
            } 
    		
            int rtn = -1;
            try
            {
                rtn = base.SaveChanges(options);
            }
            catch (System.Data.UpdateException ex){
                if (ex.InnerException is SqlException)
                {
                    string comentarioTecnicoKey = "ComentarioTecnico";
    				int errorCode = (((SqlException)ex.InnerException).Number);
                    switch (errorCode) 
                    {
    				    case 242:
                            ex.Data.Add(comentarioTecnicoKey, string.Format("El registro contiene algun campo de fecha vacio (NULL) y esto se traduce a una fecha no permitida. [ErrorCode:{0}]", errorCode));
                            throw new ExcepcionReglaNegocio("Alguno de los campos del registro está marcado como fecha y su valor no se encuentra dentro del rango permitido. Favor de volver a intentar.", ex);
        				case 515:
                            ex.Data.Add(comentarioTecnicoKey, string.Format("El registro contiene algun campo requerido, vacio (NULL). [ErrorCode:{0}]", errorCode));
                            throw new ExcepcionReglaNegocio("Alguno de los campos del registro está marcado como requerido y no se esta cumpliendo con esa regla. Favor de volver a intentar.", ex);
                        case 547:
                            ex.Data.Add(comentarioTecnicoKey, string.Format("El registro contiene una referencia a un registro inexistente. Revisar las FKs. [ErrorCode:{0}]", errorCode));
                            throw new ExcepcionReglaNegocio("Alguno de los campos del registro hace referencia a otro registro y este último es inexistente o inválido. Favor de volver a intentar.", ex);
                        case 2627:
                            ex.Data.Add(comentarioTecnicoKey, string.Format("El sistema esta intentado introducir un registro con un valor en una tabla que debera de ser unico. Revisar que la tabla tenga definida la columna Id como autogenerable y/o que los campos marcados como UNIQUE KEYS en verdad tengan valores unicos. [ErrorCode:{0}]", errorCode));
                            throw new ExcepcionReglaNegocio("Alguno de los campos del registro está marcado como único y no se esta cumpliendo con esa regla. Favor de volver a intentar.", ex);
                        default:
                            throw new ExcepcionAplicacion(string.Format("Error desconocido al guardar los cambios. [ErrorCode:{0}]", errorCode), ex);
                    }
                }
            }
    	    catch (System.StackOverflowException ex){
                throw new ExcepcionAplicacion("Se produjo in ciclo infinito; es necesario asegurarse que en las clases de logica de negocios, en los eventos onAgregar, onBorrar y onModificar NO se este llamando el metodo de SaveChanges() ni los metodos propios de las entidades que lo llamen (Borrar(), Guardar()).", ex);
            }
    		
            return rtn;
        }
    	
    #region Funciones Privadas
    
    		///Este metodo obtiene todas las entidades agregadas, borradas o modificadas del contexto actual
    		private IEnumerable<ObjectStateEntry> obtenerEntidadesConCambios()
    		{
                return (from e in ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Modified | EntityState.Deleted)
                        where
    					  e.State != EntityState.Detached &&
                          e.IsRelationship == false &&
                          e.Entity != null &&
                          e.Entity is IEntidadModificable
                        select e);
    		}
    
            private void agregarEntidad(ObjectStateEntry entry)
            {
                ((IEntidadRegistrable)entry.Entity).Modificacion = System.DateTime.Now;
                ((IEntidadRegistrable)entry.Entity).Creacion = System.DateTime.Now;
    			((IEntidadRegistrable)entry.Entity).ModificacionId = SesionActual.Instance.UsuarioId;
    			((IEntidadRegistrable)entry.Entity).CreacionId = SesionActual.Instance.UsuarioId;
                if (entry.Entity is IEntidadBorrable) { ((IEntidadBorrable)entry.Entity).Borrado = false; }
                ((IEntidadModificable)entry.Entity).OnAgregarBase(entry);
            }
    
            private void modificarEntidad(ObjectStateEntry entry)
            {
                ((IEntidadRegistrable)entry.Entity).Modificacion = System.DateTime.Now;
    			((IEntidadRegistrable)entry.Entity).ModificacionId = SesionActual.Instance.UsuarioId;
                if (entry.Entity is IEntidadBorrable) { ((IEntidadBorrable)entry.Entity).Borrado = false; }
                ((IEntidadModificable)entry.Entity).OnModificarBase(entry);
            }
    
            private void borrarEntidad(ObjectStateEntry entry)
            {
                if (entry.Entity is IEntidadBorrable)
                {
                    entry.ChangeState(EntityState.Modified);
                    ((IEntidadRegistrable)entry.Entity).Modificacion = System.DateTime.Now;
    				((IEntidadRegistrable)entry.Entity).ModificacionId = SesionActual.Instance.UsuarioId;
                    ((IEntidadBorrable)entry.Entity).Borrado = true;
                }
                ((IEntidadModificable)entry.Entity).OnBorrarBase(entry);
            }
    
    #endregion
    
    }
    
    #region Interfaces
    
    public interface IEntidadModificable
    {
    	//Eventos
    	void OnAgregarBase(ObjectStateEntry entry);
    	void OnModificarBase(ObjectStateEntry entry);
    	void OnBorrarBase(ObjectStateEntry entry);
    	
        //Metodos
        void Borrar(ObjectContext contexto);
        void Guardar(ObjectContext contexto);
    }
    
    public interface IEntidadRegistrable
    {
    	//Propiedades
    	System.DateTime Creacion { get; set; }
    	System.DateTime Modificacion { get; set; }
    	int CreacionId { get; set; }
    	int ModificacionId { get; set; }
    }
    
    public interface IEntidadBorrable
    {
    	//Propiedades
    	bool Borrado { get; set; }
    }
    
    #endregion
    	
}

