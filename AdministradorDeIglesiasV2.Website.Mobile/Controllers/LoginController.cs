using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;

namespace AdministradorDeIglesiasV2.Website.Mobile.Controllers
{
    public class LoginController : ControladorBase
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            ViewBag.UrlDeVersionCompleta = WebConfigurationManager.AppSettings["UrlDeVersionCompleta"];
            return View();
        }

        public JsonResult IniciarSesion(string email, string password, bool recordarme)
        {
            try
            {
                ManejadorDeMiembros manejador = new ManejadorDeMiembros();
                manejador.IniciarSesion(email, password);
                CrearCookie(email, recordarme);
            }
            catch (ExcepcionReglaNegocio ex)
            {
                return Json(new {error = ex.Message});
            }
            return  Json(new {url = Url.Action("Index", "Home")});
        }

        private void CrearCookie(string email, bool persistirCookie)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(email, persistirCookie);
        }
    }
}
