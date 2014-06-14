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

namespace AdministradorDeIglesiasV2.Website.Paginas
{
    public partial class Main : PaginaBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                establecerBienvenida();
                generarArbolDeNavegacion();
                mostrarAnunciosYAlertas();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void CerrarSesionActual()
        {
            this.TerminarSesion();
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path));
        }

        private void establecerBienvenida()
        {
            Miembro miembro = (from m in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where m.MiembroId == SesionActual.Instance.UsuarioId select m).SingleOrDefault();
            this.Title += string.Format("{0} - ({1} {2})", AdministradorDeIglesiasV2.Core.Constantes.Generales.nickNameDeLaApp, miembro.Primer_Nombre, miembro.Apellido_Paterno);
        }

        private void generarArbolDeNavegacion()
        {
            int usuarioId = SesionActual.Instance.UsuarioId;
            string appId = "WebV2";

            ManejadorDeMiembros manejador = new ManejadorDeMiembros();
            List<PantallaPermitida> pantallasPermitidasPorRoles = manejador.ObtenerPantallasPermitidasPorMiembro(usuarioId, appId);

            Dictionary<string, Ext.Net.TreeNode> nodos = new Dictionary<string, Ext.Net.TreeNode>();
            Ext.Net.TreeNode navegacion = new Ext.Net.TreeNode("Navegación");
            Ext.Net.TreeNode nodoCategoria = new Ext.Net.TreeNode();
            Ext.Net.TreeNode nodoPantalla;
            string categoria;
            string categoriaKey;
            string[] subcategorias;

            foreach (PantallaPermitida p in pantallasPermitidasPorRoles)
            {
                if (p.Categoria.Trim().Length > 0)
                {
                    subcategorias = p.Categoria.Split('/');
                    for (int i = 0; i < subcategorias.Length; i++)
                    {
                        categoria = subcategorias[i];
                        categoriaKey = string.Join("/", subcategorias, 0, i + 1);

                        if (nodos.ContainsKey(categoriaKey))
                        {
                            nodoCategoria = nodos[categoriaKey];
                        }
                        else
                        {
                            nodoCategoria = new Ext.Net.TreeNode(categoria);
                            nodoCategoria.Href = "javascript:void(0);";
                            nodos[categoriaKey] = nodoCategoria;

                            if (i == 0)
                            {
                                navegacion.Nodes.Add(nodoCategoria);
                            }
                            else
                            {
                                nodos[string.Join("/", subcategorias, 0, i)].Nodes.Add(nodoCategoria);
                            }
                        }

                    }

                    nodoPantalla = new Ext.Net.TreeNode(p.Nombre);
                    nodoPantalla.Href = "javascript:void(0);";
                    nodoPantalla.Listeners.Click.Handler = string.Format("cargarPantalla('{0}','{1}');", p.Nombre, ResolveUrl("~/Paginas/" + p.Categoria + "/" + p.Nombre_Tecnico));
                    nodoCategoria.Nodes.Add(nodoPantalla);
                }
            }

            pnlPantallas.Root.Clear();
            pnlPantallas.Root.Add(navegacion);
        }

        private void mostrarAnunciosYAlertas()
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();
            int miembroId = SesionActual.Instance.UsuarioId;
            List<int> idsCelulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(SesionActual.Instance.UsuarioId);
            DateTime fechaInicioDeSemana = DateTime.Now.GetFirstDateOfWeek();
            DateTime fechaFinDeSemana = DateTime.Now.GetLastDateOfWeek();
            IglesiaEntities contexto = SesionActual.Instance.getContexto<IglesiaEntities>();

            // Obtenemos las boletas de consolidacion sin reportes en esta semana
            int numDeBoletasSinReporteSemanal = (from o in contexto.ConsolidacionBoleta
                                                 where
                                                 o.Borrado == false &&
                                                 o.BoletaEstatusId > 100 &&
                                                 o.AsignadaACelulaId != null &&
                                                 idsCelulasPermitidas.Contains(o.AsignadaACelulaId.Value) &&
                                                 !contexto.ConsolidacionBoletaReporte.Any(t =>
                                                     t.Borrado == false &&
                                                     t.ConsolidacionBoletaId == o.Id &&
                                                     t.Creacion >= fechaInicioDeSemana &&
                                                     t.Creacion <= fechaFinDeSemana)
                                                 select o).Count();

            if (numDeBoletasSinReporteSemanal > 0)
            {
                generarAlerta(string.Format("Tienes {0} boleta(s) de consolidación sin reportar esta semana!", numDeBoletasSinReporteSemanal));
            }

            // Obtenemos las celulas directas que no tienen asistencia registrada esta semana
            int numDeCelulasSinAsistenciaSemanal = (from o in contexto.Celula
                                                    where
                                                    o.Borrado == false &&
                                                    contexto.CelulaLider.Any(t =>
                                                        t.Borrado == false &&
                                                        t.CelulaId == o.CelulaId &&
                                                        t.MiembroId == miembroId
                                                    ) &&
                                                    !contexto.CelulaMiembroAsistencia.Any(t =>
                                                       t.CelulaId == o.CelulaId &&
                                                       t.Dia >= fechaInicioDeSemana.Day &&
                                                       t.Mes >= fechaInicioDeSemana.Month &&
                                                       t.Anio >= fechaInicioDeSemana.Year &&
                                                       t.Dia <= fechaFinDeSemana.Day &&
                                                       t.Mes <= fechaFinDeSemana.Month &&
                                                       t.Anio <= fechaFinDeSemana.Year
                                                    ) &&
                                                    !contexto.CelulaCancelacionAsistencia.Any(t =>
                                                        t.CelulaId == o.CelulaId &&
                                                       t.Dia >= fechaInicioDeSemana.Day &&
                                                       t.Mes >= fechaInicioDeSemana.Month &&
                                                       t.Anio >= fechaInicioDeSemana.Year &&
                                                       t.Dia <= fechaFinDeSemana.Day &&
                                                       t.Mes <= fechaFinDeSemana.Month &&
                                                       t.Anio <= fechaFinDeSemana.Year
                                                       )
                                                    select o).Count();

            if (numDeCelulasSinAsistenciaSemanal > 0)
            {
                generarAlerta("Esta semana no has registrado asistencia en todas tus células!");
            }
        }

        private void generarAlerta(string mensaje)
        {
            NotificationConfig config = new NotificationConfig();
            config.Title = Core.Constantes.Generales.nickNameDeLaApp;
            config.Html = mensaje;
            config.Height = 75;
            config.Width = 300;
            config.HideDelay = 10000;
            config.BodyStyle = "padding:10px";
            config.Icon = Icon.UserAlert;
            config.PinEvent = "mouseover";

            SlideIn show = new SlideIn();
            show.Anchor = AnchorPoint.Right;

            SlideOut hide = new SlideOut();
            show.Anchor = AnchorPoint.Right;

            config.ShowFx = show;
            config.HideFx = hide;
            X.Msg.Notify(config).Show();
        }
    }
}