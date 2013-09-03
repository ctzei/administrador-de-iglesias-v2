using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;

namespace AdministradorDeIglesiasV2.Website.Mobile.Controllers.RegistroDeAsistencia
{
    public class RegistroDeAsistenciaController : Controller
    {
        public ActionResult Index()
        {
            ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
            ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();

            List<RegistroBasico> celulas = manejadorDeCelulas.ObtenerCelulasPermitidasPorMiembro(SesionActual.Instance.UsuarioId);

            Celula celulaPrincipal = manejadorDeCelulas.ObtenerCelulaQueMiembroEsLider(SesionActual.Instance.UsuarioId);
            RegistroBasico celulaPreseleccionada = new RegistroBasico()
            {
                Id = celulaPrincipal.CelulaId,
                Descripcion = celulaPrincipal.Descripcion
            };

            DateTime fechaPreseleccionada = manejadorDeAsistencias.ObtenerFechaDeSiguienteAsistencia(celulaPreseleccionada.Id);
            Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistenciaPreseleccionada = manejadorDeAsistencias.ObtenerAsistencia(celulaPreseleccionada.Id, fechaPreseleccionada);

            List<RegistroBasico> dias = new List<RegistroBasico>();
            for (int i = 1; i <= 31; i++){

                string descripcion;
                try
                {
                    DateTime d = new DateTime(fechaPreseleccionada.Year, fechaPreseleccionada.Month, i);
                    descripcion = i.ToString().PadLeft(2, '0') + " - " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.DayNames[(int)(new DateTime(fechaPreseleccionada.Year, fechaPreseleccionada.Month, i)).DayOfWeek]);
                }
                catch (Exception)
                {
                    descripcion = i.ToString();
                }

                RegistroBasico dia = new RegistroBasico();
                dia.Id = i;
                dia.Descripcion = descripcion;
                dias.Add(dia);
            }

            List<RegistroBasico> meses = new List<RegistroBasico>();
            for (int i = 1; i < 13; i++)
            {
                RegistroBasico mes = new RegistroBasico();
                mes.Id = i;
                mes.Descripcion =  CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CultureInfo.CurrentUICulture.DateTimeFormat.MonthNames[i - 1]);
                meses.Add(mes);
            }

            List<RegistroBasico> anios = new List<RegistroBasico>();
            for (int i = 2011; i <= 2015; i++)
            {
                RegistroBasico anio = new RegistroBasico();
                anio.Id = i;
                anio.Descripcion = i.ToString();
                anios.Add(anio);
            }

            ViewBag.CelulasPermitidas = new SelectList(celulas, "Id", "Descripcion", celulaPreseleccionada.Id);
            ViewBag.Dias = new SelectList(dias, "Id", "Descripcion", fechaPreseleccionada.Day);
            ViewBag.Meses = new SelectList(meses, "Id", "Descripcion", fechaPreseleccionada.Month);
            ViewBag.Anios = new SelectList(anios, "Id", "Descripcion", fechaPreseleccionada.Year);
            ViewBag.NumeroDeInvitados = 0;
            ViewBag.Asistencia = this.RenderPartialView("_Asistencia", asistenciaPreseleccionada.Asistencias);

            return View();
        }

        public JsonResult MostrarFaltante(int celulaId)
        {
            ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();
            DateTime fechaPreseleccionada = manejadorDeAsistencias.ObtenerFechaDeSiguienteAsistencia(celulaId);
            return (Mostrar(celulaId, fechaPreseleccionada.Day, fechaPreseleccionada.Month, fechaPreseleccionada.Year));
        }

        public JsonResult Mostrar(int celulaId, int dia, int mes, int anio)
        {
            try
            {
                ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();
                DateTime fecha = new DateTime(anio, mes, dia);
                Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistencia = manejadorDeAsistencias.ObtenerAsistencia(celulaId, fecha);

                string msg;
                if (manejadorDeAsistencias.CelulaFueCancelada(celulaId, fecha))
                {
                    msg = Resources.Literales.AsistenciaACelulaPreviamenteCancelada;
                }
                else
                {
                    msg = string.Empty;
                }

                ViewBag.NumeroDeInvitados = manejadorDeAsistencias.ObtenerNumeroDeInvitados(celulaId, fecha);
                return Json(new {html = this.RenderPartialView("_Asistencia", asistencia.Asistencias), msg = msg, dia = dia, mes = mes, anio = anio});
            }
            catch (ExcepcionReglaNegocio ex)
            {
                return Json(new { error = ex.Message });
            }
            catch (ArgumentOutOfRangeException)
            {
                return Json(new { error = Resources.Literales.CelulaYFechaNecesarias });
            }
        }

        public JsonResult Guardar(int celulaId, int dia, int mes, int anio, int numeroDeInvitados, List<Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembro> asistencias)
        {
            try
            {
                Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada asistenciasSumarizadas = new Core.Modelos.Retornos.AsistenciaDeCelulaPorMiembroSumarizada(asistencias);
                ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();
                manejadorDeAsistencias.GuardarAsistencia(celulaId, new DateTime(anio, mes, dia), asistenciasSumarizadas, numeroDeInvitados, SesionActual.Instance.UsuarioId);
                return Json(new { msg = Resources.Literales.AsistenciaGuardadaCorrectamente });
            }
            catch (ExcepcionReglaNegocio ex)
            {
                return Json(new { error = ex.Message });
            }
            catch (ArgumentOutOfRangeException)
            {
                return Json(new { error = Resources.Literales.CelulaYFechaNecesarias });
            }
        }

        public JsonResult Cancelar(int celulaId, int dia, int mes, int anio, string razon)
        {
            try
            {
                ManejadorDeAsistenciasDeCelula manejadorDeAsistencias = new ManejadorDeAsistenciasDeCelula();
                manejadorDeAsistencias.CancelarAsistencia(celulaId, new DateTime(anio, mes, dia), razon, SesionActual.Instance.UsuarioId);
                return Json(new { msg = Resources.Literales.CancelacionDeAsistenciaGuardadaCorrectamente });
            }
            catch (ExcepcionReglaNegocio ex)
            {
                return Json(new { error = ex.Message });
            }
            catch (ArgumentOutOfRangeException)
            {
                return Json(new { error = Resources.Literales.CelulaYFechaNecesarias });
            }
        }
    }
}
