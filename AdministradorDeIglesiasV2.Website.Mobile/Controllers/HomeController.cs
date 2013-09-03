using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Manejadores;
using AdministradorDeIglesiasV2.Core.Modelos;

namespace AdministradorDeIglesiasV2.Website.Mobile.Controllers
{
    public class HomeController : ControladorBase
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ManejadorDeMiembros manejador = new ManejadorDeMiembros();
            return View(manejador.ObtenerPantallasPermitidasPorMiembro(SesionActual.Instance.UsuarioId, "WebMobileV2"));
        }

        public JsonResult CerrarSesion()
        {
            try
            {
                Session.Abandon();
                System.Web.Security.FormsAuthentication.SignOut();
                return Json(new {url = Url.Action("Index", "Login") });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
    }
}
