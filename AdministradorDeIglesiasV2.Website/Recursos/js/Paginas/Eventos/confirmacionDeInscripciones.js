function limpiarGridDeFichas() {
    Ext.getCmp('cphMain_grdFichas').store.removeAll();
}

function obtenerFormDeFiltros() {
    return Ext.getCmp('cphMain_pnlFiltros');
}

function mostrarFichas() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        Ext.net.DirectMethods.MostrarFichasClick();
    }
}

function guardarCambios() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        Ext.net.DirectMethods.GuardarCambiosClick(getModifiedRows(Ext.getCmp('cphMain_grdFichas')));
    }
}