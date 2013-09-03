using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZagueEF.Core;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class ReporteDeAsistenciasDeCelulaSumarizado
    {
        public List<AsistenciaDeCelula> AsistenciasTotales { get { return obtenerAsistenciasTotales(); } }
        public List<AsistenciaDeCelula> AsistenciasPorSemana { get; private set; }
        public DateTime FechaInicial { get; private set; }
        public DateTime FechaFinal { get; private set; }
        public List<Fecha> Fechas { get; private set; }

        private List<AsistenciaDeCelula> asistenciasTotales;

        public ReporteDeAsistenciasDeCelulaSumarizado()
        {
            this.AsistenciasPorSemana = new List<AsistenciaDeCelula>();
            this.FechaInicial = DateTime.MaxValue;
            this.FechaFinal = DateTime.MinValue;
            this.Fechas = new List<Fecha>();
        }

        public void AgregarAsistencia(int celulaId, string descripcion, DateTime fechaInicial, DateTime fechaFinal, int asistencias, int cancelaciones, int faltas, int gente, int total)
        {
            #region Establecemos las fecha inicial/final global
            if (fechaInicial < this.FechaInicial)
            {
                this.FechaInicial = fechaInicial;
            }

            if (fechaFinal > this.FechaFinal)
            {
                this.FechaFinal = fechaFinal;
            }
            #endregion

            #region Agregamos las Asistencias por Semana

            AsistenciaDeCelula asistenciaDeCelula = this.AsistenciasPorSemana.Where(o => o.CelulaId == celulaId).SingleOrDefault();

            if (asistenciaDeCelula == null)
            {
                asistenciaDeCelula = new AsistenciaDeCelula();
                asistenciaDeCelula.CelulaId = celulaId;
                asistenciaDeCelula.Descripcion = descripcion;
                this.AsistenciasPorSemana.Add(asistenciaDeCelula);
            }

            Asistencia asistencia = new Asistencia();
            asistencia.Asistencias = asistencias;
            asistencia.Cancelaciones = cancelaciones;
            asistencia.Faltas = faltas;
            asistencia.Gente = gente;
            asistencia.Total = total;
            asistenciaDeCelula.Asistencias.Add(asistencia);

            Fecha fecha = this.Fechas.Where(o => o.FechaInicial == fechaInicial && o.FechaFinal == fechaFinal).SingleOrDefault();
            if (fecha == null)
            {
                fecha = new Fecha();
                fecha.FechaInicial = fechaInicial;
                fecha.FechaFinal = fechaFinal;
                this.Fechas.Add(fecha);
            }

            #endregion
        }

        private List<AsistenciaDeCelula> obtenerAsistenciasTotales()
        {
            //Si nunca se ha calculado las asistencias totales o si ya se habia calculado pero ya es OTRA cantidad de de asistencias por semana... se recalcula
            if (asistenciasTotales == null || (asistenciasTotales.Count != this.AsistenciasPorSemana.Count))
            {
                asistenciasTotales = new List<AsistenciaDeCelula>();

                foreach (AsistenciaDeCelula asistenciaPorSemana in this.AsistenciasPorSemana)
                {
                    AsistenciaDeCelula asistenciaDeCelulaTotal = new AsistenciaDeCelula();
                    asistenciaDeCelulaTotal.CelulaId = asistenciaPorSemana.CelulaId;
                    asistenciaDeCelulaTotal.Descripcion = asistenciaPorSemana.Descripcion;

                    Asistencia asistenciaTotal = new Asistencia();
                    foreach (Asistencia asistencia in asistenciaPorSemana.Asistencias)
                    {
                        asistenciaTotal.Asistencias += asistencia.Asistencias;
                        asistenciaTotal.Cancelaciones += asistencia.Cancelaciones;
                        asistenciaTotal.Faltas += asistencia.Faltas;
                        asistenciaTotal.Gente += asistencia.Gente;
                        asistenciaTotal.Total += asistencia.Total;
                    }

                    //Calculamos los promedios...
                    asistenciaTotal.Asistencias = asistenciaTotal.Asistencias / asistenciaPorSemana.Asistencias.Count;
                    asistenciaTotal.Cancelaciones = asistenciaTotal.Cancelaciones / asistenciaPorSemana.Asistencias.Count;
                    asistenciaTotal.Faltas = asistenciaTotal.Faltas / asistenciaPorSemana.Asistencias.Count;
                    asistenciaTotal.Gente = asistenciaTotal.Gente / asistenciaPorSemana.Asistencias.Count;

                    asistenciaDeCelulaTotal.Asistencias.Add(asistenciaTotal);
                    asistenciasTotales.Add(asistenciaDeCelulaTotal);
                } 
            }



            return asistenciasTotales;
        }

        public class AsistenciaDeCelula
        {
            public int CelulaId { get; set; }
            public string Descripcion { get; set; }
            public List<Asistencia> Asistencias { get; set; }

            public AsistenciaDeCelula()
            {
                this.Asistencias = new List<Asistencia>();
            }
        }

        public class Fecha
        {
            public DateTime FechaInicial  { get; set; }
            public DateTime FechaFinal  { get; set; }
        }

        public class Asistencia
        {
            public int Asistencias { get; set; }
            public int Cancelaciones { get; set; }
            public int Faltas { get; set; }
            public int Gente { get; set; }
            public int Total { get; set; }
        }
    }
}
