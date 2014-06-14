
using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos
{
    public partial class ConsolidacionBoleta
    {
        public class Estatus
        {
            // ABIERTA
            public readonly static KeyValuePair<int, string> SIN_REPORTE = new KeyValuePair<int, string>(101, "Sin Reporte");
            public readonly static KeyValuePair<int, string> NO_LOCALIZADO = new KeyValuePair<int, string>(102, "No localizado aún");

            // SEGUIMIENTO
            public readonly static KeyValuePair<int, string> PENDIENTE_ASIGNAR_CELULA = new KeyValuePair<int, string>(201, "Pendiente asignar célula");
            public readonly static KeyValuePair<int, string> ASIGNADO_A_CELULA = new KeyValuePair<int, string>(202, "Asignado a célula");

            // CERRADA
            public readonly static KeyValuePair<int, string> ASISTE_A_IGLESIA = new KeyValuePair<int, string>(1, "Asiste a Iglesia");
            public readonly static KeyValuePair<int, string> ASISTE_A_CELULA = new KeyValuePair<int, string>(2, "Asiste a célula");
            public readonly static KeyValuePair<int, string> ASISTE_A_IGLESIA_Y_CELULA = new KeyValuePair<int, string>(3, "Asiste a iglesia y célula");
            public readonly static KeyValuePair<int, string> NO_INTERESADO = new KeyValuePair<int, string>(4, "No interesado");
            public readonly static KeyValuePair<int, string> ASISTE_A_OTRA_IGLESIA = new KeyValuePair<int, string>(5, "Asiste a otra iglesia");
            public readonly static KeyValuePair<int, string> DATOS_INCORRECTOS = new KeyValuePair<int, string>(6, "Datos incorrectos");

            public static List<KeyValuePair<int, string>> Lista()
            {
                List<KeyValuePair<int, string>> rtn = new List<KeyValuePair<int, string>>();

                rtn.Add(SIN_REPORTE);
                rtn.Add(NO_LOCALIZADO);
                rtn.Add(PENDIENTE_ASIGNAR_CELULA);
                rtn.Add(ASIGNADO_A_CELULA);
                rtn.Add(ASISTE_A_IGLESIA);
                rtn.Add(ASISTE_A_CELULA);
                rtn.Add(ASISTE_A_IGLESIA_Y_CELULA);
                rtn.Add(NO_INTERESADO);
                rtn.Add(ASISTE_A_OTRA_IGLESIA);
                rtn.Add(DATOS_INCORRECTOS);

                return rtn;
            }

            public static bool Abierta(int? estatus)
            {
                if (estatus.HasValue)
                {
                    return estatus.Value.inRange(101, 199);
                }
                else
                {
                    return false;
                }
            }

            public static bool Seguimiento(int? estatus)
            {
                if (estatus.HasValue)
                {
                    return estatus.Value.inRange(201, 299);
                }
                else
                {
                    return false;
                }
            }

            public static bool Cerrada(int? estatus)
            {
                if (estatus.HasValue)
                {
                    return estatus.Value.inRange(1, 99);
                }
                else
                {
                    return false;
                }
            }
        }

        partial void OnAgregar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.CrearBoletasDeConsolidacion);
            validarEntidad(entry);
        }

        partial void OnModificar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.ModificarBoletasDeConsolidacion);
            validarEntidad(entry);
        }

        partial void OnBorrar(System.Data.Objects.ObjectStateEntry entry)
        {
            SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.BorrarBoletasDeConsolidacion);
        }

        private void validarEntidad(System.Data.Objects.ObjectStateEntry entry)
        {
            ConsolidacionBoleta entidad = (ConsolidacionBoleta)entry.Entity;
            Validaciones.ValidarEmail(entidad.Email);
            validarEstatus(entidad);
            establecerEdad(entidad);
            establecerCategoria(entidad);
            validarUnicidad(entry);
        }

        private void validarEstatus(ConsolidacionBoleta entidad)
        {
            // El valor predeterminado...
            if (entidad.BoletaEstatusId.HasValue == false)
            {
                entidad.BoletaEstatusId = Estatus.SIN_REPORTE.Key;
            }

            // Si se asigna a alguna celula y NO esta cerrada... se marca con el estatus correspondiente y se crea y asigna el miembro en la celula correspondiente
            if (entidad.AsignadaACelulaId.HasValue == true && entidad.AsignadaACelulaId.Value > 0 && !Estatus.Cerrada(entidad.BoletaEstatusId))
            {
                entidad.BoletaEstatusId = Estatus.ASIGNADO_A_CELULA.Key;

                // Crea/Agrega el miembro a la celula
                crearMiembroDesdeBoleta(entidad);
            }

            // Si se marca como asignado a alguna celula pero NO tiene célula asignada se notifica al usuario
            if (entidad.BoletaEstatusId == Estatus.ASIGNADO_A_CELULA.Key && entidad.AsignadaACelulaId.HasValue == false)
            {
                throw new ExcepcionReglaNegocio("Es necesario seleccionar una célula a quien se le asignara la boleta para continuar.");
            }
        }

        private void crearMiembroDesdeBoleta(ConsolidacionBoleta entidad)
        {
            IglesiaEntities contexto = new IglesiaEntities();

            Miembro miembro = (from o in contexto.Miembro where o.Email == entidad.Email select o).SingleOrDefault();
            if (miembro == null)
            {
                miembro = new Miembro();
                miembro.CelulaId = entidad.AsignadaACelulaId.Value;
                miembro.Email = entidad.Email;
                miembro.Contrasena = string.Empty;
                miembro.Primer_Nombre = entidad.PrimerNombre;
                miembro.Segundo_Nombre = entidad.SegundoNombre;
                miembro.Apellido_Paterno = entidad.ApellidoPaterno;
                miembro.Apellido_Materno = entidad.ApellidoMaterno;
                miembro.GeneroId = entidad.GeneroId;
                miembro.EstadoCivilId = entidad.EstadoCivilId;
                miembro.Fecha_Nacimiento = (entidad.FechaDeNacimiento.HasValue ? entidad.FechaDeNacimiento : DateTime.Now);
                miembro.UbicacionMunicipioId = entidad.UbicacionMunicipioId;
                miembro.Colonia = entidad.Colonia;
                miembro.Direccion = entidad.Direccion;
                miembro.Tel_Casa = entidad.TelefonoCasa;
                miembro.Tel_Movil = entidad.TelefonoMovil;
                miembro.Tel_Trabajo = entidad.TelefonoTrabajo;
                miembro.Comentario = entidad.Observaciones;

                miembro.Creacion = DateTime.Now;
                miembro.Modificacion = DateTime.Now;
                miembro.CreacionId = SesionActual.Instance.UsuarioId;
                miembro.ModificacionId = SesionActual.Instance.UsuarioId;

                contexto.AddObject(miembro.GetType().Name, miembro);
                contexto.SaveChanges(System.Data.Objects.SaveOptions.DetectChangesBeforeSave, true);
            }
        }

        private void validarUnicidad(System.Data.Objects.ObjectStateEntry entry)
        {
            // Validamos si no existe ya una boleta con el mismo correo
            ConsolidacionBoleta entidadPreexistente = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Email == ((ConsolidacionBoleta)entry.Entity).Email select o).SingleOrDefault();

            if (entidadPreexistente != null)
            {
                string registroExistenteMsg = string.Format("Ya existe una boleta para ese email [{0}], la cual su ID es [{1}]. De ser necesario, buscarla y modificarla o utilizar un email distinto.", entidadPreexistente.Email, entidadPreexistente.Id);

                if (entry.State == System.Data.EntityState.Added)
                {
                    throw new ExcepcionReglaNegocio(registroExistenteMsg);
                }
                else if (entry.State == System.Data.EntityState.Modified)
                {
                    if (Validaciones.ValidarCambiosEnCampo(entry, "email"))
                    {
                        throw new ExcepcionReglaNegocio(registroExistenteMsg);
                    }
                }
            }
        }

        private void establecerEdad(ConsolidacionBoleta entidad)
        {
            if (entidad.FechaDeNacimiento.HasValue)
            {
                entidad.Edad = DateTime.Now.Subtract(entidad.FechaDeNacimiento.Value).Days / 365;
            }
        }

        private void establecerCategoria(ConsolidacionBoleta entidad)
        {
            int categoriaId;
            // Jovenes
            if (entidad.EstadoCivilId != 2 && entidad.Edad < 38)
            {
                categoriaId = 1;
            }
            // Matrimonios Jovenes
            else if (entidad.EstadoCivilId == 2 && entidad.Edad < 38)
            {
                categoriaId = 2;
            }
            // Familiar
            else if (entidad.Edad >= 38 && entidad.Edad < 50)
            {
                categoriaId = 3;
            }
            // Red Dorada
            else if (entidad.Edad >= 50)
            {
                categoriaId = 4;
            }
            // Otra
            else
            {
                categoriaId = 99;
            }
            entidad.CategoriaBoletaId = categoriaId;

            // Si la boleta no se encuentra asignada a alguna celula, se asigna a la celula predeterminada de acuerdo a su categoria
            /* if (!entidad.AsignadaACelulaId.HasValue)
            {
                entidad.AsignadaACelulaId = entidad.ConsolidacionBoletaCategoria.Celula.CelulaId;
            } */
        }
    }
}
