function obtenerFormDeFiltros() {
    return Ext.getCmp('cphMain_pnlFiltros');
}

function ObtenerUltimasAsistenciasPorCelula() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        Ext.net.DirectMethods.ObtenerUltimasAsistenciasPorCelulaClick();
    }
}