using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Net.Mail;
using System.Diagnostics;
using System.IO;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ZagueEF.Core;
using ExtensionMethods;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeFoli
    {

        public List<FoliGrupo> ObtenerGruposPorMiembroActual()
        {
            return ObtenerGruposPorMiembro(SesionActual.Instance.UsuarioId);
        }

        public List<FoliGrupo> ObtenerGruposPorMiembro(int miembroId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMaestro where o.MiembroId == miembroId && o.FoliGrupo.Borrado == false && o.Borrado == false select o.FoliGrupo).ToList<FoliGrupo>();
        }

        public Modelos.Retornos.AvanceDeFoliSumarizado ObtenerAvances(int grupoId, DateTime fecha)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            List<Modelos.Retornos.AvanceDeFoli> avances = (
                from alumno in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMiembro
                join avance in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMiembroAvance
                 on
                    new { FolioMiembroId = alumno.Id, Anio = anioSeleccionado, Mes = mesSeleccionado, Dia = diaSeleccionado }
                    equals
                    new { FolioMiembroId = avance.FoliMiembroId, Anio = avance.Anio, Mes = avance.Mes, Dia = avance.Dia }
                    into ps
                from avance in ps.DefaultIfEmpty()
                where alumno.FoliGrupoId == grupoId
                select new Modelos.Retornos.AvanceDeFoli
                {
                    Id = (avance != null ? avance.Id : -1),
                    FoliMiembroId = alumno.Id,
                    MiembroId = alumno.MiembroId,
                    PrimerNombre = alumno.Miembro.Primer_Nombre,
                    SegundoNombre = alumno.Miembro.Segundo_Nombre,
                    ApellidoPaterno = alumno.Miembro.Apellido_Paterno,
                    ApellidoMaterno = alumno.Miembro.Apellido_Materno,
                    Asistencia = (avance != null ? avance.Asistencia : false),
                    Tarea = (avance != null ? avance.Tarea : false),
                    CalificacionTarea = (avance != null ? avance.Calificacion_Tarea : 0),
                    CalificacionExamen = (avance != null ? avance.Calificacion_Examen : 0),
                }
            ).ToList<Modelos.Retornos.AvanceDeFoli>();

            Modelos.Retornos.AvanceDeFoliSumarizado rtn = new Modelos.Retornos.AvanceDeFoliSumarizado();
            rtn.Avances = avances;
            rtn.CantidadDeRegistros = avances.Count();
            rtn.CantidadDeAsistencias = avances.Count(o => o.Asistencia == true);

            return rtn;
        }

        public bool GuardarAvances(int grupoId, DateTime fecha, List<Dictionary<string, string>> avances, int usuarioIdQueRegistra)
        {
            int anioSeleccionado = fecha.Year;
            int mesSeleccionado = fecha.Month;
            int diaSeleccionado = fecha.Day;

            Dictionary<int, string> asistenciasNuevas = new Dictionary<int, string>();
            List<int> asistenciasABorrar = new List<int>();
            Dictionary<int, string> asistenciasAActualizar = new Dictionary<int, string>();

            foreach (Dictionary<string, string> avance in avances)
            {
                string sId = avance["Id"];
                string sFoliMiembroId = avance["FoliMiembroId"];
                string sAsistencia = avance["Asistencia"];
                string sTarea = avance["Tarea"];
                string sCalTarea = avance["CalificacionTarea"];
                string sCalExamen = avance["CalificacionExamen"];

                int id;
                int foliMiembroId;
                bool tieneAsistencia;
                bool trajoTarea;
                int calificacionTarea;
                int calificacionExamen;

                if ((int.TryParse(sId, out id)) && (int.TryParse(sFoliMiembroId, out foliMiembroId)) && (bool.TryParse(sAsistencia, out tieneAsistencia)) && (bool.TryParse(sTarea, out trajoTarea)) && (int.TryParse(sCalTarea, out calificacionTarea)) && (int.TryParse(sCalExamen, out calificacionExamen)))
                {
                    FoliMiembroAvance entidad;
  
                    //Registros nuevos a crear
                    if (id < 0)
                    {
                        entidad = new FoliMiembroAvance();
                    }
                    //Registros a actualizar
                    else
                    {
                        entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMiembroAvance where o.Id == id && o.Borrado == false select o).SingleOrDefault();
                    }

                    entidad.FoliMiembroId = foliMiembroId;
                    entidad.Anio = anioSeleccionado;
                    entidad.Mes = mesSeleccionado;
                    entidad.Dia = diaSeleccionado;
                    entidad.Asistencia = tieneAsistencia;
                    entidad.Tarea = trajoTarea;
                    entidad.Calificacion_Tarea = calificacionTarea;
                    entidad.Calificacion_Examen = calificacionExamen;
                    entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }
            }

            return true;
        }

        public bool GuardarMaestrosYAlumnosEnGrupo(int grupoId, RegistrosHelper.RegistrosDeDatos maestrosNuevosYEliminados, RegistrosHelper.RegistrosDeDatos alumnosNuevosYEliminados)
        {
            bool rtn = false;

            if (grupoId > 0 && (maestrosNuevosYEliminados.RegistrosNuevosId.Count > 0 || alumnosNuevosYEliminados.RegistrosNuevosId.Count > 0 || maestrosNuevosYEliminados.RegistrosEliminadosId.Count > 0 || alumnosNuevosYEliminados.RegistrosEliminadosId.Count > 0))
            {
                using (System.Transactions.TransactionScope transactionScope = new System.Transactions.TransactionScope())
                {
                    FoliGrupo foliGrupo = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliGrupo where o.Id == grupoId select o).FirstOrDefault();
                    FoliMaestro maestro;
                    FoliMiembro alumno;

                    if (foliGrupo == null) { throw new ExcepcionReglaNegocio(Literales.RegistroInexistente); }

                    //Agregamos las nuevas maestros (siempre y cuando no existan previamente...)
                    foreach (int miembroId in maestrosNuevosYEliminados.RegistrosNuevosId)
                    {
                        maestro = new FoliMaestro();
                        maestro.FoliGrupoId = grupoId;
                        maestro.MiembroId = miembroId;
                        foliGrupo.FoliMaestro.Add(maestro);
                    }

                    //Agregamos los nuevos alumnos (siempre y cuando no existan previamente...)
                    foreach (int miembroId in alumnosNuevosYEliminados.RegistrosNuevosId)
                    {
                        alumno = new FoliMiembro();
                        alumno.FoliGrupoId = grupoId;
                        alumno.MiembroId = miembroId;
                        alumno.FoliMiembroEstatusId = 1;
                        foliGrupo.FoliMiembro.Add(alumno);
                    }

                    //Guardamos los cambios, antes de eliminar registros
                    foliGrupo.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                    //Eliminamos los maestros
                    foreach (int miembroId in maestrosNuevosYEliminados.RegistrosEliminadosId)
                    {
                        maestro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMaestro where o.FoliGrupoId == grupoId && o.MiembroId == miembroId && o.Borrado == false select o).FirstOrDefault();
                        if (maestro != null)
                        {
                            maestro.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                        }
                    }

                    //Eliminamos los alumnos
                    foreach (int miembroId in alumnosNuevosYEliminados.RegistrosEliminadosId)
                    {
                        alumno = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMiembro where o.FoliGrupoId == grupoId && o.MiembroId == miembroId && o.Borrado == false select o).FirstOrDefault();
                        if (alumno != null)
                        {
                            alumno.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                        }
                    }

                    //Marcamos como finalizadas todas las operaciones, para que la transaccion se lleve a cabo en la BD
                    transactionScope.Complete();
                }
            }

            return rtn;
        }

        public List<RegistroBasico> ObtenerMaestrosPorGrupo(int grupoId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMaestro where 
                        o.Borrado == false 
                    select new RegistroBasico { 
                        Id = o.MiembroId, 
                        Descripcion = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno + " (" + o.Miembro.Email + ")" 
                    }).ToList<RegistroBasico>();
        }

        public List<RegistroBasico> ObtenerAlumnosPorGrupo(int grupoId)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().FoliMiembro
                    where
                        o.Borrado == false
                    select new RegistroBasico
                    {
                        Id = o.MiembroId,
                        Descripcion = o.Miembro.Primer_Nombre + " " + o.Miembro.Segundo_Nombre + " " + o.Miembro.Apellido_Paterno + " " + o.Miembro.Apellido_Materno + " (" + o.Miembro.Email + ")"
                    }).ToList<RegistroBasico>();
        }
    }
}
