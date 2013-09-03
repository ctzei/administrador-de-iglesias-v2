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
    public partial class AlabanzaMiembroDiaNoDisponible
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
            AlabanzaMiembroDiaNoDisponible entidad = (AlabanzaMiembroDiaNoDisponible)entry.Entity;
            validarFechas(entidad);
            validarEnsayosYEventos(entidad);
        }

        private void validarFechas(AlabanzaMiembroDiaNoDisponible entidad)
        {
            if (entidad.FechaInicio > entidad.FechaFin)
            {
                throw new ExcepcionReglaNegocio(Literales.FechasConOrdenCronologicoIncorrecto);
            }
        }

        private void validarEnsayosYEventos(AlabanzaMiembroDiaNoDisponible entidad)
        {

            bool tieneEventos = SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoMiembro.Any(o =>
                o.AlabanzaEvento.Fecha >= entidad.FechaInicio &&
                o.AlabanzaEvento.Fecha <= entidad.FechaFin &&
                o.AlabanzaMiembroId == entidad.AlabanzaMiembroId &&
                o.Borrado == false &&
                o.AlabanzaEvento.Borrado == false);

            bool tieneEnsayos = SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayoMiembro.Any(o =>
               o.AlabanzaEnsayo.Fecha >= entidad.FechaInicio &&
               o.AlabanzaEnsayo.Fecha <= entidad.FechaFin &&
               o.AlabanzaMiembroId == entidad.AlabanzaMiembroId &&
               o.AlabanzaEnsayo.Borrado == false);

            if (tieneEventos || tieneEnsayos)
            {
                throw new ExcepcionReglaNegocio("El usuario actual ya tiene reservados esos dias para algun ensayo/evento. Es necesario primero ser eliminado de tales antes de continuar.");
            }
        }

        #endregion
    }
}
