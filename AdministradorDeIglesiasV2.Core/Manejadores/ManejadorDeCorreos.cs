using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;
using System.Data.Objects;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Modelos;
using ZagueEF.Core;
using Quiksoft.FreeSMTP;
using log4net;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeCorreos
    {
        private static readonly ILog log = LogManager.GetLogger("Email");

        #region Pruebas

        public void ProbarCorreo(string remitente, string servidorSmtp, string contenido, string asunto, string destinatario)
        {
            EmailMessage email = new EmailMessage(destinatario, remitente, asunto, contenido, BodyPartFormat.Plain);
            SMTP smtpClient = new SMTP(servidorSmtp);
            smtpClient.Send(email);
        }

        public void ProbarCorreoAsync(string remitente, string servidorSmtp, string contenido, string asunto, string destinatario)
        {
            EmailMessage email = new EmailMessage(destinatario, remitente, asunto, contenido, BodyPartFormat.Plain);

            Action generarLog = delegate()
            {
                log.InfoFormat("El callback dormira 5sec");
                System.Threading.Thread.Sleep(5000);
                log.InfoFormat("El callback desperto");

                IglesiaEntities contexto = new IglesiaEntities();

                // Actualizamos o creamos el log; para guardar cuando fue la ultima vez que se proceso dicha inscripcion y no volverla a procesar hasta dentro de una semana
                NotificacionDeAsistenciaLog notificacionLog = (from o in contexto.NotificacionDeAsistenciaLog where o.NotificacionDeAsistenciaId == -1 select o).SingleOrDefault();

                if (notificacionLog == null)
                {
                    notificacionLog = new NotificacionDeAsistenciaLog();
                }

                notificacionLog.NotificacionDeAsistenciaId = -1;
                notificacionLog.Semana = -1;
                notificacionLog.Guardar(contexto);
            };

            // Enviamos el correo
            this.EnviarCorreoAsync(servidorSmtp, email, generarLog);
        }

        #endregion

        public void EnviarCorreoAsync(string servidorSmtp, EmailMessage email)
        {
            EnviarCorreoAsync(servidorSmtp, email, null);
        }

        public void EnviarCorreoAsync(string servidorSmtp, EmailMessage email, Action callback)
        {
            AsyncMethodCaller caller = new AsyncMethodCaller(EnviarCorreoEnNuevoThread);
            caller.BeginInvoke(servidorSmtp, email, callback, new AsyncCallback(AsyncThreadCallback), null);
        }

        public List<string> ObtenerRecipientes(EmailMessage email)
        {
            List<string> recipients = new List<string>();
            if (email != null)
            {
                // Obtenemos a quienes se envio el correo
                foreach (Recipient recepient in email.Recipients)
                {
                    if (recepient.Type == RecipientType.To)
                    {
                        recipients.Add(recepient.Email);
                    }
                }
            }

            return recipients;
        }

        #region Metodos Privados

        private delegate void AsyncMethodCaller(string servidorSmtp, EmailMessage email, Action callback);

        private void EnviarCorreoEnNuevoThread(string servidorSmtp, EmailMessage email, Action callback)
        {
            // Obtenemos a quienes se envio el correo
            string recipients = string.Join(", ", ObtenerRecipientes(email));

            try
            {
                SMTP smtpClient = new SMTP(servidorSmtp);

                // Enviamos el correo
                smtpClient.Send(email);

                log.InfoFormat("Email enviado a: {0}", recipients);
                log.InfoFormat("Total de email enviados: {0}", email.Recipients.Count);

                try
                {
                    if (callback != null)
                    {
                        callback();
                    }
                }
                catch (Exception e)
                {
                    log.ErrorFormat("Ocurrio un error al momento de llamar la funcion 'callback' luego en enviar correctamente el email a: {0}", recipients);
                    log.Error(e);
                }
            }
            catch (Exception e)
            {
                log.ErrorFormat("Ocurrio un error al momento de estar mandando un email a: {0}", recipients);
                log.Error(e);
            }
        }

        private void AsyncThreadCallback(IAsyncResult ar)
        {
            try
            {
                AsyncResult result = (AsyncResult)ar;
                AsyncMethodCaller caller = (AsyncMethodCaller)result.AsyncDelegate;
                caller.EndInvoke(ar);
            }
            catch (Exception e)
            {
                log.Error("Ocurrio un error desconocido al regresar del 'thread' encargado de enviar los correos", e);
            }
        }

        #endregion

    }
}
