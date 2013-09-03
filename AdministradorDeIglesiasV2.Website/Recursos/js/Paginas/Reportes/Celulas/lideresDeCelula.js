function obtenerFormDeFiltros() {
    return Ext.getCmp('cphMain_pnlFiltros');
}

function obtenerLideresDeCelula() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
            Ext.net.DirectMethods.ObtenerLideresDeCelulaClick();
    }
}