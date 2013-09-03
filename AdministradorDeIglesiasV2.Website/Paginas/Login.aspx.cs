using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
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

namespace AdministradorDeIglesiasV2.Website.Paginas
{
    public partial class Login : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            cmdVersionMobile.Listeners.Click.Handler = string.Format("window.location.href = '{0}'; return false;", WebConfigurationManager.AppSettings["UrlDeVersionMobile"]);
            
            if (string.IsNullOrEmpty(Request["noMobile"]) && MobileDetection.IsMobileBrowser())
            {
                Response.Redirect(WebConfigurationManager.AppSettings["UrlDeVersionMobile"], true);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                txtUsername.Text = Page.User.Identity.Name;
            }
        }

        [DirectMethod(ShowMask=true)]
        public void LoginClick()
        {
            try
            {
                string email = txtUsername.Text;
                string password = txtPassword.Text;
                bool recordarme = chkRecordarme.Checked;

                ManejadorDeMiembros manejador = new ManejadorDeMiembros();
                manejador.IniciarSesion(email, password);
                CrearCookie(email, recordarme);
                RedireccionarAPaginaProtegida();
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Core.Constantes.Generales.nickNameDeLaApp, ex.Message).Show();
            }
            txtPassword.Clear();
        }

        [DirectMethod(ShowMask = true)]
        public void CambiarContrasenaClick()
        {
            try
            {
                string email = txtUsername.Text;
                string oldPassword = txtPassword.Text;
                string newPassword = txtNewPassword.Text;

                ManejadorDeMiembros manejador = new ManejadorDeMiembros();
                manejador.CambiarContrasena(email, oldPassword, newPassword);
                X.Msg.Alert(Core.Constantes.Generales.nickNameDeLaApp, Resources.Literales.ContrasenaCambiadaCorrectamente).Show();
                txtPassword.Focus();
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Core.Constantes.Generales.nickNameDeLaApp, ex.Message).Show();
            }

            wndCambiarContrasena.Hide();
            txtPassword.Clear();
            txtNewPassword.Clear();
        }

        private void CrearCookie(string email, bool persistirCookie)
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(email, persistirCookie);
        }

        private void RedireccionarAPaginaProtegida()
        {
            string returnUrl = Request["ReturnUrl"];
            string mainUrl = ResolveUrl("~/");
            X.AddScript(string.Format("Ext.net.Mask.show({{ el: Ext.getBody() }}); window.location.replace('{0}'); ", (string.IsNullOrEmpty(returnUrl) ? mainUrl : returnUrl)));
        }

    }
}