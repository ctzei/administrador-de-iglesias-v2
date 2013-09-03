using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using ExtensionMethods;
using Ext.Net;
using ZagueEF.Core;
using ZagueEF.Core.Web;
using ZagueEF.Core.Web.Interfaces;
using AdministradorDeIglesiasV2.Core;
using AdministradorDeIglesiasV2.Core.Constantes;
using AdministradorDeIglesiasV2.Core.Modelos;
using AdministradorDeIglesiasV2.Core.Manejadores;

using AdministradorDeIglesiasV2.Core.Enums;
using ZagueEF.Core.Web.ExtNET.Controls;

namespace AdministradorDeIglesiasV2.Website.Paginas.UserControls
{
    public partial class FiltrosMiembros : System.Web.UI.UserControl
    {
        #region Propiedades

        public ZNumberField FiltroId { get { return this.filtroId; } }
        public ZTextField FiltroPrimerNombre { get { return this.filtroPrimerNombre; } }
        public ZTextField FiltroSegundoNombre { get { return this.filtroSegundoNombre; } }
        public ZTextField FiltroApellidoPaterno { get { return this.filtroApellidoPaterno; } }
        public ZTextField FiltroApellidoMaterno { get { return this.filtroApellidoMaterno; } }
        public ZTextField FiltroEmail { get { return this.filtroEmail; } }
        public ZMultiCombo FiltroCelula { get { return this.filtroCelula; } }
        public ZMultiCombo FiltroEstadoCivil { get { return this.filtroEstadoCivil; } }
        public ZComboBoxMunicipio FiltroMunicipio { get { return this.filtroMunicipio; } }
        public ZTextField FiltroColonia { get { return this.filtroColonia; } }
        public ZTextField FiltroDireccion { get { return this.filtroDireccion; } }
        public ZNumberField FiltroTel { get { return this.filtroTel; } }
        public ZDateField FiltroFechaDeNacimiento { get { return this.filtroFechaDeNacimiento; } }

        public Ext.Net.Store SCelulas { get { return this.StoreCelulas; } }
        public Ext.Net.Store SEstadosCiviles { get { return this.StoreEstadosCiviles; } }

        private bool mostrarUsuarioActual = false;
        private TipoDeMiembroABuscar tipoDeMiembro = TipoDeMiembroABuscar.Miembro;

        [DefaultValue(false), DesignOnly(true), Description("Determina si se mostrara o no el usuario actual en la lista de miembros.")]
        public bool MostrarUsuarioActual { get { return mostrarUsuarioActual; } set { mostrarUsuarioActual = value; } }

        [DefaultValue(false), DesignOnly(true), Description("Determina los tipos de miembros que se desean buscar.")]
        public TipoDeMiembroABuscar TipoDeMiembro { get { return tipoDeMiembro; } set { tipoDeMiembro = value; } }

        #endregion

        public void CargarControles()
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();

            StoreCelulas.Cargar(manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoCelulas(SesionActual.Instance.UsuarioId));
            StoreEstadosCiviles.Cargar(EstadoCivil.Obtener());
            StoreGeneros.Cargar(Genero.Obtener());
        }

