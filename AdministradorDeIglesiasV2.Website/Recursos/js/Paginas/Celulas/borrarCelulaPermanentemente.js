function obtenerFormDeFiltros() {
    return Ext.getCmp('cphMain_pnlFiltros');
}

function borrarCelula() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        Ext.Msg.confirm('Cargar', '¿Está seguro que desea borrar permanentemente la célula? Esta operación no se puede deshacer.', function (btn) {
            if (btn == 'yes') {
                Ext.net.DirectMethods.BorrarCelulaPermanentementeClick();
            }
        });
    }
}