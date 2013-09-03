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
    public partial class CatalogoDeBoletas : PaginaBase, ICatalogo
    {
        void ICatalogo.CargarControles()
        {
            StoreGeneros.Cargar(Genero.Obtener());
            StoreEstadosCiviles.Cargar(EstadoCivil.Obtener());
            StoreCultos.Cargar(Culto.Obtener());
            StoreRazonesDeVisita.Cargar(ConsolidacionBoletaRazonVisita.Obtener());
            StoreRazonesParaCerrar.Cargar(ConsolidacionBoletaRazonCerrada.Obtener());
            StoreCategorias.Cargar(ConsolidacionBoletaCategoria.Obtener());
            StoreReportes.Limpiar();

            if (SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.VerTodasLasBoletasDeConsolidacion, false))
            {
                filtroFechaDeCulto.Value = DateTime.Now.PreviousSunday();
                registroFechaDeCulto.Value = DateTime.Now.PreviousSunday();
            }
        }

        void ICatalogo.Buscar()
        {
            List<int> idsGenero = filtroGenero.ObtenerIds();
            int? invitadoPorMiembroId = filtroInvitadoPorMiembro.ObtenerId(true);
            List<int> idsCulto = filtroCulto.ObtenerIds();
            List<int> idsRazonDeVisita = filtroRazonDeVisita.ObtenerIds();
            List<int> idsRazonParaCerrar = filtroRazonParaCerrar.ObtenerIds();
            List<int> idsCategorias = filtroCategoria.ObtenerIds();
            int idMunicipio = filtroMunicipio.ObtenerId();
            List<int> idsEstadoCivil = filtroEstadoCivil.ObtenerIds();
            int? asignadaACelulaId = filtroCelulaAsignada.ObtenerId(true);
            int? asignadaAMiembroId = filtroMiembroAsignada.ObtenerId(true);
            bool verTodasLasBoletasDeConsolidacion = SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.VerTodasLasBoletasDeConsolidacion, false);

            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();
            List<int> idsCelulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(SesionActual.Instance.UsuarioId);

            var query = (
                from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta
                where
                    (o.Email.Contains(filtroEmail.Text)) &&
                    (o.PrimerNombre.Contains(filtroPrimerNombre.Text)) &&
                    (o.SegundoNombre.Contains(filtroSegundoNombre.Text)) &&
                    (o.ApellidoPaterno.Contains(filtroApellidoPaterno.Text)) &&
                    (o.ApellidoMaterno.Contains(filtroApellidoMaterno.Text)) &&
                    (idsGenero.Contains(o.GeneroId) || (idsGenero.Count == 0)) &&
                    ((o.InvitadoPorMiembroId == invitadoPorMiembroId) || (invitadoPorMiembroId == null)) &&
                    (idsCulto.Contains(o.CultoId) || (idsCulto.Count == 0)) &&
                    (idsRazonDeVisita.Contains(o.BoletaRazonVisitaId) || (idsRazonDeVisita.Count == 0)) &&
                    (o.BoletaCerrada == filtroBoletaCerrada.Checked) &&
                    (idsRazonParaCerrar.Contains(o.BoletaRazonCerradaId.Value) || (idsRazonParaCerrar.Count == 0)) &&
                    (idsCategorias.Contains(o.CategoriaBoletaId) || (idsCategorias.Count == 0)) &&
                    (o.UbicacionMunicipioId == (idMunicipio > 0 ? idMunicipio : o.UbicacionMunicipioId)) &&
                    ((o.Colonia.Contains(filtroColonia.Text)) || (o.Colonia == null)) &&
                    ((o.Direccion.Contains(filtroDireccion.Text)) || (o.Direccion == null)) &&
                    (idsEstadoCivil.Contains(o.EstadoCivilId) || (idsEstadoCivil.Count == 0)) &&
                    ((o.AsignadaACelulaId == asignadaACelulaId) || (asignadaACelulaId == null)) &&
                    ((o.AsignadaAMiembroId == asignadaAMiembroId) || (asignadaAMiembroId == null)) &&
                    (((o.TelefonoCasa.Contains(filtroTel.Text)) || (o.TelefonoCasa == null)) ||
                    ((o.TelefonoMovil.Contains(filtroTel.Text)) || (o.TelefonoMovil == null)) ||
                    ((o.TelefonoTrabajo.Contains(filtroTel.Text)) || (o.TelefonoTrabajo == null)) &&
                    ((o.Observaciones.Contains(filtroObservaciones.Text)) || (o.Observaciones == null))) &&
                    ((verTodasLasBoletasDeConsolidacion == true) || (idsCelulasPermitidas.Contains(o.AsignadaACelulaId.Value) || o.AsignadaAMiembroId.Value == SesionActual.Instance.UsuarioId)) &&
                    (o.Borrado == false) //Registros NO borrados
                orderby o.PrimerNombre, o.SegundoNombre, o.ApellidoPaterno, o.ApellidoMaterno
                select new
                {
                    Id = o.Id,
                    Email = o.Email,
                    PrimerNombre = o.PrimerNombre,
                    SegundoNombre = o.SegundoNombre,
                    ApellidoPaterno = o.ApellidoPaterno,
                    ApellidoMaterno = o.ApellidoMaterno,
                    Genero = o.Genero.Descripcion,
                    InvitadoPorMiembroId = o.InvitadoPorMiembroId,
                    Culto = o.Culto.Descripcion,
                    FechaDeCulto = o.FechaDeCulto,
                    RazonDeVisita = o.ConsolidacionBoletaRazonVisita.Descripcion,
                    Cerrada = o.BoletaCerrada,
                    RazonDeCerrar = o.ConsolidacionBoletaRazonCerrada.Descripcion,
                    Municipio = o.UbicacionMunicipio.Descripcion,
                    Colonia = o.Colonia,
                    Direccion = o.Direccion,
                    Nacimiento = o.FechaDeNacimiento,
                    Edad = o.Edad,
                    EstadoCivil = o.EstadoCivil.Descripcion,
                    AsignadaACelulaId = o.AsignadaACelulaId,
                    AsignadaAMiembroId = o.AsignadaAMiembroId,
                    TelCasa = o.TelefonoCasa,
                    TelMovil = o.TelefonoMovil,
                    TelTrabajo = o.TelefonoTrabajo,
                    Observaciones = o.Observaciones,
                    Categoria = o.ConsolidacionBoletaCategoria.Descripcion
                });

            //*** Filtros complejos ***//

            if (filtroFechaDeCulto.SelectedValue != null)
            {
                query = query.Where(o => o.FechaDeCulto == filtroFechaDeCulto.SelectedDate);
            }

            if (filtroEdad.Value != null)
            {
                int edad = Convert.ToInt32(filtroEdad.Number);
                query = query.Where(o => o.Edad == edad);
            }

            StoreResultados.Cargar(query);
            registroNumeroDeBoletas.Value = string.Format("Total de boletas: {0}", query.Count());
        }

        void ICatalogo.Mostrar(int id)
        {
            ConsolidacionBoleta entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Id == id select o).FirstOrDefault();
            registroId.Value = entidad.Id.ToString();
            registroEmail.Value = entidad.Email;
            registroPrimerNombre.Value = entidad.PrimerNombre;
            registroSegundoNombre.Value = entidad.SegundoNombre;
            registroApellidoPaterno.Value = entidad.ApellidoPaterno;
            registroApellidoMaterno.Value = entidad.ApellidoMaterno;
            registroGenero.Value = entidad.GeneroId;
            registroInvitadoPorMiembro.EstablecerId(entidad.InvitadoPorMiembroId);
            registroCulto.Value = entidad.CultoId;
            registroFechaDeCulto.Value = entidad.FechaDeCulto;
            registroRazonDeVisita.Value = entidad.BoletaRazonVisitaId;
            registroBoletaCerrada.Checked = entidad.BoletaCerrada;
            registroRazonParaCerrar.Value = entidad.BoletaRazonCerradaId;
            registroCategoria.Value = entidad.CategoriaBoletaId;
            registroMunicipio.ForzarSeleccion(entidad.UbicacionMunicipioId, entidad.UbicacionMunicipio.Descripcion);
            registroColonia.Value = entidad.Colonia;
            registroDireccion.Value = entidad.Direccion;
            registroFechaDeNacimiento.Value = entidad.FechaDeNacimiento;
            registroEdad.Value = entidad.Edad;
            registroEstadoCivil.Value = entidad.EstadoCivilId;
            registroCelulaAsignada.EstablecerId(entidad.AsignadaACelulaId);
            registroMiembroAsignada.EstablecerId(entidad.AsignadaAMiembroId);
            registroTelCasa.Value = entidad.TelefonoCasa;
            registroTelMovil.Value = entidad.TelefonoMovil;
            registroTelTrabajo.Value = entidad.TelefonoTrabajo;
            registroObservaciones.Value = entidad.Observaciones;

            StoreReportes.Cargar(from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoletaReporte
                                 join miembro in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro on o.CreacionId equals miembro.MiembroId
                                 orderby o.Id descending
                                 where o.ConsolidacionBoletaId == id && o.Borrado == false
                                 select new
                                 {
                                     Id = o.Id,
                                     Reporte = o.Reporte,
                                     Fecha = o.Creacion,
                                     ReportadoPorMiembro = miembro.Primer_Nombre + " " + miembro.Segundo_Nombre + " " + miembro.Apellido_Paterno + " " + miembro.Apellido_Materno + " (" + miembro.Email + ")"
                                 });
        }

        void ICatalogo.Borrar(int id)
        {
            ConsolidacionBoleta entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Id == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            ConsolidacionBoleta entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().ConsolidacionBoleta where o.Id == id select o).FirstOrDefault();

            if (entidad == null)
            {
                entidad = new ConsolidacionBoleta();
            }

            entidad.Email = registroEmail.Text;
            entidad.PrimerNombre = registroPrimerNombre.Text;
            entidad.SegundoNombre = registroSegundoNombre.Text;
            entidad.ApellidoPaterno = registroApellidoPaterno.Text;
            entidad.ApellidoMaterno = registroApellidoMaterno.Text;
            entidad.GeneroId = registroGenero.ObtenerId();
            entidad.InvitadoPorMiembroId = registroInvitadoPorMiembro.ObtenerId(true);
            entidad.CultoId = registroCulto.ObtenerId();
            entidad.FechaDeCulto = registroFechaDeCulto.SelectedDate;
            entidad.BoletaRazonVisitaId = registroRazonDeVisita.ObtenerId();
            entidad.BoletaCerrada = registroBoletaCerrada.Checked;
            entidad.BoletaRazonCerradaId = registroRazonParaCerrar.ObtenerId(true);
            entidad.UbicacionMunicipioId = registroMunicipio.ObtenerId();
            entidad.Colonia = registroColonia.Text;
            entidad.Direccion = registroDireccion.Text;
            entidad.EstadoCivilId = registroEstadoCivil.ObtenerId();
            entidad.AsignadaACelulaId = registroCelulaAsignada.ObtenerId(true);
            entidad.AsignadaAMiembroId = registroMiembroAsignada.ObtenerId(true);
            entidad.TelefonoCasa = registroTelCasa.Text;
            entidad.TelefonoMovil = registroTelMovil.Text;
            entidad.TelefonoTrabajo = registroTelTrabajo.Text;
            entidad.Observaciones = registroObservaciones.Text;

            if (registroFechaDeNacimiento.SelectedValue != null)
            {
                entidad.FechaDeNacimiento = registroFechaDeNacimiento.SelectedDate;
            }
            else
            {
                entidad.FechaDeNacimiento = null;
            }

            if (registroEdad.Value != null)
            {
                entidad.Edad = Convert.ToInt32(registroEdad.Number);
            }
            else
            {
                entidad.Edad = null;
            }

            entidad.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

            // Si la boleta fue cerrada correctamente...
            if (entidad.BoletaCerrada == true)
            {
                Miembro miembroYaExistente = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro where o.Email == entidad.Email select o).SingleOrDefault();
                if (miembroYaExistente != null)
                {
                    X.Msg.Alert(Generales.nickNameDeLaApp, string.Format("La boleta ha sido cerrada correctamente. El miembro ya se encuentra asignado a la célula: {0} [{1}]", miembroYaExistente.Celula.Descripcion, miembroYaExistente.CelulaId)).Show();
                }
                else
                {
                    registroBoletaId.Text = registroId.Text;

                    // Preseleccionamos la celula a la que se habia asignado la boleta
                    if (entidad.AsignadaACelulaId.HasValue)
                    {
                        registroCelulaDeMiembroCreado.EstablecerId(entidad.AsignadaACelulaId.Value);
                    }
                    else
                    {
                        registroCelulaDeMiembroCreado.Limpiar();
                    }

                    // Mostramos el mensaje para "asignar" la boleta a alguna celula preexistente
                    X.AddScript(string.Format("{0}.show();", wndCrearMiembro.ClientID));
                }
            }
        }

        [DirectMethod(ShowMask = true)]
        public void AgregarReporte()
        {
            try
            {
                int boletaId = Convert.ToInt32(registroId.Number);
                wndAgregarReporte.Hide();

                if (boletaId > 0)
                {
                    ConsolidacionBoletaReporte reporte = new ConsolidacionBoletaReporte();
                    reporte.ConsolidacionBoletaId = boletaId;
                    reporte.Reporte = registroReporteNuevo.Text.Trim();
                    reporte.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                    registroReporteNuevo.Text = string.Empty;
                    ((ICatalogo)this).Mostrar(boletaId);
                    X.Msg.Notify(Generales.nickNameDeLaApp, "Reporte agregadado correctamente").Show();
                }
                else
                {
                    throw new ExcepcionReglaNegocio("Favor de seleccionar alguna boleta válida.");
                }

            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void CrearMiembro()
        {
            try
            {
                int boletaId;
                if (!int.TryParse(registroBoletaId.Text, out boletaId)){
                    boletaId = -1;
                }

                if (boletaId > 0)
                {
                    wndCrearMiembro.Hide();
                    ManejadorDeConsolidacion manejador = new ManejadorDeConsolidacion();
                    Miembro miembro = manejador.CrearMiembroDesdeBoleta(boletaId, registroCelulaDeMiembroCreado.ObtenerId());
                    X.Msg.Alert(Generales.nickNameDeLaApp, string.Format("El miembro creado correctamente y asignado a la célula: {0} [{1}]", miembro.Celula.Descripcion, miembro.CelulaId)).Show();
                }
                else
                {
                    throw new ExcepcionReglaNegocio("Favor de seleccionar alguna boleta válida.");
                }

            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void CargarDatosDeBoletaAnterior()
        {
            ManejadorDeConsolidacion manejador = new ManejadorDeConsolidacion();
            ConsolidacionBoleta ultimaBoleta = manejador.ObtenerUltimaBoletaRegistrada();
            ((ICatalogo)this).Mostrar(ultimaBoleta.Id);

            registroId.Number = -1;
            registroEmail.Clear();
            registroPrimerNombre.Clear();
            registroSegundoNombre.Clear();
            registroApellidoPaterno.Clear();
            registroApellidoMaterno.Clear();
            registroGenero.Clear();
            registroFechaDeNacimiento.Clear();
            registroEstadoCivil.Clear();
            registroEdad.Clear();
            registroTelMovil.Clear();
            registroTelTrabajo.Clear();
        }


    }
}