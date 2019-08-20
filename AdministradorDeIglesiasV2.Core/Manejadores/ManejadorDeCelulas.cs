using AdministradorDeIglesiasV2.Core.Modelos;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Reflection;
using System.Transactions;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Manejadores
{
    public class ManejadorDeCelulas
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ManejadorDeCelulas));

        public void BorrarCelulaPermanentemente(int celulaId)
        {
            List<int> celulas = this.ObtenerRedInferior(celulaId);
            celulas.Add(celulaId);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 10, 0)))
            {

                foreach (int celula in celulas)
                {

                    // Se va a hacer cambios a los miembros, sin borrarlos en si
                    using (var contexto = new IglesiaEntities())
                    {

                        // Obtenemos todos los miembros de la celula
                        // List<int> miembros = (from o in contexto.Miembro where o.CelulaId == celula select o.MiembroId).Take<int>(1).ToList<int>();
                        List<int> miembros = (from o in contexto.Miembro where o.CelulaId == celula select o.MiembroId).ToList<int>();

                        foreach (int miembro in miembros)
                        {
                            // Borramos las "Fotos" de cada miembro
                            foreach (MiembroFoto aBorrar in (from o in contexto.MiembroFoto where o.MiembroId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos los "Pasos" de cada miembro
                            foreach (MiembroPaso aBorrar in (from o in contexto.MiembroPaso where o.MiembroId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos los "Roles" de cada miembro
                            foreach (MiembroRol aBorrar in (from o in contexto.MiembroRol where o.MiembroId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos las "Asistencias a las celulas" de cada miembro
                            foreach (CelulaMiembroAsistencia aBorrar in (from o in contexto.CelulaMiembroAsistencia where o.MiembroId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos las "Asistencias Registradas" de cada miembro
                            foreach (CelulaMiembroAsistencia aBorrar in (from o in contexto.CelulaMiembroAsistencia where o.MiembroQueRegistraId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos las "Asistencias de Invitados Registradas" de cada miembro
                            foreach (CelulaInvitadosAsistencia aBorrar in (from o in contexto.CelulaInvitadosAsistencia where o.MiembroQueRegistraId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos las "Cancelaciones Registradas" de cada miembro
                            foreach (CelulaCancelacionAsistencia aBorrar in (from o in contexto.CelulaCancelacionAsistencia where o.MiembroQueRegistraId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos los "Liderazgos" de cada miembro
                            foreach (CelulaLider aBorrar in (from o in contexto.CelulaLider where o.MiembroId == miembro select o)) {
                                contexto.DeleteObject(aBorrar);
                            }

                            // Borramos las "Notificaciones de Asistencia" de cada miembro
                            foreach (NotificacionDeAsistenciaInscripcion aBorrar in (from o in contexto.NotificacionDeAsistenciaInscripcion where o.MiembroId == miembro select o)) {
                                NotificacionDeAsistenciaLog log = (from o in contexto.NotificacionDeAsistenciaLog where o.NotificacionDeAsistenciaId == aBorrar.Id select o).SingleOrDefault();
                                if (log != null) {
                                    contexto.DeleteObject(log);
                                }

                                contexto.DeleteObject(aBorrar);
                            }

                            // Modificamos las "Boletas de Consolidacion" asignadas a cada miembro
                            foreach (ConsolidacionBoleta aModificar in (from o in contexto.ConsolidacionBoleta where o.AsignadaAMiembroId == miembro select o)) {
                                aModificar.AsignadaAMiembroId = null;
                            }

                            // Modificamos las "Boletas de Consolidacion" en que cada miembro "invito"
                            foreach (ConsolidacionBoleta aModificar in (from o in contexto.ConsolidacionBoleta where o.InvitadoPorMiembroId == miembro select o)) {
                                aModificar.InvitadoPorMiembroId = null;
                            }

                            // Modificamos las "Conyugues" en cada miembro
                            foreach (Miembro aModificar in (from o in contexto.Miembro where o.ConyugeId == miembro select o)) {
                                aModificar.ConyugeId = null;
                            }

                        }

                        // Guardamos los cambios en la BD (Por miembros...)
                        contexto.SaveChanges(SaveOptions.DetectChangesBeforeSave, true);
                    }

                    // Se va a hacer cambios a los miembros, incluido borrarlos
                    using (var contexto = new IglesiaEntities()) {

                        // Obtenemos todos los miembros de la celula
                        List<int> miembros = (from o in contexto.Miembro where o.CelulaId == celula select o.MiembroId).ToList<int>();

                        foreach (int miembro in miembros) {
                            // Borramos los miembros en si
                            Miembro miembroABorrar = (from o in contexto.Miembro where o.MiembroId == miembro select o).SingleOrDefault();
                            contexto.DeleteObject(miembroABorrar);
                        }

                        // Guardamos los cambios en la BD (Por miembros...)
                        contexto.SaveChanges(SaveOptions.DetectChangesBeforeSave, true);
                    }


                    // Se va a hacer cambios a las celulas, incluido borrarlas
                    using (var contexto = new IglesiaEntities())
                    {
                        // Borramos las "Asistencias" de la celula en si
                        foreach (CelulaMiembroAsistencia aBorrar in (from o in contexto.CelulaMiembroAsistencia where o.CelulaId == celula select o))
                        {
                            contexto.DeleteObject(aBorrar);
                        }

                        // Borramos las "Asistencias de Invitados" de la celula en si
                        foreach (CelulaInvitadosAsistencia aBorrar in (from o in contexto.CelulaInvitadosAsistencia where o.CelulaId == celula select o)) {
                            contexto.DeleteObject(aBorrar);
                        }

                        // Borramos las "Cancelaciones de Asistencia" de la celula en si
                        foreach (CelulaCancelacionAsistencia aBorrar in (from o in contexto.CelulaCancelacionAsistencia where o.CelulaId == celula select o))
                        {
                            contexto.DeleteObject(aBorrar);
                        }

                        // Borramos los "Liderazgos" de la celula en si
                        foreach (CelulaLider aBorrar in (from o in contexto.CelulaLider where o.CelulaId == celula select o))
                        {
                            contexto.DeleteObject(aBorrar);
                        }

                        // Modificamos las "Boletas de Consolidacion" asignadas a la celula en si
                        foreach (ConsolidacionBoleta aBorrar in (from o in contexto.ConsolidacionBoleta where o.AsignadaACelulaId == celula select o))
                        {
                            aBorrar.AsignadaACelulaId = null;
                        }

                        // Modificamos las "Celula Predeterminada para las Boletas de Consolidacion" asignadas a la celula en si
                        foreach (ConsolidacionBoletaCategoria aModificar in (from o in contexto.ConsolidacionBoletaCategoria where o.CelulaPredeterminadaId == celula select o))
                        {
                            aModificar.CelulaPredeterminadaId = 1;
                        }

                        // Boramos la celula en si
                        Celula celulaABorrar = (from o in contexto.Celula where o.CelulaId == celula select o).SingleOrDefault();
                        contexto.DeleteObject(celulaABorrar);

                        // Guardamos los cambios en la BD (Por celula...)
                        contexto.SaveChanges(SaveOptions.DetectChangesBeforeSave, true);
                    }
                }

                scope.Complete();
            }

            Cache.Instance.Limpiar();
        }

        #region Celulas sin Lideres

        public List<int> ObtenerCelulasSinLideresComoIds()
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                    where
                        o.CelulaLider.Count(cl => cl.Borrado == false) == 0 &&
                        o.Borrado == false
                    orderby o.CelulaId
                    select o.CelulaId).ToList<int>();
        }

        public List<Celula> ObtenerCelulasSinLideresComoCelulas()
        {
            List<int> celulas = ObtenerCelulasSinLideresComoIds();
            return (from c in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                    where celulas.Contains(c.CelulaId)
                    orderby c.Descripcion
                    select c).ToList<Celula>();
        }

        #endregion

        #region Celulas eliminadas con Miembros

        public List<int> ObtenerCelulasEliminadasConMiembrosComoIds()
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                    where
                        o.Miembro.Any(m => m.Borrado == false) == true &&
                        !o.Miembro.All(m => m.Borrado == false) &&
                        o.Borrado == true
                    orderby o.CelulaId
                    select o.CelulaId).ToList<int>();
        }

        #endregion

        #region Celulas por Miembro

        /// <summary>
        /// Regresa la primer celula encontrada a la que el miembro es lider directo
        /// </summary>
        /// <param name="miembroId"></param>
        /// <returns></returns>
        public Celula ObtenerCelulaQueMiembroEsLider(int miembroId)
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name + miembroId;
            Celula registro = Cache.Instance.Obtener<Celula>(key);
            if (registro == null)
            {
                registro = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                            where
                                o.MiembroId == miembroId &&
                                o.Miembro.Borrado == false &&
                                o.Celula.Borrado == false &&
                                o.Borrado == false
                            select o.Celula).FirstOrDefault();
                Cache.Instance.Guardar(key, registro);
            }
            return registro;
        }

        public List<RegistroBasico> ObtenerCelulasPermitidasPorMiembro(int miembroId)
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name + miembroId;
            List<RegistroBasico> registros = Cache.Instance.Obtener<List<RegistroBasico>>(key);
            if (registros == null)
            {
                List<int> celulas = ObtenerCelulasPermitidasPorMiembroComoIds(miembroId);
                registros = (from c in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                             where celulas.Contains(c.CelulaId)
                             orderby c.Descripcion
                             select new RegistroBasico
                             {
                                 Id = c.CelulaId,
                                 Descripcion = c.Descripcion
                             }).ToList<RegistroBasico>();

                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }

        /// <summary>
        /// Este metodo sirve para obtener todas las celulas permitidas por un miembro especifico (es decir que es lider, o que es lider del lider...)
        /// EL RESULTADO ES UNA LISTA DE CELULAS (ENTIDADES)
        /// </summary>
        /// <param name="miembroId"></param>
        /// <returns></returns>
        public List<Celula> ObtenerCelulasPermitidasPorMiembroComoCelulas(int miembroId)
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name + miembroId;
            List<Celula> registros = Cache.Instance.Obtener<List<Celula>>(key);
            if (registros == null)
            {
                List<int> celulas = ObtenerCelulasPermitidasPorMiembroComoIds(miembroId);
                registros = (from c in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                             where celulas.Contains(c.CelulaId)
                             orderby c.Descripcion
                             select c).ToList<Celula>();

                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }

        /// <summary>
        /// Este metodo sirve para obtener todas las celulas permitidas por un miembro especifico (es decir que es lider, o que es lider del lider...)
        /// EL RESULTADO ES UNA LISTA DE IDS
        /// </summary>
        /// <param name="miembroId"></param>
        /// <returns></returns>
        public List<int> ObtenerCelulasPermitidasPorMiembroComoIds(int miembroId)
        {
            return ObtenerCelulasPermitidasPorMiembroComoIds(miembroId, false);
        }

        public List<int> ObtenerCelulasPermitidasPorMiembroComoIds(int miembroId, bool mostrarCelulasSinLider)
        {
            string key = MethodBase.GetCurrentMethod().DeclaringType.FullName + MethodBase.GetCurrentMethod().Name + miembroId + mostrarCelulasSinLider;
            List<int> registros = Cache.Instance.Obtener<List<int>>(key);
            if (registros == null)
            {
                List<int> rtn = new List<int>();

                //Validamos si el usuario actual tiene el permiso especial de poder ver los mismos datos que su lider; de ser asi, reemplazamos el ID a utilizar para cargar las celulas
                if (SesionActual.Instance.ValidarPermisoEspecial((int)PermisosEspeciales.VerMismosDatosQueLider, false))
                {
                    Miembro miembro = ManejadorDeMiembros.ObtenerMiembroActual();
                    miembroId = miembro.Celula.CelulaLider.First(o => o.Borrado == false).MiembroId;
                }

                List<int> celulasHijas = ObtenerCelulasHijas(miembroId); //Celulas de las que el usuario es lider  
                List<int> celulasAnidadas = ObtenerCelulasAnidadas(celulasHijas); //Celulas de las que el usuario es lider del lider

                //Borramos las celulas hijas de las celulas anidadas ya que NO deberian de existir; esto unicamente sucede en usuarios que son lideres de la misma celula a la que asisten
                celulasAnidadas.RemoveAll(o => celulasHijas.Contains(o));

                rtn.AddRange(celulasHijas);

                while (celulasAnidadas.Except(celulasHijas).Count() > 0)
                {
                    rtn.AddRange(celulasAnidadas);
                    celulasHijas = celulasAnidadas;
                    celulasAnidadas = ObtenerCelulasAnidadas(celulasAnidadas);
                }

                registros = rtn.Distinct().ToList<int>();
                Cache.Instance.Guardar(key, registros);
            }
            return registros;
        }

        #region Metodos Privados

        private List<int> ObtenerCelulasHijas(int miembroId)
        {
            //Obtenemos las celulas de las que el usuario es lider
            List<int> celulasHijas = (
                from c in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                join l in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                    on c.CelulaId equals l.CelulaId
                where ((c.Borrado == false) && (l.Borrado == false)) && (l.MiembroId == miembroId)
                select c.CelulaId)
                .ToList<int>();

            return celulasHijas;
        }

        private List<int> ObtenerCelulasAnidadas(List<int> celulas)
        {
            List<int> celulasAnidadas;

            if (celulas.Count > 0)
            {
                celulasAnidadas = (
                    from c in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                    join l in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                        on c.CelulaId equals l.CelulaId
                    where ((c.Borrado == false) && (l.Borrado == false)) && celulas.Contains(l.Miembro.CelulaId)
                    select c.CelulaId).Distinct().ToList<int>();
            }
            else
            {
                celulasAnidadas = new List<int>();
            }

            return celulasAnidadas;
        }

        #endregion

        #endregion

        #region Redes por Miembro

        public List<RegistroBasico> ObtenerRedesPermitidasPorMiembro(int miembroId)
        {
            List<int> celulas = ObtenerCelulasPermitidasPorMiembroComoIds(miembroId);
            return (from c in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                    where celulas.Contains(c.CelulaId) &&
                    c.Miembro.Any(o => (o.CelulaLider.Count(u => u.Borrado == false) > 0))
                    orderby c.Descripcion
                    select new RegistroBasico
                    {
                        Id = c.CelulaId,
                        Descripcion = c.Descripcion
                    }).ToList<RegistroBasico>();
        }

        #endregion

        #region Red

        /// <summary>
        /// Obtiene un diccionario con todas las redes de mas alto nivel, junto con su lista completa de celulas hijas
        /// </summary>
        /// <returns></returns>
        public Dictionary<Celula, List<int>> ObtenerRedes()
        {
            // LLenamos la lista que contiene las redes
            List<int> idsDeRedes = new List<int> { 
                    2, // Jovenes
                    141, // Familiar
                    178, // Dorada
                    199 // Matrimonios Jovenes
                };
            Dictionary<Celula, List<int>> redes = new Dictionary<Celula, List<int>>();
            foreach (int id in idsDeRedes)
            {
                Celula celula = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula where o.CelulaId == id && o.Borrado == false select o).SingleOrDefault();
                if (celula != null)
                {
                    redes.Add(celula, this.ObtenerRedInferior(id));
                }
            }

            return redes;
        }

        /// <summary>
        /// Obtiene la lista de ids de las celulas que pertenecen a un red especifica, directamente abajo a partir de la celula definida como parametro de entrada
        /// </summary>
        /// <param name="celulaId">El id de la celula "padre" de la red a buscar</param>
        /// <returns>Lista de ids de las celulas de la red</returns>
        public List<int> ObtenerRedInferiorDirecta(int celulaId)
        {
            return (from liderzago in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider
                    join miembro in SesionActual.Instance.getContexto<IglesiaEntities>().Miembro on liderzago.MiembroId equals miembro.MiembroId
                    where
                    miembro.CelulaId == celulaId &&
                    miembro.Borrado == false &&
                    liderzago.Borrado == false &&
                    liderzago.Celula.Borrado == false
                    select liderzago.CelulaId).Distinct().ToList<int>();
        }

        /// <summary>
        /// Obtiene la lista de ids de las celulas que pertenecen a un red especifica, para abajo a partir de la celula definida como parametro de entrada
        /// </summary>
        /// <param name="celulaId">El id de la celula "padre" de la red a buscar</param>
        /// <returns>Lista de ids de las celulas de la red</returns>
        public List<int> ObtenerRedInferior(int celulaId)
        {
            List<int> red;
            int index = 0;

            Func<int, List<int>> obtenerRedInferiorDirecta = (celulaHijaId) =>
            {
                index++;
                return this.ObtenerRedInferiorDirecta(celulaHijaId);
            };

            red = obtenerRedInferiorDirecta(celulaId);

            if (red.Any())
            {
                do
                {
                    red.AddRange(obtenerRedInferiorDirecta(red[index - 1]));

                    // Valores unicos
                    red = red.Distinct().ToList<int>();

                } while (index <= red.Count);

                // Ordemos de menor a mayor
                red.Sort();
            }

            return red;
        }

        /// <summary>
        /// Obtiene la lista de celulas que pertenecen a un red especifica, para arriba a partir de la celula definida como parametro de entrada
        /// </summary>
        /// <param name="celula"></param>
        /// <returns></returns>
        public List<Celula> ObtenerRedSuperior(Celula celula)
        {
            List<Celula> red = new List<Celula>();

            Celula celulaPadre = ObtenerCelulaPadre(celula);
            Celula celulaHija = celula;
            red.Add(celulaHija);
            do
            {
                red.Add(celulaPadre);
                celulaHija = celulaPadre;
                celulaPadre = ObtenerCelulaPadre(celulaHija);

            } while (celulaPadre != celulaHija);

            red.Remove(red.Last()); //Quitamos el ultimo valor ya que es el "Sin Celula"
            red.Reverse();
            return red;
        }

        /// <summary>
        /// Obtiene una cadena que tiene todas celulas "padres" a partir de la celula definida como parametro de entrada
        /// </summary>
        /// <param name="celula"></param>
        /// <param name="separador"></param>
        /// <returns></returns>
        public string ObtenerRedSuperior(Celula celula, string separador)
        {
            separador = string.Format(" {0} ", separador.Trim());
            System.Text.StringBuilder rtn = new System.Text.StringBuilder(1024);
            ManejadorDeCelulas manejador = new ManejadorDeCelulas();

            List<Celula> red = manejador.ObtenerRedSuperior(celula);
            foreach (Celula c in red)
            {
                rtn.AppendFormat("{0} [{1}]", c.Descripcion, c.CelulaId);

                if (c != red.Last())
                {
                    rtn.Append(separador);
                }
            }

            return rtn.ToString();
        }

        /// <summary>
        /// Obtiene la celula "padre" directa de la celula definida como parametro de entrada
        /// </summary>
        /// <param name="celula"></param>
        /// <returns></returns>
        public Celula ObtenerCelulaPadre(Celula celula)
        {
            return (from o in SesionActual.Instance.getContexto<IglesiaEntities>().CelulaLider where o.Borrado == false && o.CelulaId == celula.CelulaId select o.Miembro.Celula).FirstOrDefault();
        }

        #endregion

        #region Buscador

        public List<Modelos.Retornos.CelulaProxima> ObtenerCelulasProximas(double latitud, double longitud, int kilometrosRedonda, List<int> idCategoriasDeCelulas)
        {
            List<Modelos.Retornos.CelulaProxima> registros = new List<Modelos.Retornos.CelulaProxima>();

            Func<double, double> Radians = (val) =>
            {
                return val * Math.PI / 180;
            };

            Func<double, double, double, double, double> DistanceBetweenPlaces = (lon1, lat1, lon2, lat2) =>
            {
                double R = 6378.16; // km
                double sLat1 = Math.Sin(Radians(lat1));
                double sLat2 = Math.Sin(Radians(lat2));
                double cLat1 = Math.Cos(Radians(lat1));
                double cLat2 = Math.Cos(Radians(lat2));
                double cLon = Math.Cos(Radians(lon1) - Radians(lon2));
                double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;
                double d = Math.Acos(cosD);
                double dist = R * d;
                return dist;
            };

            List<Celula> celulas = (from o in SesionActual.Instance.getContexto<IglesiaEntities>().Celula
                                    where
                                    (idCategoriasDeCelulas.Contains(o.CategoriaId) || (idCategoriasDeCelulas.Count == 0)) &&
                                    o.Borrado == false &&
                                    o.Coordenadas != null
                                    select o).ToList();

            foreach (Celula celula in celulas)
            {
                String[] coordenadas = celula.Coordenadas.Replace("(", String.Empty).Replace(")", String.Empty).Split(',');
                double longitudCelula = double.Parse(coordenadas[1]);
                double latitudCelula = double.Parse(coordenadas[0]);

                if (longitudCelula != 0 && latitudCelula != 0 && DistanceBetweenPlaces(longitud, latitud, longitudCelula, latitudCelula) <= kilometrosRedonda)
                {
                    Modelos.Retornos.CelulaProxima registro = new Modelos.Retornos.CelulaProxima();

                    registro.Id = celula.CelulaId;
                    registro.Descripcion = celula.Descripcion;
                    registro.Dia = celula.DiaSemana.Descripcion;
                    registro.Hora = celula.HoraDia.Descripcion;
                    registro.Coordenadas = celula.Coordenadas;
                    registro.Municipio = celula.UbicacionMunicipio.Descripcion;
                    registro.Colonia = celula.Colonia;
                    registro.Direccion = celula.Direccion;

                    registros.Add(registro);
                }
            }

            return registros;
        }

        #endregion
    }
}
