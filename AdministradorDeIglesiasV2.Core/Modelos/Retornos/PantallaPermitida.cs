using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class PantallaPermitida
    {
        public int Id { get; set; }
        public bool Marcado { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string AppId { get; set; }
    }
}
