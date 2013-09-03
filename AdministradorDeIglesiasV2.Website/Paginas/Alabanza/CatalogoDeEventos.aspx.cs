using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Objects;
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

namespace AdministradorDeIglesiasV2.Website.Paginas.Alabanza
{
    public partial class CatalogoDeEventos : PaginaBase, ICatalogo, ICatalogoComplejo
    {
        ManejadorDeAlabanza manejadorDeAlabanza = new ManejadorDeAlabanza();

        void ICatalogo.CargarControles()
        {
            permitirAgregarEnsayos(false);

            // Ocultamos las columnas de asistencia/retraso
            registroMiembros.ColumnModel.SetHidden(3, true);
            registroMiembros.ColumnModel.SetHidden(4, true);

            StoreMiembros.Limpiar();
            StoreCanciones.Limpiar();
            StoreEnsayos.Limpiar();
            StoreEnsayoMiembros.Limpiar();
            StoreHorasDelDia.Cargar(HoraDia.Obtener());
            StoreInstrumentos.Cargar(AlabanzaTipoInstrumento.Obtener());
        }

        void ICatalogo.Buscar()
        {
            List<int> idsHorasDelDiaInicio = filtroHoraDelDiaInicio.ObtenerIds();
            List<int> idsHorasDelDiaFin = filtroHoraDelDiaInicio.ObtenerIds();

            StoreResultados.Cargar(manejadorDeAlabanza.BuscarEventos(filtroId.Number == Double.MinValue ? -1 : Convert.ToInt32(filtroId.Number), filtroDescripcion.Text, filtroFecha.SelectedDate, idsHorasDelDiaInicio, idsHorasDelDiaFin).Select(o =>
                new
                {
                    Id = o.Id,
                    Descripcion = o.Descripcion,
                    Fecha = o.Fecha,
                    HoraInicio = o.HoraDiaInicio.Descripcion,
                    HoraFin = o.HoraDiaFin.Descripcion
                }));
        }

        void ICatalogo.Mostrar(int id)
        {
            AlabanzaEvento entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEvento where o.Id == id select o).FirstOrDefault();
            registroId.Value = entidad.Id;
            registroDescripcion.Value = entidad.Descripcion;
            registroFecha.Value = entidad.Fecha;
            registroHoraDelDiaInicio.Value = entidad.HoraDiaInicioId;
            registroHoraDelDiaFin.Value = entidad.HoraDiaFinId;

            permitirAgregarEnsayos(true);

            // Mostramos las columnas de asistencia/retraso
            registroMiembros.ColumnModel.SetHidden(3, false);
            registroMiembros.ColumnModel.SetHidden(4, false);

            // Cargamos los grids
            CargarMiembros();
            CargarCanciones();
            CargarEnsayos();
        }

