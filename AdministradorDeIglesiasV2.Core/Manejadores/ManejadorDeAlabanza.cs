using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeAlabanza
    {
        #region Busquedas

        public IEnumerable<AlabanzaEvento> BuscarEventos(int eventoId, string descripcion, DateTime fecha, List<int> horasInicio, List<int> horasFin)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEvento
                    where
                    ((o.Id == (eventoId > 0 ? eventoId : o.Id)) &&
                    ((o.Descripcion.Contains(descripcion)) || (o.Descripcion == null)) &&
                    (o.Fecha == (fecha > DateTime.MinValue ? fecha : o.Fecha)) &&
                    (horasInicio.Contains(o.HoraDiaInicioId) || (horasInicio.Count == 0)) &&
                    (horasFin.Contains(o.HoraDiaFinId) || (horasFin.Count == 0)) &&
                    (o.Borrado == false))
                    select o);
        }

        public IEnumerable<AlabanzaCancion> BuscarCanciones(string titulo, string artista, string disco, string tono)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion
                    where
                        (o.Titulo.Contains(titulo) || string.IsNullOrEmpty(titulo)) &&
                        (o.Artista.Contains(artista) || string.IsNullOrEmpty(artista)) &&
                        (o.Disco.Contains(disco) || string.IsNullOrEmpty(disco)) &&
                        (o.Tono.Contains(tono) || string.IsNullOrEmpty(tono)) &&
                        (o.Borrado == false)
                    select o);
        }

        public AlabanzaCancion ObtenerCancion(int cancionId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaCancion
                    where
                        (o.Id == cancionId) &&
                        (o.Borrado == false)
                    select o).SingleOrDefault();
        }

        #endregion

        #region Funciones por Evento

        public IEnumerable<AlabanzaEventoMiembro> ObtenerMiembrosPorEvento(int eventoId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoMiembro
                    where
                        (o.AlabanzaEventoId == eventoId) &&
                        (o.Borrado == false) &&
                        (o.AlabanzaMiembro.Borrado == false) &&
                        (o.AlabanzaEvento.Borrado == false)
                    orderby o.AlabanzaMiembro.Miembro.Primer_Nombre, o.AlabanzaMiembro.Miembro.Segundo_Nombre, o.AlabanzaMiembro.Miembro.Apellido_Paterno, o.AlabanzaMiembro.Miembro.Apellido_Materno, o.AlabanzaMiembro.Miembro.Email
                    select o);
        }

        public IEnumerable<AlabanzaEventoCancion> ObtenerCancionesPorEvento(int eventoId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoCancion
                    where
                        (o.AlabanzaEventoId == eventoId) &&
                        (o.Borrado == false) &&
                        (o.AlabanzaCancion.Borrado == false) &&
                        (o.AlabanzaEvento.Borrado == false)
                    select o);
        }

        public IEnumerable<AlabanzaEnsayo> ObtenerEnsayosPorEvento(int eventoId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayo
                    where
                        (o.AlabanzaEventoId == eventoId) &&
                        (o.Borrado == false) &&
                        (o.AlabanzaEvento.Borrado == false)
                    select o);
        }

        #endregion

        #region Funciones por Ensayo

        public IEnumerable<AlabanzaEnsayoMiembro> ObtenerMiembrosPorEnsayo(int ensayoId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayoMiembro
                    where
                        (o.AlabanzaEnsayoId == ensayoId) &&
                        (o.AlabanzaMiembro.Borrado == false) &&
                        (o.AlabanzaEnsayo.Borrado == false) &&
                        (o.AlabanzaEnsayo.AlabanzaEvento.Borrado == false)
                    orderby o.AlabanzaMiembro.Miembro.Primer_Nombre, o.AlabanzaMiembro.Miembro.Segundo_Nombre, o.AlabanzaMiembro.Miembro.Apellido_Paterno, o.AlabanzaMiembro.Miembro.Apellido_Materno, o.AlabanzaMiembro.Miembro.Email
                    select o);
        }

        public IEnumerable<AlabanzaEventoCancion> ObtenerCancionesPorEnsayo(int ensayoId)
        {
            AlabanzaEvento evento = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayo where o.Id == ensayoId select o.AlabanzaEvento).SingleOrDefault();
            return this.ObtenerCancionesPorEvento(evento.Id);
        }

        #endregion

        #region Funciones por Miembro

        public IEnumerable<AlabanzaMiembroDiaNoDisponible> ObtenerDiasNoDisponiblesPorMiembro(int miembroId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembroDiaNoDisponible
                    where
                        o.AlabanzaMiembro.MiembroId == miembroId &&
                        o.AlabanzaMiembro.Miembro.Borrado == false &&
                        o.Borrado == false
                    orderby o.FechaInicio descending
                    select o);
        }

        public IEnumerable<AlabanzaEvento> ObtenerEventosPorMiembro(int miembroId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoMiembro
                    where
                        o.AlabanzaMiembro.MiembroId == miembroId &&
                        o.AlabanzaMiembro.Miembro.Borrado == false &&
                        o.AlabanzaEvento.Borrado == false &&
                        o.AlabanzaMiembro.Borrado == false
                    orderby o.AlabanzaEvento.Fecha descending, o.AlabanzaEvento.HoraDiaInicioId descending
                    select o.AlabanzaEvento);
        }

        public IEnumerable<AlabanzaEnsayo> ObtenerEnsayosPorMiembro(int miembroId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayoMiembro
                    where
                        o.AlabanzaMiembro.MiembroId == miembroId &&
                        o.AlabanzaMiembro.Miembro.Borrado == false &&
                        o.AlabanzaEnsayo.Borrado == false &&
                        o.AlabanzaMiembro.Borrado == false
                    orderby o.AlabanzaEnsayo.Fecha descending, o.AlabanzaEnsayo.HoraDiaInicioId descending
                    select o.AlabanzaEnsayo);
        }


        #endregion
    }
}
