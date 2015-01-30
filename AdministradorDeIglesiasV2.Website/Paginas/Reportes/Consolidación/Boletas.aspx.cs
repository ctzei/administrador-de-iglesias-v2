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
using AdministradorDeIglesiasV2.Core.Modelos.Retornos;
using AdministradorDeIglesiasV2.Core.Manejadores;
using System.Globalization;


namespace AdministradorDeIglesiasV2.Website.Paginas.Reportes.Consolidacion
{
    public partial class Boletas : PaginaBase
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
            dtpFechaInicial.SelectedDate = new DateTime(DateTime.Now.Year, 1, 1);
            dtpFechaFinal.SelectedDate = DateTime.Now;
        }

        [DirectMethod(ShowMask = true, Timeout = 120000)]
        public static string ObtenerBoletas(string sFechaInicial, string sFechaFinal)
        {
            DateTime fechaInicial;
            DateTime fechaFinal;

            if (DateTime.TryParse(sFechaInicial, out fechaInicial) == false || DateTime.TryParse(sFechaFinal, out fechaFinal) == false)
            {
                fechaInicial = DateTime.MinValue;
                fechaFinal = DateTime.MinValue;
            }

            if ((fechaInicial.Year > 1900 && fechaFinal.Year > 1900))
            {
                ManejadorDeCelulas manejadorDeCelulas = new ManejadorDeCelulas();

                // Pre-cargamos todas las celulas/miembros/redes para usarlos al momento de "modificar" el resultado a mostrar al usuario
                List<Celula> celulas = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.Borrado == false select o).ToList();
                List<Miembro> miembros = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.Borrado == false select o).ToList();
                Dictionary<Celula, List<int>> redes = manejadorDeCelulas.ObtenerRedes();

                var query = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta
                             where o.Creacion >= fechaInicial && o.Creacion <= fechaFinal && o.Borrado == false
                             select new
                             {
                                 o.AsignadaACelulaId,
                                 o.AsignadaAMiembroId,
                                 o.BoletaEstatusId,
                                 o.Creacion.Year,
                                 o.Creacion.Month
                             });

                String sinAsignacion = "(Sin Asignación)";

                // Executamos el query, y luego modificamos ciertos campos
                return query.AsEnumerable().Select(o => new
                {
                    Red = o.AsignadaACelulaId.HasValue ? redes.Where(p => p.Key.CelulaId == o.AsignadaACelulaId || p.Value.Contains(o.AsignadaACelulaId.Value) == true).Select(p => p.Key.Descripcion).SingleOrDefault() ?? sinAsignacion : sinAsignacion,
                    Celula = o.AsignadaACelulaId.HasValue ? celulas.Where(p => p.CelulaId == o.AsignadaACelulaId).Select(p => p.Descripcion).SingleOrDefault() ?? sinAsignacion : sinAsignacion,
                    Miembro = o.AsignadaAMiembroId.HasValue ? miembros.Where(p => p.MiembroId == o.AsignadaAMiembroId).Select(p => p.NombreCompleto).SingleOrDefault() ?? sinAsignacion : sinAsignacion,
                    Estatus = ConsolidacionBoleta.Estatus.Lista().Where(x => x.Key == o.BoletaEstatusId).FirstOrDefault().Value.Split('-')[0].Trim(),
                    SubEstatus = ConsolidacionBoleta.Estatus.Lista().Where(x => x.Key == o.BoletaEstatusId).FirstOrDefault().Value.Split('-')[1].Trim(),
                    Año = o.Year,
                    Mes = o.Month + " - " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(o.Month).ToUpper()
                }).ToList().ToJson();

            }
            else
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, "Es necesario seleccionar las fechas antes de continuar.").Show();
                return string.Empty;
            }
        }
    }
}