        public void BuscarMiembros()
        {
            ManejadorDeCelulas manejadorCelulas = new ManejadorDeCelulas();
            int usuarioId = SesionActual.Instance.UsuarioId;

            List<int> idsCelulas = filtroCelula.ObtenerIds();
            List<int> idGeneros = filtroGenero.ObtenerIds();
            List<int> idsEstadosCiviles = filtroEstadoCivil.ObtenerIds();
            int idMunicipio = filtroMunicipio.ObtenerId();
            List<int> idsCelulasPermitidas = manejadorCelulas.ObtenerCelulasPermitidasPorMiembroComoIds(usuarioId);
            List<int> idsCelulasSinLider = manejadorCelulas.ObtenerCelulasSinLideresComoIds();
            List<int> idsCelulasEliminadasConMiembros = manejadorCelulas.ObtenerCelulasEliminadasConMiembrosComoIds();
            bool registrosBorrados = filtroBorrado.Checked;
            bool registrosSinCelula = filtroSinCelula.Checked;

            //Guarda los resultados obtenidos de las busquedas
            IQueryable<object> resultados = null;

            switch (tipoDeMiembro)
            {
                case TipoDeMiembroABuscar.Miembro:
                    {
                        resultados = (
                            from o in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro
                            where
                                ((o.MiembroId == (filtroId.Number > 0 ? filtroId.Number : o.MiembroId)) &&
                                ((o.Primer_Nombre.Contains(filtroPrimerNombre.Text)) || (o.Primer_Nombre == null)) &&
                                ((o.Segundo_Nombre.Contains(filtroSegundoNombre.Text)) || (o.Segundo_Nombre == null)) &&
                                ((o.Apellido_Paterno.Contains(filtroApellidoPaterno.Text)) || (o.Apellido_Paterno == null)) &&
                                ((o.Apellido_Materno.Contains(filtroApellidoMaterno.Text)) || (o.Apellido_Materno == null)) &&
                                ((o.Email.Contains(filtroEmail.Text)) || (o.Email == null)) &&
                                (idsCelulas.Contains(o.CelulaId) || (idsCelulas.Count == 0)) &&
                                (idGeneros.Contains(o.GeneroId) || (idGeneros.Count == 0)) &&
                                (idsEstadosCiviles.Contains(o.EstadoCivilId) || (idsEstadosCiviles.Count == 0)) &&
                                (o.UbicacionMunicipioId == (idMunicipio > 0 ? idMunicipio : o.UbicacionMunicipioId)) &&
                                ((o.Colonia.Contains(filtroColonia.Text)) || (o.Colonia == null)) &&
                                ((o.Direccion.Contains(filtroDireccion.Text)) || (o.Direccion == null)) &&
                                (((o.Tel_Casa.Contains(filtroTel.Text)) || (o.Tel_Casa == null)) ||
                                ((o.Tel_Movil.Contains(filtroTel.Text)) || (o.Tel_Movil == null)) ||
                                ((o.Tel_Trabajo.Contains(filtroTel.Text)) || (o.Tel_Trabajo == null))) &&
                                (o.Fecha_Nacimiento == (filtroFechaDeNacimiento.SelectedDate > DateTime.MinValue ? filtroFechaDeNacimiento.SelectedDate : o.Fecha_Nacimiento)) &&
                                (o.Borrado == registrosBorrados) &&
                                (((idsCelulasPermitidas.Contains(o.CelulaId))  && !registrosSinCelula) || //Dentro de las celulas permitidas para el usuario actual
                                ((idsCelulasSinLider.Contains(o.CelulaId)) || (idsCelulasEliminadasConMiembros.Contains(o.CelulaId)) && registrosSinCelula) || //Dentro de los usuarios cuya celula no tiene lider
                                ((mostrarUsuarioActual) && (o.MiembroId == usuarioId)))) //El mismo usuario actual...
                            orderby o.Primer_Nombre ascending, o.Apellido_Paterno ascending
                            select new
                            {
                                GUID = o.MiembroId,
                                Id = o.MiembroId,
                                PrimerNombre = o.Primer_Nombre,
                                SegundoNombre = o.Segundo_Nombre,
                                ApellidoPaterno = o.Apellido_Paterno,
                                ApellidoMaterno = o.Apellido_Materno,
                                Email = o.Email,
                                Celula = (idsCelulasEliminadasConMiembros.Contains(o.CelulaId) ? string.Empty : o.Celula.Descripcion),
                                EstadoCivil = o.EstadoCivil.Descripcion,
                                Municipio = o.UbicacionMunicipio.Descripcion,
                                Colonia = o.Colonia,
                                TelCasa = o.Tel_Casa,
                                TelMovil = o.Tel_Movil,
                                TelTrabajo = o.Tel_Trabajo,
                                Nacimiento = o.Fecha_Nacimiento,
                                AsisteIglesia = o.AsisteIglesia,
                                Genero = o.Genero.Descripcion,
                                RowColor = (idsCelulasSinLider.Contains(o.CelulaId) || idsCelulasEliminadasConMiembros.Contains(o.CelulaId)) ? "red" : string.Empty
                            });
                        break;
                    }

                case TipoDeMiembroABuscar.ServidorCoordinador:
                    {
                        resultados = (
                            from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorCoordinador
                            where
                                ((o.Miembro.MiembroId == (filtroId.Number > 0 ? filtroId.Number : o.Miembro.MiembroId)) &&
                                ((o.Miembro.Primer_Nombre.Contains(filtroPrimerNombre.Text)) || (o.Miembro.Primer_Nombre == null)) &&
                                ((o.Miembro.Segundo_Nombre.Contains(filtroSegundoNombre.Text)) || (o.Miembro.Segundo_Nombre == null)) &&
                                ((o.Miembro.Apellido_Paterno.Contains(filtroApellidoPaterno.Text)) || (o.Miembro.Apellido_Paterno == null)) &&
                                ((o.Miembro.Apellido_Materno.Contains(filtroApellidoMaterno.Text)) || (o.Miembro.Apellido_Materno == null)) &&
                                ((o.Miembro.Email.Contains(filtroEmail.Text)) || (o.Miembro.Email == null)) &&
                                (idsCelulas.Contains(o.Miembro.CelulaId) || (idsCelulas.Count == 0)) &&
                                (idGeneros.Contains(o.Miembro.GeneroId) || (idGeneros.Count == 0)) &&
                                (idsEstadosCiviles.Contains(o.Miembro.EstadoCivilId) || (idsEstadosCiviles.Count == 0)) &&
                                (o.Miembro.UbicacionMunicipioId == (idMunicipio > 0 ? idMunicipio : o.Miembro.UbicacionMunicipioId)) &&
                                ((o.Miembro.Colonia.Contains(filtroColonia.Text)) || (o.Miembro.Colonia == null)) &&
                                ((o.Miembro.Direccion.Contains(filtroDireccion.Text)) || (o.Miembro.Direccion == null)) &&
                                (((o.Miembro.Tel_Casa.Contains(filtroTel.Text)) || (o.Miembro.Tel_Casa == null)) ||
                                ((o.Miembro.Tel_Movil.Contains(filtroTel.Text)) || (o.Miembro.Tel_Movil == null)) ||
                                ((o.Miembro.Tel_Trabajo.Contains(filtroTel.Text)) || (o.Miembro.Tel_Trabajo == null))) &&
                                (o.Miembro.Fecha_Nacimiento == (filtroFechaDeNacimiento.SelectedDate > DateTime.MinValue ? filtroFechaDeNacimiento.SelectedDate : o.Miembro.Fecha_Nacimiento)) &&
                                (o.Miembro.Borrado == registrosBorrados) && //Registros NO borrados
                                (o.Borrado == false)) //Registros NO borrados
                            orderby o.Miembro.Primer_Nombre ascending, o.Miembro.Apellido_Paterno ascending
                            select new
                            {
                                GUID = o.Id,
                                Id = o.Miembro.MiembroId,
                                PrimerNombre = o.Miembro.Primer_Nombre,
                                SegundoNombre = o.Miembro.Segundo_Nombre,
                                ApellidoPaterno = o.Miembro.Apellido_Paterno,
                                ApellidoMaterno = o.Miembro.Apellido_Materno,
                                Email = o.Miembro.Email,
                                Celula = o.Miembro.Celula.Descripcion,
                                EstadoCivil = o.Miembro.EstadoCivil.Descripcion,
                                Municipio = o.Miembro.UbicacionMunicipio.Descripcion,
                                Colonia = o.Miembro.Colonia,
                                TelCasa = o.Miembro.Tel_Casa,
                                TelMovil = o.Miembro.Tel_Movil,
                                TelTrabajo = o.Miembro.Tel_Trabajo,
                                Nacimiento = o.Miembro.Fecha_Nacimiento,
                                AsisteIglesia = o.Miembro.AsisteIglesia,
                                Genero = o.Miembro.Genero.Descripcion
                            });
                        break;
                    }

                case TipoDeMiembroABuscar.ServidorCapitan:
                    {
                        resultados = (
                            from o in SesionActual.Instance.getContexto<IglesiaEntities>().ServidorCapitan
                            where
                                ((o.Miembro.MiembroId == (filtroId.Number > 0 ? filtroId.Number : o.Miembro.MiembroId)) &&
                                ((o.Miembro.Primer_Nombre.Contains(filtroPrimerNombre.Text)) || (o.Miembro.Primer_Nombre == null)) &&
                                ((o.Miembro.Segundo_Nombre.Contains(filtroSegundoNombre.Text)) || (o.Miembro.Segundo_Nombre == null)) &&
                                ((o.Miembro.Apellido_Paterno.Contains(filtroApellidoPaterno.Text)) || (o.Miembro.Apellido_Paterno == null)) &&
                                ((o.Miembro.Apellido_Materno.Contains(filtroApellidoMaterno.Text)) || (o.Miembro.Apellido_Materno == null)) &&
                                ((o.Miembro.Email.Contains(filtroEmail.Text)) || (o.Miembro.Email == null)) &&
                                (idsCelulas.Contains(o.Miembro.CelulaId) || (idsCelulas.Count == 0)) &&
                                (idGeneros.Contains(o.Miembro.GeneroId) || (idGeneros.Count == 0)) &&
                                (idsEstadosCiviles.Contains(o.Miembro.EstadoCivilId) || (idsEstadosCiviles.Count == 0)) &&
                                (o.Miembro.UbicacionMunicipioId == (idMunicipio > 0 ? idMunicipio : o.Miembro.UbicacionMunicipioId)) &&
                                ((o.Miembro.Colonia.Contains(filtroColonia.Text)) || (o.Miembro.Colonia == null)) &&
                                ((o.Miembro.Direccion.Contains(filtroDireccion.Text)) || (o.Miembro.Direccion == null)) &&
                                (((o.Miembro.Tel_Casa.Contains(filtroTel.Text)) || (o.Miembro.Tel_Casa == null)) ||
                                ((o.Miembro.Tel_Movil.Contains(filtroTel.Text)) || (o.Miembro.Tel_Movil == null)) ||
                                ((o.Miembro.Tel_Trabajo.Contains(filtroTel.Text)) || (o.Miembro.Tel_Trabajo == null))) &&
                                (o.Miembro.Fecha_Nacimiento == (filtroFechaDeNacimiento.SelectedDate > DateTime.MinValue ? filtroFechaDeNacimiento.SelectedDate : o.Miembro.Fecha_Nacimiento)) &&
                                (o.Miembro.Borrado == registrosBorrados) && //Registros NO borrados
                                (o.Borrado == false)) //Registros NO borrados
                            orderby o.Miembro.Primer_Nombre ascending, o.Miembro.Apellido_Paterno ascending
                            select new
                            {
                                GUID = o.Id,
                                Id = o.Miembro.MiembroId,
                                PrimerNombre = o.Miembro.Primer_Nombre,
                                SegundoNombre = o.Miembro.Segundo_Nombre,
                                ApellidoPaterno = o.Miembro.Apellido_Paterno,
                                ApellidoMaterno = o.Miembro.Apellido_Materno,
                                Email = o.Miembro.Email,
                                Celula = o.Miembro.Celula.Descripcion,
                                EstadoCivil = o.Miembro.EstadoCivil.Descripcion,
                                Municipio = o.Miembro.UbicacionMunicipio.Descripcion,
                                Colonia = o.Miembro.Colonia,
                                TelCasa = o.Miembro.Tel_Casa,
                                TelMovil = o.Miembro.Tel_Movil,
                                TelTrabajo = o.Miembro.Tel_Trabajo,
                                Nacimiento = o.Miembro.Fecha_Nacimiento,
                                AsisteIglesia = o.Miembro.AsisteIglesia,
                                Genero = o.Miembro.Genero.Descripcion
                            });
                        break;
                    }

                case TipoDeMiembroABuscar.AlabanzaMiembro:
                    {
                        resultados = (
                            from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro
                            where
                                ((o.Miembro.MiembroId == (filtroId.Number > 0 ? filtroId.Number : o.Miembro.MiembroId)) &&
                                ((o.Miembro.Primer_Nombre.Contains(filtroPrimerNombre.Text)) || (o.Miembro.Primer_Nombre == null)) &&
                                ((o.Miembro.Segundo_Nombre.Contains(filtroSegundoNombre.Text)) || (o.Miembro.Segundo_Nombre == null)) &&
                                ((o.Miembro.Apellido_Paterno.Contains(filtroApellidoPaterno.Text)) || (o.Miembro.Apellido_Paterno == null)) &&
                                ((o.Miembro.Apellido_Materno.Contains(filtroApellidoMaterno.Text)) || (o.Miembro.Apellido_Materno == null)) &&
                                ((o.Miembro.Email.Contains(filtroEmail.Text)) || (o.Miembro.Email == null)) &&
                                (idsCelulas.Contains(o.Miembro.CelulaId) || (idsCelulas.Count == 0)) &&
                                (idGeneros.Contains(o.Miembro.GeneroId) || (idGeneros.Count == 0)) &&
                                (idsEstadosCiviles.Contains(o.Miembro.EstadoCivilId) || (idsEstadosCiviles.Count == 0)) &&
                                (o.Miembro.UbicacionMunicipioId == (idMunicipio > 0 ? idMunicipio : o.Miembro.UbicacionMunicipioId)) &&
                                ((o.Miembro.Colonia.Contains(filtroColonia.Text)) || (o.Miembro.Colonia == null)) &&
                                ((o.Miembro.Direccion.Contains(filtroDireccion.Text)) || (o.Miembro.Direccion == null)) &&
                                (((o.Miembro.Tel_Casa.Contains(filtroTel.Text)) || (o.Miembro.Tel_Casa == null)) ||
                                ((o.Miembro.Tel_Movil.Contains(filtroTel.Text)) || (o.Miembro.Tel_Movil == null)) ||
                                ((o.Miembro.Tel_Trabajo.Contains(filtroTel.Text)) || (o.Miembro.Tel_Trabajo == null))) &&
                                (o.Miembro.Fecha_Nacimiento == (filtroFechaDeNacimiento.SelectedDate > DateTime.MinValue ? filtroFechaDeNacimiento.SelectedDate : o.Miembro.Fecha_Nacimiento)) &&
                                (o.Miembro.Borrado == registrosBorrados) && //Registros NO borrados
                                (o.Borrado == false)) //Registros NO borrados
                            orderby o.Miembro.Primer_Nombre ascending, o.Miembro.Apellido_Paterno ascending
                            select new
                            {
                                GUID = o.Id,
                                Id = o.Miembro.MiembroId,
                                PrimerNombre = o.Miembro.Primer_Nombre,
                                SegundoNombre = o.Miembro.Segundo_Nombre,
                                ApellidoPaterno = o.Miembro.Apellido_Paterno,
                                ApellidoMaterno = o.Miembro.Apellido_Materno,
                                Email = o.Miembro.Email,
                                Celula = o.Miembro.Celula.Descripcion,
                                EstadoCivil = o.Miembro.EstadoCivil.Descripcion,
                                Municipio = o.Miembro.UbicacionMunicipio.Descripcion,
                                Colonia = o.Miembro.Colonia,
                                TelCasa = o.Miembro.Tel_Casa,
                                TelMovil = o.Miembro.Tel_Movil,
                                TelTrabajo = o.Miembro.Tel_Trabajo,
                                Nacimiento = o.Miembro.Fecha_Nacimiento,
                                AsisteIglesia = o.Miembro.AsisteIglesia,
                                Genero = o.Miembro.Genero.Descripcion
                            });
                        break;
                    }
            }

            if (resultados != null)
            {
                //int numeroDeResultadosMax = 75; //Es el numero maximo de resultados a regresar al cliente...
                //int numeroDeResultados = resultados.Count();
                //if (numeroDeResultados > numeroDeResultadosMax) { X.Msg.Alert(Generales.nickNameDeLaApp, string.Format(Resources.Literales.LimiteDeResultadosExcedido, numeroDeResultados, numeroDeResultadosMax)).Show(); }
                //StoreResultados.Cargar(resultados.Take(numeroDeResultadosMax));
                StoreResultados.Cargar(resultados);
            }
        }

    }
}