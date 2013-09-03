using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class CelulaLider
    {
        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            validarEntidad(entry);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            validarEntidad(entry);
        }

        #region Validacion de Reglas de Negocio

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            CelulaLider entidad = (CelulaLider)entry.Entity;
            validarLiderzagoDeMiembro(entidad);
        }

        private void validarLiderzagoDeMiembro(CelulaLider entidad)
        {
            //Validamos que el usuario no "asista" a la celula que se le esta asignando como lider
            if (entidad.Miembro.CelulaId == entidad.CelulaId)
            {
                throw new ExcepcionReglaNegocio(Literales.MiembroNoPuedeSerLiderDeCelulaQueAsiste);
            }

            //Validamos que no existan lideres de una misma celula que no asistan a la misma celula
            if (entidad.Celula.CelulaLider.Any(o => 
                o.Borrado == false && 
                o.Miembro.Borrado == false &&
                o.Miembro.CelulaId != entidad.Miembro.CelulaId && 
                o.MiembroId != entidad.MiembroId))
            {
                throw new ExcepcionReglaNegocio(Literales.LideresEnDistintasCelulas);
            }
        }

        #endregion

    }
}
