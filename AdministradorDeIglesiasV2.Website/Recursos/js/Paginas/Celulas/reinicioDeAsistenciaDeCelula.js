function obtenerFormDeFiltros() {
    return Ext.getCmp('cphMain_pnlFiltros');
}

function reiniciarCelula() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        Ext.Msg.confirm('Cargar', '¿Está seguro que desea reiniciar la asistencia de la célula? Esta operación no se puede deshacer.', function (btn) {
            if (btn == 'yes') {
                Ext.net.DirectMethods.ReinciarAsistenciaDeCelulaClick();
            }
        });
    }
}