using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class FichaDeInscripcionDeEvento
    {
        public int Folio { get; set; }
        public bool Cerrada { get; set; }
        public int? RefNum { get; set; }
        public string RefAlfa { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Email { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Tel { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Genero { get; set; }
        public string TipoDeRegistrante { get; set; }
        public string InfoExtraDeRegistrate { get; set; }
    }
}
