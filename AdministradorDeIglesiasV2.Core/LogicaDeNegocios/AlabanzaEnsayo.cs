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
    public partial class AlabanzaEnsayo
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
            AlabanzaEnsayo entidad = (AlabanzaEnsayo)entry.Entity;
            validarAsistenciaSegunFecha(entidad);
            validarHoras(entidad);
        }

        private void validarAsistenciaSegunFecha(AlabanzaEnsayo entidad)
        {
            if (entidad.AlabanzaEvento.Fecha < entidad.Fecha)
            {
                throw new ExcepcionReglaNegocio(Literales.FechaDelEnsayoPosteriorAlEvento);
            }
        }

        private void validarHoras(AlabanzaEnsayo entidad)
        {
            if (entidad.HoraDiaInicioId >= entidad.HoraDiaFinId)
            {
                throw new ExcepcionReglaNegocio(Literales.HorasConOrdenCronologicoIncorrecto);
            }
        }

        #endregion

    }
}
