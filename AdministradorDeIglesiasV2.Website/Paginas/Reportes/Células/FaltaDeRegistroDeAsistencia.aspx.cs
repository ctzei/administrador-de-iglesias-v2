using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;
using Quiksoft.FreeSMTP;

namespace AdministradorDeIglesiasV2.Website.Paginas.Reportes.Celulas
{
    public partial class FaltaDeRegistroDeAsistencia : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                CargarControles();
            }                                 
        }

        public void CargarControles()
        {
            StoreDias.Cargar(DiaSemana.Obtener());
            StoreHoras.Cargar(HoraDia.Obtener());
            StoreTipoDeReportes.Cargar((from o in SesionActual.Instance.getContexto<IglesiaEntities>().NotificacionDeAsistenciaTipo select o));
            PrecargarDatosDeUsuarioActual();
        }

        private void PrecargarDatosDeUsuarioActual()
        {
            NotificacionDeAsistenciaInscripcion inscripcion = ObtenerInscripcionDeUsuarioActual(false);
            if (inscripcion != null)
            {
                chkInscribirse.Checked = true;
                cboDiaSemana.Value = inscripcion.DiaSemanaId;
                cboHoraDia.Value = inscripcion.HoraDiaId;
                cboTipoDeReporte.Value = inscripcion.TipoId;
                cboTipoDeReporteGenerado.Value = inscripcion.TipoId;
            }
        }

        [DirectMethod(ShowMask = true)]
        public void GuardarCambios()
        {
            NotificacionDeAsistenciaInscripcion inscripcion = ObtenerInscripcionDeUsuarioActual(true);

            //Vamos a crear la inscripcion o en su defecto, actualizarla
            if (chkInscribirse.Checked)
            {
                if (inscripcion == null)
                {
                    inscripcion = new NotificacionDeAsistenciaInscripcion();
                }

                inscripcion.MiembroId = SesionActual.Instance.UsuarioId;
                inscripcion.DiaSemanaId = cboDiaSemana.ObtenerId();
                inscripcion.HoraDiaId = cboHoraDia.ObtenerId();
                inscripcion.TipoId = cboTipoDeReporte.ObtenerId();
                inscripcion.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }
            //Vamos a borrar la inscripcion
            else
            {
                if (inscripcion != null)
                {
                    inscripcion.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }
            }

            X.Msg.Notify(Generales.nickNameDeLaApp, Resources.Literales.CambiosAplicados).Show();
        }

        [DirectMethod(ShowMask = true)]
        public void GenerarReporte()
        {
            try
            {
                Miembro miembro = ManejadorDeMiembros.ObtenerMiembroActual();

                ManejadorDeReportesDeAsistencias manejador = new ManejadorDeReportesDeAsistencias();
                DateTime fechaInicial = DateTime.Now.GetFirstDateOfWeek().AddDays(-7);
                EmailMessage mensaje = manejador.GenerarCorreoSemanalDeFaltaDeAsistenciasPorRed(fechaInicial, miembro, (ManejadorDeReportesDeAsistencias.TipoDeReporte)cboTipoDeReporteGenerado.ObtenerId(), string.Empty, WebConfigurationManager.AppSettings["RemitenteDeCorreos"]);

                if (mensaje != null)
                {
                    string destinatario = string.Empty;
                    List<string> destinatariosCC = new List<string>();
                    foreach (Recipient recipient in mensaje.Recipients)
                    {
                        if (recipient.Type == RecipientType.To)
                        {
                            destinatario = recipient.Email;
                        }
                        else
                        {
                            destinatariosCC.Add(recipient.Email);
                        }
                    }

                    pnlHtml.Html = "Para: " + destinatario + "<br/>CC: " + string.Join(",", destinatariosCC) + "<br/><hr/>" + ((BodyPart)mensaje.BodyParts.GetLastItem()).Body;
                }
                else
                {
                    pnlHtml.Html = "Todas las células han registrado sus asistencias correspondientes.";
                }
            }
            catch (ExcepcionAplicacion ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        private NotificacionDeAsistenciaInscripcion ObtenerInscripcionDeUsuarioActual(bool incluirBorradas)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().NotificacionDeAsistenciaInscripcion where o.MiembroId == SesionActual.Instance.UsuarioId && (o.Borrado == false || o.Borrado == incluirBorradas) select o).SingleOrDefault();
        }

    }
}