using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class CelulaProxima
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Dia { get; set; }
        public string Hora { get; set; }
        public string Coordenadas { get; set; }
        public string Municipio { get; set; }
        public string Colonia { get; set; }
        public string Direccion { get; set; }
    }
}
