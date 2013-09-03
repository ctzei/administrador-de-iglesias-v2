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
    
    public partial class Genero : IEntidadModificable , IEntidadRegistrable
    {
    #region Eventos
    
        public void OnAgregarBase(ObjectStateEntry entry)
        {
            OnAgregar(entry);
        }
        partial void OnAgregar(ObjectStateEntry entry);
    	
    	public void OnModificarBase(ObjectStateEntry entry)
        {
            OnModificar(entry);
        }
        partial void OnModificar(ObjectStateEntry entry);
    	
    	public void OnBorrarBase(ObjectStateEntry entry)
    	{
    		OnBorrar(entry);
    	}
    	partial void OnBorrar(ObjectStateEntry entry);
    	
    #endregion
    
    #region Metodos
    
    	public void Borrar(ObjectContext contexto)
        {
            this.Borrar(contexto, System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }
    		
        public void Borrar(ObjectContext contexto, SaveOptions saveOptions)
        {
            contexto.DeleteObject(this);
            contexto.SaveChanges(saveOptions);
        }
    
    	public void Guardar(ObjectContext contexto)
        {
            this.Guardar(contexto, System.Data.Objects.SaveOptions.AcceptAllChangesAfterSave);
        }
    		
        public void Guardar(ObjectContext contexto, SaveOptions saveOptions)
        {
            if (this.EntityKey == null)
            {
                contexto.AddObject(this.GetType().Name, this);
            }
            contexto.SaveChanges(saveOptions);
        }
    		
    #endregion
    }
}
