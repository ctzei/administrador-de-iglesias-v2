using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core.Modelos.Retornos
{
    public class InformacionGeneralPorRed
    {
        public int Id { get; set; }
        public int CantidadDeCelulas { get; set; }
        public int CantidadDeLideresDeCelula { get; set; }
        public int CantidadDeEstacas { get; set; }
        public int CantidadDeMiembros { get; set; }
        public int CantidadDeMiembrosHombres { get; set; }
        public int CantidadDeMiembrosMujeres { get; set; }
        public int CantidadDeMiembrosQueAsistenIglesia { get; set; }
        public int CantidadDeMiembrosQueAsistenIglesiaHombres { get; set; }
        public int CantidadDeMiembrosQueAsistenIglesiaMujeres { get; set; }
        public int CantidadDeFolis { get; set; }
    }
}
