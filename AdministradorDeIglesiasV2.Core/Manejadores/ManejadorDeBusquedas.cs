using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;
using ZagueEF.Core;
using LinqKit;
using System.Data.Objects.DataClasses;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeBusquedas
    {

        public IEnumerable<object> Buscar(TipoDeObjeto tipoDeObjeto, string[] conceptosABuscar)
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();
            int usuarioId = SesionActual.Instance.UsuarioId;
            List<int> idsCelulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(usuarioId);
            List<int> idsCelulasSinLider = manejadorCelulas.ObtenerCelulasSinLideresComoCelulas().Select(o => o.CelulaId).ToList<int>();

            switch (tipoDeObjeto)
            {
                case TipoDeObjeto.Celula:
                    {
                        return (Celula.Buscar(conceptosABuscar, idsCelulasPermitidas.Union(idsCelulasSinLider).ToList<int>()).Select(o => new {
                            Id = o.CelulaId,
                            Descripcion = o.Descripcion,
                            RowColor = idsCelulasSinLider.Contains(o.CelulaId) ? "red" : string.Empty
                        }));
                    }

                case TipoDeObjeto.Miembro:
                    {
                        return (Miembro.Buscar(conceptosABuscar, idsCelulasPermitidas.ToList<int>()).Select(o => new
                        {
                            Id = o.MiembroId,
                            Descripcion = o.Primer_Nombre + " " + o.Segundo_Nombre + " " + o.Apellido_Paterno + " " + o.Apellido_Materno + " (" + o.Email + ")"
                        }));
                    }

                case TipoDeObjeto.AlabanzaMiembro: 
                    {
                        return (AlabanzaMiembro.Buscar(conceptosABuscar).Select(o => new
                        {
                            Id = o.Id,
                            Descripcion = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno + " (" + o.Miembro.Email + ")"
                        }));
                    }

                default:{
                    return new List<EntityObject>();
                }
            }
        }
        
        public enum TipoDeObjeto
        {
            Miembro = 0,
            Celula = 1,
            AlabanzaMiembro = 2
        }
    }
}
