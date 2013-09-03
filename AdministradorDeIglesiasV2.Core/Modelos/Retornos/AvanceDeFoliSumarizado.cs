using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class AvanceDeFoliSumarizado
    {
        public int CantidadDeRegistros { get; set; }
        public int CantidadDeAsistencias { get; set; }
        public List<AvanceDeFoli> Avances { get; set; }
    }
}
