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

namespace AdministradorDeIglesiasV2.Core.ScheduledJobs
{
    public class NotificacionDeNuevasBoletasDeConsolidacion : IJob
    {
        private static readonly ILog log = LogManager.GetLogger("ScheduledJobs");

        public void Execute(IJobExecutionContext context)
        {
            log.Debug("Iniciando NotificacionDeNuevasBoletasDeConsolidacion");
            IglesiaEntities contexto = new IglesiaEntities();

            #region Reporte de Nuevas Boletas asignadas a Miembro

            List<ConsolidacionBoleta> boletasParaMiembros = (from o in contexto.ConsolidacionBoleta where o.Modificacion >= DateTime.Today && o.AsignadaAMiembroId != null && o.Borrado == false select o).ToList<ConsolidacionBoleta>();
            log.Debug(String.Format("{0} boletas ya asignadas a algun miembro modificadas hoy", boletasParaMiembros.Count));

            Dictionary<Miembro, List<ConsolidacionBoleta>> boletasPorMiembro = new Dictionary<Miembro, List<ConsolidacionBoleta>>();
            foreach (ConsolidacionBoleta boleta in boletasParaMiembros) {
                if (!boletasPorMiembro.ContainsKey(boleta.Miembro))
                {
                    boletasPorMiembro.Add(boleta.Miembro, new List<ConsolidacionBoleta>());
                }
                boletasPorMiembro[boleta.Miembro].Add(boleta);
            }

            foreach (KeyValuePair<Miembro, List<ConsolidacionBoleta>> entry in boletasPorMiembro)
            {
                log.DebugFormat("Iniciando reporte de Nuevas Boletas de Consolidacion para (miembro): {0} - {1} boletas", entry.Key.NombreCompleto, entry.Value.Count);
                EmailMessage email = new EmailMessage();

                // Contenido del correo
                StringBuilder contenido = new StringBuilder(750);
                contenido.AppendFormat("<h3>Reporte de Nuevas Boletas de Consolidacion para: {0}</h3>", entry.Key.NombreCompleto);
                contenido.AppendFormat("<p>A continuacion se mostraran las nuevas boletas de consolidacion:</p>");
                contenido.AppendLine("<table border='1' width='500px'><thead><tr><td>Id</td><td>Nombre</td></tr></thead><tbody>");
                foreach (ConsolidacionBoleta boleta in entry.Value)
                {
                    contenido.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", boleta.Id, boleta.PrimerNombre + " " + boleta.ApellidoPaterno);
                }
                contenido.AppendFormat("</tbody></table>");
                email.BodyParts.Add(contenido.ToString(), BodyPartFormat.HTML);

                email.Recipients.Add(entry.Key.Email, entry.Key.NombreCompleto, RecipientType.To);
                email.Subject = "Nuevas Boletas de Consolidacion";

                ManejadorDeCorreos manejadorDeCorreos = new ManejadorDeCorreos();
                manejadorDeCorreos.EnviarCorreoAsync(email);
            }

            #endregion

            #region Reporte de Nuevas Boletas asignadas a Celula

            int estatus = ConsolidacionBoleta.Estatus.ASIGNADO_A_CELULA.Key;
            List<ConsolidacionBoleta> boletasParaCelulas = (from o in contexto.ConsolidacionBoleta where o.Modificacion >= DateTime.Today && o.AsignadaACelulaId != null && o.BoletaEstatusId == estatus && o.Borrado == false select o).ToList<ConsolidacionBoleta>();
            log.Debug(String.Format("{0} boletas ya asignadas a alguna celula modificadas hoy", boletasParaCelulas.Count));

            Dictionary<Celula, List<ConsolidacionBoleta>> boletasPorCelula = new Dictionary<Celula, List<ConsolidacionBoleta>>();
            foreach (ConsolidacionBoleta boleta in boletasParaCelulas)
            {
                if (!boletasPorCelula.ContainsKey(boleta.Celula))
                {
                    boletasPorCelula.Add(boleta.Celula, new List<ConsolidacionBoleta>());
                }
                boletasPorCelula[boleta.Celula].Add(boleta);
            }

            foreach (KeyValuePair<Celula, List<ConsolidacionBoleta>> entry in boletasPorCelula)
            {
                log.DebugFormat("Iniciando reporte de Nuevos Miembros asignados desde Consolidacion para (celula): {0} [{1}] - {2} boletas", entry.Key.Descripcion, entry.Key.CelulaId, entry.Value.Count);
                EmailMessage email = new EmailMessage();

                // Contenido del correo
                StringBuilder contenido = new StringBuilder(750);
                contenido.AppendFormat("<h3>Reporte de Nuevos Miembros asignados desde Consolidacion para: {0}</h3>", entry.Key.Descripcion);
                contenido.AppendFormat("<p>A continuacion se mostraran los nuevos miembros asignados desde consolidacion:</p>");
                contenido.AppendLine("<table border='1' width='500px'><thead><tr><td>Id</td><td>Nombre</td></tr></thead><tbody>");
                foreach (ConsolidacionBoleta boleta in entry.Value)
                {
                    contenido.AppendFormat("<tr><td>{0}</td><td>{1}</td></tr>", boleta.Id, boleta.PrimerNombre + " " + boleta.ApellidoPaterno);
                }
                contenido.AppendFormat("</tbody></table>");
                email.BodyParts.Add(contenido.ToString(), BodyPartFormat.HTML);

                // Agregamos a todos los lideres de celula al correo
                foreach (CelulaLider celulaLider in entry.Key.CelulaLider.Where(x => x.Borrado == false))
                {
                    email.Recipients.Add(celulaLider.Miembro.Email, celulaLider.Miembro.NombreCompleto, RecipientType.To);
                }

                email.Subject = "Nuevas Miembros asignados desde Consolidacion";

                ManejadorDeCorreos manejadorDeCorreos = new ManejadorDeCorreos();
                manejadorDeCorreos.EnviarCorreoAsync(email);
            }

            #endregion

            log.Debug("Finalizando NotificacionDeNuevasBoletasDeConsolidacion");
        }
    }
}
