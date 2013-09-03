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
    public class ManejadorDeConsolidacion
    {

        public Miembro CrearMiembroDesdeBoleta(int boletaId, int celulaId)
        {
            ConsolidacionBoleta boleta = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Id == boletaId select o).SingleOrDefault();
            if (boleta != null)
            {
                Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.Email == boleta.Email select o).SingleOrDefault();
                if (miembro == null)
                {
                    miembro = new Miembro();
                    miembro.CelulaId = celulaId;
                    miembro.Email = boleta.Email;
                    miembro.Contrasena = string.Empty;
                    miembro.Primer_Nombre = boleta.PrimerNombre;
                    miembro.Segundo_Nombre = boleta.SegundoNombre;
                    miembro.Apellido_Paterno = boleta.ApellidoPaterno;
                    miembro.Apellido_Materno = boleta.ApellidoMaterno;
                    miembro.GeneroId = boleta.GeneroId;
                    miembro.EstadoCivilId = boleta.EstadoCivilId;
                    miembro.Fecha_Nacimiento = (boleta.FechaDeNacimiento.HasValue ? boleta.FechaDeNacimiento : DateTime.Now);
                    miembro.UbicacionMunicipioId = boleta.UbicacionMunicipioId;
                    miembro.Colonia = boleta.Colonia;
                    miembro.Direccion = boleta.Direccion;
                    miembro.Tel_Casa = boleta.TelefonoCasa;
                    miembro.Tel_Movil = boleta.TelefonoMovil;
                    miembro.Tel_Trabajo = boleta.TelefonoTrabajo;
                    miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }

                return miembro;
            }
            else
            {
                throw new ExcepcionReglaNegocio("Boleta NO existente.");
            }
        }

        public ConsolidacionBoleta ObtenerUltimaBoletaRegistrada()
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Borrado == false orderby o.Id descending select o).FirstOrDefault();
        }


    }
}
