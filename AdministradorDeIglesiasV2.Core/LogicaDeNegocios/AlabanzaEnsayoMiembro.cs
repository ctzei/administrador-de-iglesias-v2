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
    public partial class AlabanzaEnsayoMiembro
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            validarEntidad(entry);
            
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            validarEntidad(entry);
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
          
        }

        #region Validacion de Reglas de Negocio

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            AlabanzaEnsayoMiembro entidad = (AlabanzaEnsayoMiembro)entry.Entity;
            establecerAsistenciaSegunRetraso(entidad);
            establecerValoresIniciales(entry);
        }

        private void establecerAsistenciaSegunRetraso(AlabanzaEnsayoMiembro entidad)
        {
            entidad.Asistencia = entidad.Asistencia || entidad.Retraso;
        }

        private void establecerValoresIniciales(System.Data.Objects.ObjectStateEntry entry)
        {
            AlabanzaEnsayoMiembro entidad = (AlabanzaEnsayoMiembro)entry.Entity;
            if (entry.State == System.Data.EntityState.Added)
            {
                entidad.Asistencia = false;
                entidad.Retraso = false;
            }
        }

        #endregion

    }
}
