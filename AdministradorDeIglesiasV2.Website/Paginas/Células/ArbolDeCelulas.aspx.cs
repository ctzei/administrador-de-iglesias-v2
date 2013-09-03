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
    public partial class ArbolDeCelulas : PaginaBase
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
            ManejadorDeCelulas manejador = new ManejadorDeCelulas();
            StoreCelulas.Cargar(manejador.ObtenerRedesPermitidasPorMiembro(SesionActual.Instance.UsuarioId));
        }

        [DirectMethod(ShowMask = true)]
        public string CargarClick()
        {
            string rtn;
            int celulaSeleccionada;
            if (int.TryParse(cboCelula.SelectedItem.Value, out celulaSeleccionada))
            {
                int usuarioId = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider where ((o.CelulaId == celulaSeleccionada) &&( o.Borrado == false)) select o.MiembroId).FirstOrDefault();
                ManejadorDeCelulas manejadorCelulasWeb = new ManejadorDeCelulas();
                rtn = this.GenerarArbolDeCelulas(usuarioId, ThreeStateBool.Undefined, false).Nodes.ToJson();
            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, Resources.Literales.CelulaYFechaNecesarias).Show();
                rtn = string.Empty;
            }
            return rtn;
        }

        #region Generar Arbol de Celulas (ToDo: MOVER A MANEJADOR)

        private string atributoDeSoloLectura = "READONLY";

        /// <summary>
        /// Esta funcion se encarga de generar un arbol de las celulas disponibles para dicho usuario, incluidas a las cuales es lider, lider del lider y la misma que el usuario mismo asiste
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="mostrarCheckboxes"></param>
        /// <returns></returns>
        public Ext.Net.TreeNode GenerarArbolDeCelulas(int usuarioId, ThreeStateBool mostrarCheckboxes, bool expandido)
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();

            List<Celula> celulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoCelulas(usuarioId);
            TreeData.TreeDataTableDataTable dt = new TreeData.TreeDataTableDataTable();

            //Agregamos la celula a la que el miembro asiste, aun y cuando sera de solo lectura; solamente si no existia ya en la lista de celulas permitidas (es decir, asiste y es lider a la vez)
            Miembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.MiembroId == usuarioId select o).SingleOrDefault();
            if (!(celulasPermitidas.Any(o => o.CelulaId == miembro.CelulaId)))
            {
                dt.AddTreeDataTableRow(miembro.CelulaId.ToString(), string.Empty, miembro.Celula.Descripcion, atributoDeSoloLectura);
            }

            string celulaId;
            string miembroCelulaId;
            string atributoExtra;
            foreach (Celula c in celulasPermitidas)
            {
                celulaId = c.CelulaId.ToString();
                if (c.CelulaLider.Count > 0)
                {
                    CelulaLider celulaLider = c.CelulaLider.FirstOrDefault(o => o.Borrado == false && o.Miembro.Borrado == false);
                    if (celulaLider != null)
                    {
                        miembroCelulaId = celulaLider.Miembro.CelulaId.ToString();
                        miembroCelulaId = (celulaId.Equals(miembroCelulaId)) ? string.Empty : miembroCelulaId; //Evitamos un loop infinito al momento en que el miembro sea lider de la celula a la que asiste
                        atributoExtra = (string.IsNullOrEmpty(miembroCelulaId)) ? atributoDeSoloLectura : string.Empty;
                        dt.AddTreeDataTableRow(celulaId, miembroCelulaId, c.Descripcion, atributoExtra);
                    }
                }
            }

            //Obtenemos las celulas sin "lideres"
            List<Celula> celulasSinLider = manejadorCelulas.ObtenerCelulasSinLideresComoCelulas();
            if (celulasSinLider.Count > 0)
            {
                int idNodoCelulasSinLideres = 9999999;
                dt.AddTreeDataTableRow(idNodoCelulasSinLideres.ToString(), miembro.CelulaId.ToString(), "Celulas sin Asignacion de Lider", atributoDeSoloLectura);
                foreach (Celula c in celulasSinLider)
                {
                    dt.AddTreeDataTableRow(c.CelulaId.ToString(), idNodoCelulasSinLideres.ToString(), c.Descripcion, string.Empty);
                }
            }

            Ext.Net.TreeNode navegacion = new Ext.Net.TreeNode("Celulas");
            navegacion.Expanded = expandido;
            if ((celulasPermitidas != null) && (celulasPermitidas.Count > 0))
            {
                generarNodos(navegacion, TreeBuilder.GenerateTree(miembro.CelulaId.ToString(), dt), mostrarCheckboxes, expandido); //La celula inicial es la celula a la que el usuario asiste
            }

            return navegacion;
        }

        private void generarNodos(Ext.Net.TreeNode navegacion, System.Xml.XmlNode nodo, ThreeStateBool mostrarCheckboxes, bool expandido)
        {
            Ext.Net.TreeNode nodoHijo;
            foreach (System.Xml.XmlNode n in nodo.ChildNodes)
            {
                nodoHijo = new Ext.Net.TreeNode(n.Attributes["nodeDescription"].Value);
                nodoHijo.Checked = mostrarCheckboxes;
                nodoHijo.NodeID = n.Attributes["nodeID"].Value;
                nodoHijo.Expanded = expandido;

                if (n.Attributes["nodeNote"].Value == atributoDeSoloLectura)
                {
                    nodoHijo.Editable = false;
                    nodoHijo.Disabled = true;
                }

                navegacion.Nodes.Add(nodoHijo);

                if (n.HasChildNodes)
                {
                    generarNodos(nodoHijo, n, mostrarCheckboxes, expandido);
                }
            }
        }

        #endregion

    }
}