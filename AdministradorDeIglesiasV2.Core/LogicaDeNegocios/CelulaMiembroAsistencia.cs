
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;
using System.Reflection;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class CelulaMiembroAsistencia
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.AgregarAsistencias);
            validarEntidad(entry);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarBorrarAsistencias);
            validarEntidad(entry);
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarBorrarAsistencias);
        }

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            CelulaMiembroAsistencia entidad = (CelulaMiembroAsistencia)entry.Entity;
            cerrarBoleta(entidad);
        }

        private void cerrarBoleta(CelulaMiembroAsistencia entidad)
        {
            IglesiaEntities contexto = new IglesiaEntities();
            ConsolidacionBoleta boleta = (from o in contexto.ConsolidacionBoleta where o.Email == entidad.Miembro.Email select o).SingleOrDefault();
            if (boleta != null && !ConsolidacionBoleta.Estatus.Cerrada(boleta.BoletaEstatusId))
            {
                boleta.BoletaEstatusId = ConsolidacionBoleta.Estatus.ASISTE_A_CELULA.Key;
                contexto.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave, true);
            }
        }
    }
}
