using AdministradorDeIglesiasV2.Core.Modelos;
using log4net;
using Quartz;
using Quiksoft.FreeSMTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AdministradorDeIglesiasV2.Core.Manejadores;
using ExtensionMethods;

namespace AdministradorDeIglesiasV2.Core.ScheduledJobs
{
    public class NotificacionDeFaltaDeRegistroDeAsistencia : IJob
    {
        private static readonly ILog log = LogManager.GetLogger("ScheduledJobs");

        public void Execute(IJobExecutionContext context)
        {
            log.Debug("Iniciando NotificacionDeFaltaDeRegistroDeAsistencia");

            IglesiaEntities contexto = new IglesiaEntities();
            ManejadorDeReportesDeAsistencias manejadorDeAsistencias = new ManejadorDeReportesDeAsistencias();
            ManejadorDeCorreos manejadorDeCorreos = new ManejadorDeCorreos();
            DateTime fecha = DateTime.Now;
            DateTime fechaInicial = fecha.GetFirstDateOfWeek().AddDays(-7);
            int semanaActual = fecha.GetWeekNumber();
            string tituloDelReporte = string.Format("Asistencias de Celula NO REGISTRADAS de la semana {0} ({2} a {3})", semanaActual, fecha.Year, fechaInicial.ToString("dd/MMM/yy"), fechaInicial.AddDays(7).ToString("dd/MMM/yy"));

            //Obtenemos todos los miembros inscritos a este reporte
            List<int> inscripcionesANotificar = (from inscripciones in contexto.NotificacionDeAsistenciaInscripcion
                                                 join logs in contexto.NotificacionDeAsistenciaLog on
                                                    inscripciones.Id equals logs.NotificacionDeAsistenciaId into ps
                                                 from logs in ps.DefaultIfEmpty()
                                                 where
                                                    inscripciones.Borrado == false &&
                                                    inscripciones.Miembro.Borrado == false &&
                                                    (logs.Semana != semanaActual || logs == null) &&
                                                    (((inscripciones.DiaSemanaId == (int)fecha.DayOfWeek) && ((inscripciones.HoraDiaId / 2) - .5 <= fecha.Hour)) || // Si es el mismo dia de la inscripcion valida la hora actual contra la de la inscripcion
                                                    ((int)fecha.DayOfWeek > inscripciones.DiaSemanaId)) // Si ya se paso del dia de la inscipcion se manda la notificacion sin importar la hora
                                                 select inscripciones.Id).ToList<int>();

            NotificacionDeAsistenciaInscripcion notificacionInscripcion;
            if (inscripcionesANotificar.Count > 0)
            {
                log.Debug(String.Format("{0} miembros inscritos a notificaciones", inscripcionesANotificar.Count));
                foreach (int inscripcionId in inscripcionesANotificar)
                {
                    // Obtenemos el miembro suscrito y su email
                    notificacionInscripcion = (from o in contexto.NotificacionDeAsistenciaInscripcion where o.Id == inscripcionId select o).SingleOrDefault();

                    // Se mandara un correo unico por miembro; incluyendo toda la info de todas las celulas a las que es lider directo
                    EmailMessage email = manejadorDeAsistencias.GenerarCorreoSemanalDeFaltaDeAsistenciasPorRed(fechaInicial, notificacionInscripcion.Miembro, (ManejadorDeReportesDeAsistencias.TipoDeReporte)notificacionInscripcion.TipoId, tituloDelReporte);

                    // Mandaremos el correo, unicamente si se genero. Si no tenia faltas de asistencias NO se manda correo.
                    if (email != null)
                    {
                        Action generarLog = delegate()
                        {
                            // Actualizamos o creamos el log; para guardar cuando fue la ultima vez que se proceso dicha inscripcion y no volverla a procesar hasta dentro de una semana
                            NotificacionDeAsistenciaLog notificacionLog = (from o in contexto.NotificacionDeAsistenciaLog where o.NotificacionDeAsistenciaId == inscripcionId select o).SingleOrDefault();

                            if (notificacionLog == null)
                            {
                                notificacionLog = new NotificacionDeAsistenciaLog();
                            }

                            notificacionLog.NotificacionDeAsistenciaId = inscripcionId;
                            notificacionLog.Semana = semanaActual;
                            notificacionLog.Guardar(contexto);

                            log.InfoFormat("Log de notificacion generado correctamente para: {0}", notificacionLog.NotificacionDeAsistenciaInscripcion.Miembro.Email);
                        };

                        log.Debug(String.Format("Al usuario {0} se le notificara de las celulas con falta de registro de asistencia", notificacionInscripcion.Miembro.NombreCompleto));

                        // Enviamos el correo
                        manejadorDeCorreos.EnviarCorreoAsync(email, generarLog);
                    }
                }

            }

            log.Debug("Finalizando NotificacionDeFaltaDeRegistroDeAsistencia");
        }
    }
}
