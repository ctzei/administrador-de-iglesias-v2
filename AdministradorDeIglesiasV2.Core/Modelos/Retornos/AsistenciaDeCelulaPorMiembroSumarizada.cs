using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class AsistenciaDeCelulaPorMiembroSumarizada
    {
        public int CantidadDeRegistros { get; set; }
        public int CantidadDeAsistencias { get; set; }
        public List<AsistenciaDeCelulaPorMiembro> Asistencias { get; set; }

        public AsistenciaDeCelulaPorMiembroSumarizada()
        {
        }

        public AsistenciaDeCelulaPorMiembroSumarizada(List<AsistenciaDeCelulaPorMiembro> asistencias)
        {
            this.CantidadDeRegistros = asistencias.Count;
            this.CantidadDeAsistencias = 0;
            this.Asistencias = asistencias;

            foreach (AsistenciaDeCelulaPorMiembro asistencia in asistencias)
            {
                if (asistencia.Asistencia)
                {
                    this.CantidadDeAsistencias++;
                }
            }
        }

        /// <summary>
        /// Creamos un objeto que contiene las asistencias a partir de una lista de diccionarios (JSON)
        /// </summary>
        /// <param name="registros"></param>
        public AsistenciaDeCelulaPorMiembroSumarizada(List<Dictionary<string, string>> registros){
            this.CantidadDeRegistros = registros.Count;
            this.CantidadDeAsistencias = 0;
            this.Asistencias = new List<AsistenciaDeCelulaPorMiembro>();

            AsistenciaDeCelulaPorMiembro asistencia;
            foreach (Dictionary<string, string> registro in registros)
            {
                string sAsistencia = registro["Asistencia"];
                string sId = registro["Id"];
                string sMiembroId = registro["MiembroId"];
                string sPeticiones = registro["Peticiones"];

                bool tieneAsistencia;
                int id;
                int miembroId;

                if ((bool.TryParse(sAsistencia, out tieneAsistencia)) && (int.TryParse(sId, out id)) && (int.TryParse(sMiembroId, out miembroId)))
                {
                    asistencia = new AsistenciaDeCelulaPorMiembro();
                    asistencia.Id = id;
                    asistencia.MiembroId = miembroId;
                    asistencia.Asistencia = tieneAsistencia;
                    asistencia.Peticiones = sPeticiones;
                    this.Asistencias.Add(asistencia);

                    if (tieneAsistencia)
                    {
                        this.CantidadDeAsistencias++;
                    }
                }
                else
                {
                    throw new ExcepcionAplicacion("No es posible cargar la asistencia a partir de la lista de diccionarios (JSON). Alguno de los campos falta o contiene datos invalidos.");
                }
            }
        }
    }
}
