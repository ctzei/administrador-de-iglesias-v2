
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class FoliGrupo
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.CrearGruposDeFoli);
            validarEntidad(entry);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarGruposDeFoli);
            validarEntidad(entry);
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.BorrarGruposDeFoli);
        }

        #region Validacion de Reglas de Negocio

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            FoliGrupo entidad = (FoliGrupo)entry.Entity;
            validarFechas(entidad);
        }

        private void validarFechas(FoliGrupo entidad)
        {
            //Validamos que las fechas de los modulos y la fecha final vaya en orden; no puede iniciar un modulo ANTES que el siguiente ni la fecha final puede ser ANTES del inicio de alguno de los modulos
            if (!((entidad.Fecha_Inicio_Modulo1 < entidad.Fecha_Inicio_Modulo2) && (entidad.Fecha_Inicio_Modulo2 < entidad.Fecha_Inicio_Modulo3) && (entidad.Fecha_Inicio_Modulo3 < entidad.Fecha_Inicio_Modulo4) && (entidad.Fecha_Inicio_Modulo4 < entidad.Fecha_Fin)))
            {
                throw new ExcepcionReglaNegocio(Literales.FechasDeGrupoNoValidas);
            }
        }

        #endregion
    }
}
