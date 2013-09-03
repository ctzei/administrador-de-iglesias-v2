using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdministradorDeIglesiasV2.Core
{
    public enum PermisosEspeciales : int
    {
        CrearCelulas = 0,
        ModificarCelulas = 1,
        BorrarCelulas = 2,
        CrearMiembros = 3,
        ModificarMiembros = 4,
        BorrarMiembros = 5,
        AgregarAsistencias = 6,
        ModificarBorrarAsistencias = 7,
        VerMismosDatosQueLider = 8,
        CrearGruposDeFoli = 9,
        ModificarGruposDeFoli = 10,
        BorrarGruposDeFoli = 11,
        CrearBoletasDeConsolidacion = 12,
        ModificarBoletasDeConsolidacion = 13,
        BorrarBoletasDeConsolidacion = 14,
        VerTodasLasBoletasDeConsolidacion = 15
    }
}
