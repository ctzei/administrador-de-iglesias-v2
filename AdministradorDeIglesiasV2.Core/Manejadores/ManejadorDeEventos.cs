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
    public class ManejadorDeEventos
    {
        public List<RegistroBasico> ObtenerEventosActivos()
        {
            return (
                from eventos in SesionActual.Instance.getContexto<IglesiaEntities>().Evento
                orderby eventos.Descripcion
                where 
                    eventos.Fecha_Fin <= DateTime.Now &&
                    eventos.Borrado == false
                select new RegistroBasico
                {
                    Id = eventos.Id,
                    Descripcion = eventos.Descripcion
                }).ToList<RegistroBasico>();
        }

        public List<Modelos.Retornos.FichaDeInscripcionDeEvento> ObtenerFichasDeInscripcion(int eventoId, bool cerradas, DateTime fechaInicial, DateTime fechaFinal)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().EventoFichaInscripcion
                    where
                        o.EventoId == eventoId &&
                        o.Fecha_Deposito >= fechaInicial &&
                        o.Fecha_Deposito <= fechaFinal &&
                        (cerradas == true || o.Cerrada == false)
                    select new Modelos.Retornos.FichaDeInscripcionDeEvento
                    {
                        Folio = o.Id,
                        Cerrada = o.Cerrada,
                        RefNum = o.Referencia_Numerica_Deposito,
                        RefAlfa = o.Referencia_Alfanumerica_Deposito,
                        Cantidad = o.CantidadPersonas,
                        Fecha = o.Fecha_Deposito,
                        Email = o.Email,
                        PrimerNombre = o.Primer_Nombre,
                        SegundoNombre = o.Segundo_Nombre,
                        ApellidoPaterno = o.Apellido_Paterno,
                        ApellidoMaterno = o.Apellido_Materno,
                        Tel = o.Tel,
                        Estado = o.UbicacionMunicipio.UbicacionEstado.Descripcion,
                        Municipio = o.UbicacionMunicipio.Descripcion,
                        Genero = o.Genero.Descripcion,
                        TipoDeRegistrante = o.EventoFichaInscripcionTipoRegistrante.Descripcion,
                        InfoExtraDeRegistrate = o.InfoExtraRegistrante
                    }).ToList<Modelos.Retornos.FichaDeInscripcionDeEvento>();
        }

        public bool GuardarFichasDeInscripcion(List<Dictionary<string, string>> fichas)
        {

            foreach (Dictionary<string, string> ficha in fichas)
            {
                string sFolio = ficha["Folio"];
                string sCerrada = ficha["Cerrada"];

                int folio;
                bool cerrada;

                if (bool.TryParse(sCerrada, out cerrada) && int.TryParse(sFolio, out folio)) //Si obtenemos un valor valido de falso/verdadero...
                {
                    EventoFichaInscripcion entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().EventoFichaInscripcion
                                                      where o.Id == folio
                                                      select o).SingleOrDefault();
                    entidad.Cerrada = cerrada;
                    entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }

            };

            return true;
        }
    }
}