        void ICatalogo.Borrar(int id)
        {
            AlabanzaEvento entidad = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEvento where o.Id == id select o).FirstOrDefault();
            entidad.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
        }

        void ICatalogo.Guardar(int id, RegistrosHelper.ListaDeRegistrosDeDatos listaDeRegistrosDeDatos)
        {
            // Condiciones de errores NO FATALES
            bool algunaAsistenciaNovalida = false;
            bool algunMiembroYaExistente = false;
            bool algunaCancionYaExistente = false;

            RegistrosHelper.RegistrosDeDatos miembros = listaDeRegistrosDeDatos.Obtener(registroMiembros.ClientID);
            RegistrosHelper.RegistrosDeDatos canciones = listaDeRegistrosDeDatos.Obtener(registroCanciones.ClientID);
            RegistrosHelper.RegistrosDeDatos ensayos = listaDeRegistrosDeDatos.Obtener(registroEnsayos.ClientID);

            AlabanzaEvento evento;
            string cancionesYMiembrosRequeridos = "No es posible continuar sin agregar canciones/miembros.";
            if (id > 0)
            {
                evento = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEvento where o.Id == id select o).FirstOrDefault();
                if ((evento.AlabanzaEventoMiembro.Count <= 0 && miembros.RegistrosNuevosId.Count <= 0) || (evento.AlabanzaEventoCancion.Count <= 0 && canciones.RegistrosNuevosId.Count <= 0))
                {
                    throw new ExcepcionReglaNegocio(cancionesYMiembrosRequeridos);
                }
            }
            else
            {
                evento = new AlabanzaEvento();
                if ((miembros.RegistrosNuevosId.Count <= 0) || (canciones.RegistrosNuevosId.Count <= 0))
                {
                    throw new ExcepcionReglaNegocio(cancionesYMiembrosRequeridos);
                }
            }

            evento.Descripcion = registroDescripcion.Text;
            evento.Fecha = registroFecha.SelectedDate;
            evento.HoraDiaInicioId = registroHoraDelDiaInicio.ObtenerId();
            evento.HoraDiaFinId = registroHoraDelDiaFin.ObtenerId();
            evento.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

            // Se determina si la fecha es la correcta como para establecer la asistencia
            bool esValidoMarcarAsistencia = (DateTime.Now >= evento.Fecha);

            #region Procesamos los MIEMBROS

            foreach (KeyValuePair<int, Dictionary<string, string>> registro in miembros.RegistrosNuevos)
            {
                AlabanzaEventoMiembro miembro;
                if (registro.Key > 0)
                {
                    miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoMiembro where o.Id == registro.Key select o).SingleOrDefault();
                }
                else
                {
                    miembro = new AlabanzaEventoMiembro();
                }

                miembro.AlabanzaEventoId = evento.Id;
                miembro.AlabanzaMiembroId = registro.Value["AlabanzaMiembroId"].ToInt();
                miembro.AlabanzaTipoInstrumentoId = registro.Value["InstrumentoId"].ToInt();
                miembro.Retraso = registro.Value["Retraso"].ToBool();
                miembro.Asistencia = registro.Value["Asistencia"].ToBool();

                if ((miembro.Retraso || miembro.Asistencia) && !esValidoMarcarAsistencia)
                {
                    algunaAsistenciaNovalida = true;
                }

                if ((!evento.AlabanzaEventoMiembro.Any(o => o.AlabanzaMiembroId == miembro.AlabanzaMiembroId && o.AlabanzaTipoInstrumentoId == miembro.AlabanzaTipoInstrumentoId && o.Borrado == false)) || (registro.Key > 0))
                {
                    miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }
                else
                {
                    algunMiembroYaExistente = true;
                }
            }

            foreach (int idEliminado in miembros.RegistrosEliminadosId)
            {
                AlabanzaEventoMiembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoMiembro where o.Id == idEliminado select o).SingleOrDefault();
                miembro.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }

            #endregion

            #region Procesamos las CANCIONES

            foreach (KeyValuePair<int, Dictionary<string, string>> registro in canciones.RegistrosNuevos)
            {
                AlabanzaEventoCancion cancion;
                if (registro.Key > 0)
                {
                    cancion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoCancion where o.Id == registro.Key select o).SingleOrDefault();
                }
                else
                {
                    cancion = new AlabanzaEventoCancion();
                }

                cancion.AlabanzaEventoId = evento.Id;
                cancion.AlabanzaCancionId = registro.Value["AlabanzaCancionId"].ToInt();

                if ((!evento.AlabanzaEventoCancion.Any(o => o.AlabanzaCancionId == cancion.AlabanzaCancionId && o.Borrado == false)) || (registro.Key > 0))
                {
                    cancion.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }
                else
                {
                    algunaCancionYaExistente = true;
                }
            }

            foreach (int idEliminado in canciones.RegistrosEliminadosId)
            {
                AlabanzaEventoCancion cancion = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoCancion where o.Id == idEliminado select o).SingleOrDefault();
                cancion.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }

            #endregion

            #region Procesamos los ENSAYOS

            foreach (int idEliminado in ensayos.RegistrosEliminadosId)
            {
                AlabanzaEnsayo ensayo = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayo where o.Id == idEliminado select o).SingleOrDefault();
                ensayo.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
            }

            #endregion

            #region validamos y alertamos todos los ERRORES NO FATALES

            System.Text.StringBuilder condicionDeError = new System.Text.StringBuilder();
            if (algunaAsistenciaNovalida)
            {
                condicionDeError.AppendLine("No se pueden registrar asistencias/retrasos si el evento aun no se ha llevado a cabo.");
            }

            if (algunMiembroYaExistente)
            {
                condicionDeError.AppendLine("No puede existir multiples veces el mismo miembro asignado el mismo instrumento.");
            }

            if (algunaCancionYaExistente)
            {
                condicionDeError.AppendLine("No puede existir multiples veces la mismsa cancion.");
            }

            if (!string.IsNullOrEmpty(condicionDeError.ToString()))
            {
                X.MessageBox.Alert(Generales.nickNameDeLaApp, "Los cambios han sido guardados, con la siguiente condicion: " + condicionDeError.ToString()).Show();
            }

            #endregion
        }

        void permitirAgregarEnsayos(bool permitir)
        {
            if (permitir)
            {
                btnAgregarNuevoEnsayo.Disabled = false;
                btnAgregarNuevoEnsayo.ToolTip = "";
            }
            else
            {
                btnAgregarNuevoEnsayo.Disabled = true; // Deshabilitado hasta que se encuentre en modo de "edicion"
                btnAgregarNuevoEnsayo.ToolTip = "Para poder agregar ensayos es necesario guardar los cambios actuales.";
            }
        }

        #region DirectMethods

        [DirectMethod(ShowMask = true)]
        public void AgregarMiembro()
        {
            try
            {
                int alabanzaMiembroId = registroMiembro.ObtenerId();
                int instrumentoId = registroInstrumento.ObtenerId();

                if (alabanzaMiembroId > 0 && instrumentoId > 0)
                {
                    permitirAgregarEnsayos(false);

                    AlabanzaMiembro alabanzaMiembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaMiembro where o.Id == alabanzaMiembroId select o).SingleOrDefault();
                    AlabanzaTipoInstrumento instrumento = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaTipoInstrumento where o.Id == instrumentoId select o).SingleOrDefault();
                    StoreMiembros.AddRecord(new
                    {
                        MiembroId = alabanzaMiembro.MiembroId,
                        AlabanzaMiembroId = alabanzaMiembroId,
                        Nombre = alabanzaMiembro.Miembro.NombreCompleto,
                        InstrumentoId = instrumentoId,
                        Instrumento = instrumento.Descripcion
                    });

                    LimpiarVentanaDeMiembros();
                    X.Msg.Notify(Generales.nickNameDeLaApp, "Miembro agregadado correctamente").Show();
                }
                else
                {
                    throw new ExcepcionReglaNegocio("Favor de seleccionar algun miembro e instrumento validos.");
                }

            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void BuscarCancion()
        {
            try
            {
                StoreCancionesEncontradas.Cargar(manejadorDeAlabanza.BuscarCanciones(filtroCancionTitulo.Text,
                    filtroCancionArtista.Text,
                    filtroCancionDisco.Text,
                    filtroCancionTono.Text).Select(o =>
                    new
                    {
                        Id = o.Id,
                        Titulo = o.Titulo,
                        Artista = o.Artista,
                        Disco = o.Disco,
                        Tono = o.Tono
                    }
                                    ));
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void AgregarCancion()
        {
            try
            {
                RowSelectionModel sm = registroCancionesEncontradas.SelectionModel.Primary as RowSelectionModel;
                if (sm.SelectedRow != null)
                {
                    int id = Convert.ToInt32(sm.SelectedRecordID);
                    AlabanzaCancion cancion = manejadorDeAlabanza.ObtenerCancion(id);
                    if (cancion != null)
                    {
                        StoreCanciones.AddRecord(
                        new
                        {
                            AlabanzaCancionId = cancion.Id,
                            Titulo = cancion.Titulo,
                            Artista = cancion.Artista,
                            Disco = cancion.Disco,
                            Tono = cancion.Tono
                        });

                        LimpiarVentanaDeCanciones();
                        X.Msg.Notify(Generales.nickNameDeLaApp, "Cancion agregada correctamente").Show();
                    }
                    else
                    {
                        throw new ExcepcionAplicacion("Cancion inexistente");
                    }
                }
                else
                {
                    throw new ExcepcionReglaNegocio("Es necesario seleccionar alguna cancion.");
                }
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void GuardarEnsayo(List<Dictionary<string, string>> registros)
        {
            try
            {
                // Condiciones de errores NO FATALES
                bool algunaAsistenciaNovalida = false;

                RegistrosHelper.RegistrosDeDatos miembros = RegistrosHelper.ObtenerRegistrosDiferenciados(registros);
                int eventoId = Convert.ToInt32(registroId.Number);
                int ensayoId = Convert.ToInt32((registroEnsayoId.Number > double.MinValue) ? registroEnsayoId.Number : -1);

                AlabanzaEnsayo ensayo = (ensayoId > 0 ? (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayo where o.Id == ensayoId select o).SingleOrDefault() : new AlabanzaEnsayo());
                ensayo.AlabanzaEventoId = eventoId;
                ensayo.Fecha = registroEnsayoFecha.SelectedDate;
                ensayo.HoraDiaInicioId = registroEnsayoHoraInicio.ObtenerId();
                ensayo.HoraDiaFinId = registroEnsayoHoraFin.ObtenerId();
                ensayo.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());

                // Se determina si la fecha es la correcta como para establecer la asistencia
                bool esValidoMarcarAsistencia = (DateTime.Now >= ensayo.Fecha);

                foreach (KeyValuePair<int, Dictionary<string, string>> registro in miembros.RegistrosNuevos)
                {
                    int alabanzaEnsayoMiembroId = registro.Value["Id"].ToInt();

                    AlabanzaEnsayoMiembro miembro = (alabanzaEnsayoMiembroId > 0 ? (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayoMiembro where o.Id == alabanzaEnsayoMiembroId select o).SingleOrDefault() : new AlabanzaEnsayoMiembro());
                    miembro.AlabanzaEnsayoId = ensayo.Id;
                    miembro.AlabanzaMiembroId = registro.Value["AlabanzaMiembroId"].ToInt();
                    miembro.Retraso = registro.Value["Retraso"].ToBool();
                    miembro.Asistencia = registro.Value["Asistencia"].ToBool();

                    if ((miembro.Retraso || miembro.Asistencia) && !esValidoMarcarAsistencia)
                    {
                        algunaAsistenciaNovalida = true;
                    }

                    miembro.Guardar(SesionActual.Instance.getContexto<IglesiaEntities>());
                }

                foreach (int idEliminado in miembros.RegistrosEliminadosId)
                {
                    if (idEliminado > 0)
                    {
                        AlabanzaEnsayoMiembro miembro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayoMiembro where o.Id == idEliminado select o).FirstOrDefault();
                        miembro.Borrar(SesionActual.Instance.getContexto<IglesiaEntities>());
                    }
                }

                // Ocultamos las columnas de asistencia/retraso
                registroEnsayoMiembros.ColumnModel.SetHidden(3, true);
                registroEnsayoMiembros.ColumnModel.SetHidden(4, true);

                LimpiarVentanaDeEnsayos();
                CargarEnsayos();
                wndAgregarEnsayo.Hide();

                #region validamos y alertamos todos los ERRORES NO FATALES

                System.Text.StringBuilder condicionDeError = new System.Text.StringBuilder();
                if (algunaAsistenciaNovalida)
                {
                    condicionDeError.AppendLine("No se pueden registrar asistencias/retrasos si el ensayo aun no se ha llevado a cabo.");
                }

                if (!string.IsNullOrEmpty(condicionDeError.ToString()))
                {
                    X.MessageBox.Alert(Generales.nickNameDeLaApp, "Los cambios han sido guardados, con la siguiente condicion: " + condicionDeError.ToString()).Show();
                }

                #endregion
            }
            catch (ExcepcionReglaNegocio ex)
            {
                X.Msg.Alert(Generales.nickNameDeLaApp, ex.Message).Show();
            }
        }

        [DirectMethod(ShowMask = true)]
        public void CargarEnsayo(int ensayoId)
        {
            if (ensayoId > 0)
            {
                AlabanzaEnsayo ensayo = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayo where o.Id == ensayoId select o).SingleOrDefault();
                registroEnsayoId.Value = ensayo.Id;
                registroEnsayoFecha.Value = ensayo.Fecha;
                registroEnsayoHoraInicio.Value = ensayo.HoraDiaInicioId;
                registroEnsayoHoraFin.Value = ensayo.HoraDiaFinId;

                // Mostramos las columnas de asistencia/retraso
                registroEnsayoMiembros.ColumnModel.SetHidden(3, false);
                registroEnsayoMiembros.ColumnModel.SetHidden(4, false);

                StoreEnsayoMiembros.Cargar(from o in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEnsayoMiembro
                                           join p in SesionActual.Instance.getContexto<IglesiaEntities>().AlabanzaEventoMiembro on o.AlabanzaMiembroId equals p.AlabanzaMiembroId
                                           where
                                               o.AlabanzaEnsayoId == ensayoId &&
                                               o.AlabanzaMiembro.Borrado == false &&
                                               o.AlabanzaEnsayo.Borrado == false &&
                                               p.AlabanzaMiembro.Borrado == false &&
                                               p.AlabanzaEvento.Borrado == false &&
                                               p.Borrado == false
                                           select new
                                           {
                                               Id = o.Id,
                                               MiembroId = o.AlabanzaMiembro.MiembroId,
                                               AlabanzaMiembroId = o.AlabanzaMiembroId,
                                               Nombre = o.AlabanzaMiembro.Miembro.Primer_Nombre + " " + o.AlabanzaMiembro.Miembro.Segundo_Nombre + " " + o.AlabanzaMiembro.Miembro.Apellido_Paterno + " " + o.AlabanzaMiembro.Miembro.Apellido_Materno + " (" + o.AlabanzaMiembro.Miembro.Email + ")",
                                               Instrumento = p.AlabanzaTipoInstrumento.Descripcion,
                                               Asistencia = o.Asistencia,
                                               Retraso = o.Retraso
                                           });
            }
            else
            {
                int eventoId = Convert.ToInt32(registroId.Number);
                LimpiarVentanaDeEnsayos();

                // Ocultamos las columnas de asistencia/retraso
                registroEnsayoMiembros.ColumnModel.SetHidden(3, true);
                registroEnsayoMiembros.ColumnModel.SetHidden(4, true);

                StoreEnsayoMiembros.Cargar(manejadorDeAlabanza.ObtenerMiembrosPorEvento(eventoId).Select(o =>
                    new
                    {
                        MiembroId = o.AlabanzaMiembro.MiembroId,
                        AlabanzaMiembroId = o.AlabanzaMiembroId,
                        Nombre = o.AlabanzaMiembro.Miembro.Primer_Nombre + " " + o.AlabanzaMiembro.Miembro.Segundo_Nombre + " " + o.AlabanzaMiembro.Miembro.Apellido_Paterno + " " + o.AlabanzaMiembro.Miembro.Apellido_Materno + " (" + o.AlabanzaMiembro.Miembro.Email + ")",
                        Instrumento = o.AlabanzaTipoInstrumento.Descripcion
                    }));

                registroEnsayoMiembros.MarcarSucio();
            }

            wndAgregarEnsayo.Show();
        }

        [DirectMethod(ShowMask = true)]
        public void CargarMiembros()
        {
            int eventoId = Convert.ToInt32(registroId.Number);
            StoreMiembros.Cargar(manejadorDeAlabanza.ObtenerMiembrosPorEvento(eventoId).Select(o =>
                new
                {
                    Id = o.Id,
                    MiembroId = o.AlabanzaMiembro.MiembroId,
                    AlabanzaMiembroId = o.AlabanzaMiembroId,
                    Nombre = o.AlabanzaMiembro.Miembro.Primer_Nombre + " " + o.AlabanzaMiembro.Miembro.Segundo_Nombre + " " + o.AlabanzaMiembro.Miembro.Apellido_Paterno + " " + o.AlabanzaMiembro.Miembro.Apellido_Materno + " (" + o.AlabanzaMiembro.Miembro.Email + ")",
                    InstrumentoId = o.AlabanzaTipoInstrumentoId,
                    Instrumento = o.AlabanzaTipoInstrumento.Descripcion,
                    Asistencia = o.Asistencia,
                    Retraso = o.Retraso
                }));
        }

        [DirectMethod(ShowMask = true)]
        public void CargarCanciones()
        {
            int eventoId = Convert.ToInt32(registroId.Number);
            StoreCanciones.Cargar(manejadorDeAlabanza.ObtenerCancionesPorEvento(eventoId).Select(o =>
                new
                {
                    Id = o.Id,
                    AlabanzaCancionId = o.AlabanzaCancionId,
                    Titulo = o.AlabanzaCancion.Titulo,
                    Artista = o.AlabanzaCancion.Artista,
                    Disco = o.AlabanzaCancion.Disco,
                    Tono = o.AlabanzaCancion.Tono
                }));
        }

        [DirectMethod(ShowMask = true)]
        public void CargarEnsayos()
        {
            int eventoId = Convert.ToInt32(registroId.Number);
            StoreEnsayos.Cargar(manejadorDeAlabanza.ObtenerEnsayosPorEvento(eventoId).Select(o => new
            {
                Id = o.Id,
                Fecha = o.Fecha,
                HoraInicio = o.HoraDiaInicio.Descripcion,
                HoraFin = o.HoraDiaFin.Descripcion
            }));

        }

        [DirectMethod(ShowMask = true)]
        public void LimpiarVentanaDeMiembros()
        {
            pnlAgregarMiembro.Reset();
            registroMiembro.Limpiar();
        }

        [DirectMethod(ShowMask = true)]
        public void LimpiarVentanaDeCanciones()
        {
            pnlAgregarCancion.Reset();
            StoreCancionesEncontradas.Limpiar();
        }

        [DirectMethod(ShowMask = true)]
        public void LimpiarVentanaDeEnsayos()
        {
            pnlAgregarEnsayo.Reset();
            StoreEnsayoMiembros.Limpiar();
        }

        #endregion
    }
}