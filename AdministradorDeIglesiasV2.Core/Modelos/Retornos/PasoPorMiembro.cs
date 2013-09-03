using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class PasoPorMiembro
    {
        public int CicloId { get; set; }
        public string Ciclo { get; set; }
        public int PasoId { get; set; }
        public string Paso { get; set; }
        public int CategoriaId { get; set; }
        public string Categoria { get; set; }
    }
}
