var resultadoMiembros = {
    filaNoSeleccionada: 'Es necesario seleccionar una fila antes de continuar.'
};

function mostrarInformacionExtra(grid, seleccion) {
    if (seleccion) {
        ADMI.VerDetallesDeMiembro(seleccion.id);
    }
    else {
        Ext.Msg.alert('', resultadoMiembros.filaNoSeleccionada);
    }
}