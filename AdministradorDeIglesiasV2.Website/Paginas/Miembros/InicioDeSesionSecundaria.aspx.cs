using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using log4net;


namespace AdministradorDeIglesiasV2.Website.Paginas
{
    public partial class InicioDeSesionSecundaria : PaginaBase
    {
        private static readonly ILog log = LogManager.GetLogger("Usuario");

        [DirectMethod(ShowMask = true)]
        public bool IniciarSesionClick()
        {
            int miembroSecundarioId = registroMiembro.ObtenerId();
            if (miembroSecundarioId > 0)
            {
                ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();
                List<int> celulasPermitidas = manejadorDeCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(SesionActual.Instance.UsuarioId);

                Miembro miembroSecundario = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == miembroSecundarioId && o.Borrado == false select o).SingleOrDefault();
                if (miembroSecundario != null && celulasPermitidas.Contains(miembroSecundario.CelulaId))
                {
                    Miembro miembroActual = ManejadorDeMiembros.ObtenerMiembroActual();
                    log.InfoFormat("El usuario {0} [{1}] iniciara sesion como {2} [{3}]", miembroActual.Email, miembroActual.MiembroId, miembroSecundario.Email, miembroSecundario.MiembroId);

                    this.RestablecerCacheDeSesion();
                    ManejadorDeMiembros manejadorDeMiembros = new ManejadorDeMiembros();
                    manejadorDeMiembros.IniciarSesion(miembroSecundario.Email, string.Empty, true);
                    
                    return true;
                }
                else
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, "Miembro inexistente o no pertenece a la red").Show();
                }
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, "Es necesario establecer un miembro").Show();
            }
            return false;
        }

    }
}