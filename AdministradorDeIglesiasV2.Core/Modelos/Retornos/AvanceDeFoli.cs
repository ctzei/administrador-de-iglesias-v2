using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class AvanceDeFoli
    {
        public int Id { get; set; }
        public int FoliMiembroId { get; set; }
        public int MiembroId { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public bool Asistencia { get; set; }
        public bool Tarea { get; set; }
        public int CalificacionTarea { get; set; }
        public int CalificacionExamen { get; set; }
    }
}
