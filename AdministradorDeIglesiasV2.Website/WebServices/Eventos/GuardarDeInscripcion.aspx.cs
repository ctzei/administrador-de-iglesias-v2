using System;
using System.Globalization;
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
using ZagueEF.Core.Web.ExtNET;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;


using System.Web.Caching;

namespace AdministradorDeIglesiasV2.Website.WebServices.Eventos
{
    public partial class GuardarDeInscripcion : JsonWebServiceBase
    {
        public override System.Data.Objects.DataClasses.EntityObject IniciarSesion()
        {
            //Iniciamos sesion del usuario especificado para SOLO LECTURA del sistema
            ManejadorDeMiembros manejador = new ManejadorDeMiembros();
            return manejador.IniciarSesion(WebConfigurationManager.AppSettings["EmailDeUsuarioDeSoloLectura"], WebConfigurationManager.AppSettings["PwdDeUsuarioDeSoloLectura"]);
        }

        public string guardarInscripcion()
        {
            bool success = true;
            string error = string.Empty;
            int folio = -1;
            List<string> invalidFields = new List<string>();

            //Parametros obtenidos del request
            string sEventoId = Request["evento"];
            string sEmail = Request["email"];
            string sPrimerNombre = Request["pnombre"];
            string sSegundoNombre = Request["snombre"];
            string sApellidoPaterno = Request["apaterno"];
            string sApellidoMaterno = Request["amaterno"];
            string sTel = Request["tel"];
            string sGeneroId = Request["genero"];
            string sMunicipioId = Request["municipio"];
            string sTipoDeRegistrante = Request["tregistrante"];
            string sInfoExtraDeRegistrante = Request["iregistrante"];
            string sReferenciaAlfanumerica = Request["ralfa"];
            string sReferenciaNumerica = Request["rnum"];
            string sFechaDeposito = Request["fecha"];
            string sCantidadDePersonas = Request["cant"];

            //Parametros convertidos y utilizados para crear la entidad
            int eventoId;
            int generoId;
            int municipioId;
            int tipoDeRegistranteId;
            int referenciaNumerica;
            DateTime fechaDeposito;
            int cantidadDePersonas;

            //Convertimos los valores de "strings" a sus respectivos tipos
            if (!int.TryParse(sEventoId, out eventoId)) {invalidFields.Add("evento");}
            if (!Validaciones.ValidarEmail(sEmail, false)) { invalidFields.Add("email"); }
            if (!int.TryParse(sGeneroId, out generoId)) { invalidFields.Add("genero"); }
            if (!int.TryParse(sMunicipioId, out municipioId)) { invalidFields.Add("municipio"); }
            if (!int.TryParse(sTipoDeRegistrante, out tipoDeRegistranteId)) { invalidFields.Add("tregistrante"); }
            if (!int.TryParse(sReferenciaNumerica, out referenciaNumerica)) {invalidFields.Add("rnum");}
            if (!DateTime.TryParse(sFechaDeposito, CultureInfo.CreateSpecificCulture("es-MX"), System.Globalization.DateTimeStyles.None, out fechaDeposito)) { invalidFields.Add("fecha"); }
            if (!int.TryParse(sCantidadDePersonas, out cantidadDePersonas)) { invalidFields.Add("cant"); }

            //Solo intentamos grabar la ficha si no hubo error al obtener los campos
            if (invalidFields.Count <= 0)
            {
                EventoFichaInscripcion inscripcion = new EventoFichaInscripcion();
                inscripcion.EventoId = eventoId;
                inscripcion.Email = sEmail;
                inscripcion.Primer_Nombre = sPrimerNombre;
                inscripcion.Segundo_Nombre = sSegundoNombre;
                inscripcion.Apellido_Paterno = sApellidoPaterno;
                inscripcion.Apellido_Materno = sApellidoMaterno;
                inscripcion.Tel = sTel;
                inscripcion.GeneroId = generoId;
                inscripcion.UbicacionMunicipioId = municipioId;
                inscripcion.TipoRegistranteId = tipoDeRegistranteId;
                inscripcion.InfoExtraRegistrante = sInfoExtraDeRegistrante;
                inscripcion.Referencia_Alfanumerica_Deposito = sReferenciaAlfanumerica;
                inscripcion.Referencia_Numerica_Deposito = referenciaNumerica;
                inscripcion.Fecha_Deposito = fechaDeposito;
                inscripcion.CantidadPersonas = cantidadDePersonas;
                inscripcion.Cerrada = false;

                try
                {
                    inscripcion.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    folio = inscripcion.Id;
                }
                catch (ExcepcionReglaNegocio ex)
                {
                    success = false;
                    error = ex.Message;
                }

            }
            else
            {
                success = false;
                error = "Cuando menos uno de los campos proporcionados tiene un valor invalido. Favor de volverlo a intentar.";
            }

            return (new { success = success, invalidFields = invalidFields, error = error, folio = folio }).ToJson();
        }

    }
}