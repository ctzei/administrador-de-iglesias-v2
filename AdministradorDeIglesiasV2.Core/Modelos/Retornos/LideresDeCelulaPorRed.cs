using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class LideresDeCelulaPorRed
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string TelMovil { get; set; }
        public string TelCasa { get; set; }
        public string TelTrabajo { get; set; }
        public int CelulaId { get; set; }
        public string Celula { get; set; }
        public string Genero { get; set; }
    }
}
