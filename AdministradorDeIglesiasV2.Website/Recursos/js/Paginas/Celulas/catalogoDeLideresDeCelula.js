function seleccionarLiderzagoDeCelulas(celulas) {
    toggleChecksOnTree(Ext.getCmp("cphMain_cphEdicion_registroLiderazgoCelulasArbol"), false);
    for (i = 0; i < celulas.length; i++) {
        Ext.getCmp("cphMain_cphEdicion_registroLiderazgoCelulasArbol").getNodeById(celulas[i]).ui.toggleCheck(true);
    }
}