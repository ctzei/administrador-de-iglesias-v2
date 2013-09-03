using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using ExtensionMethods;
using Ext.Net;
using LinqKit;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Enums;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;

using ZagueEF.Core.Web.ExtNET.Controls;

namespace AdministradorDeIglesiasV2.Website.Paginas.UserControls
{
    public partial class Buscador : System.Web.UI.UserControl
    {
        #region Propiedades

        public ZGridPanel GridDeListadoDeObjetos { get { return this.gridDeListadoDeObjetos; } }
        public ZGridPanel GridDeObjetosEncontrados { get { return this.gridDeObjetosEncontrados; } }
        public Ext.Net.Store SListadoDeObjetos { get { return this.StoreListadoDeObjetos; } }
        public Ext.Net.Store SObjetosEncontrados { get { return this.StoreObjetosEncontrados; } }

        private ManejadorDeBusquedas.TipoDeObjeto tipoDeBusqueda = ManejadorDeBusquedas.TipoDeObjeto.Celula;
        private string titulo = string.Empty;
        private int altura = 0;
        private TipoDeLista tipoDeLista = TipoDeLista.Multiple;

        [DefaultValue(typeof(ManejadorDeBusquedas.TipoDeObjeto), "Celula"), DesignOnly(true), Description("Determina los tipos de elementos que se desean buscar.")]
        public ManejadorDeBusquedas.TipoDeObjeto TipoDeBusqueda { get { return tipoDeBusqueda; } set { tipoDeBusqueda = value; } }

        [DefaultValue(""), DesignOnly(true), Description("Titulo del grid contenedor de los elementos seleccionados.")]
        public string Titulo { get { return titulo; } set { titulo = value; } }

        [DefaultValue(0), DesignOnly(true), Description("Altura del grid contenedor de los elementos seleccionados.")]
        public int Altura { get { return altura; } set { altura = value; } }

        [DefaultValue(false), DesignOnly(true), Description("Determina si solo un registro podra ser seleccionado/agregado al listado.")]
        public TipoDeLista TipoDeLista { get { return tipoDeLista; } set { tipoDeLista = value; } }

        #endregion

        protected override void OnPreRender(EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.titulo))
            {
                this.gridDeListadoDeObjetos.Title = titulo;
            }

            if (this.altura > 0)
            {
                this.gridDeListadoDeObjetos.Height = altura;
            }

            if (this.tipoDeLista == Core.Enums.TipoDeLista.Simple)
            {
                establecerTipoDeListaSimple();
            }
            base.OnPreRender(e);
        }

        protected void cmdBuscarConcepto_Click(object sender, DirectEventArgs e)
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();
            int usuarioId = SesionActual.Instance.UsuarioId;
            List<int> idsCelulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(usuarioId);
            List<int> idsCelulasSinLider = manejadorCelulas.ObtenerCelulasSinLideresComoCelulas().Select(o => o.CelulaId).ToList<int>();

            //Obtemenos todos los conceptos a buscar (separados por espacios)
            string[] conceptosABuscar = registroConceptoABuscar.Text.Trim().Split(' ');

            //Guarda los resultados obtenidos de las busquedas
            IQueryable<object> resultados = null;

            switch (tipoDeBusqueda)
            {
                case ManejadorDeBusquedas.TipoDeObjeto.Celula:
                    {
                        var busqueda = Celula.BuscarV1(conceptosABuscar, idsCelulasPermitidas.Union(idsCelulasSinLider).ToList<int>());  //Dentro de las celulas permitidas para el usuario actual... o aquellas celulas sin lider asignado (para poderlas ASGINAR)
                        resultados = ((
                        from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula.Where(busqueda)
                        orderby
                            o.Descripcion
                        select new
                        {
                            Id = o.CelulaId,
                            Descripcion = o.Descripcion,
                            RowColor = idsCelulasSinLider.Contains(o.CelulaId) ? "red" : string.Empty
                        })).AsExpandable();
                        break;
                    }

                case ManejadorDeBusquedas.TipoDeObjeto.Miembro:
                {
                    var busqueda = Miembro.BuscarV1(conceptosABuscar, idsCelulasPermitidas.ToList<int>());  //Dentro de las celulas permitidas para el usuario actual... o el mismo usuario actual
                    resultados = ((
                    from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro.Where(busqueda)
                    orderby
                        o.Primer_Nombre, o.Segundo_Nombre, o.Apellido_Paterno, o.Apellido_Materno
                    select new
                    {
                        Id = o.MiembroId,
                        Descripcion = o.Primer_Nombre + " " + o.Segundo_Nombre + " " + o.Apellido_Paterno + " " + o.Apellido_Materno + " (" + o.Email + ")"
                    })).AsExpandable();
                    break;
                }         
            }

            if (resultados != null)
            {
                int numeroDeResultadosMax = 75; //Es el numero maximo de resultados a regresar al cliente...
                int numeroDeResultados = resultados.Count();
                if (numeroDeResultados > numeroDeResultadosMax) {
                    X.Msg.Alert(Generales.nickNameDeLaApp, string.Format(Resources.Literales.LimiteDeResultadosExcedido, numeroDeResultados, numeroDeResultadosMax)).Show();
                    numeroDeResultados = numeroDeResultadosMax;
                }
                StoreObjetosEncontrados.Cargar(resultados.Take(numeroDeResultadosMax));
                registroNumeroDeResultados.Text = string.Format("{0} Resultados", numeroDeResultados);
            }

            //Ponemos el foco el el grid...
            GridDeListadoDeObjetos.Focus();
        }

        private void establecerTipoDeListaSimple()
        {
            X.AddScript(string.Format("{0}.registroSimple = true;", this.GridDeListadoDeObjetos.ClientID));

            if (!string.IsNullOrEmpty(this.titulo))
            {
                this.gridDeListadoDeObjetos.Height = 120;
            }
            else
            {
                this.gridDeListadoDeObjetos.Height = 65;
            }
        }

        /// <summary>
        /// Limpia la lista de las entidades "seleccionadas"
        /// </summary>
        public void LimpiarControles()
        {
            this.StoreListadoDeObjetos.RemoveAll();
            this.StoreObjetosEncontrados.RemoveAll();
            this.registroConceptoABuscar.Text = string.Empty;
        }

        /// <summary>
        /// Carga la lista de entidades al grid de datos "seleccionados"
        /// </summary>
        /// <param name="entidades"></param>
        public void CargarListado(object entidades)
        {
            this.SListadoDeObjetos.Cargar(entidades);
        }
    }
}