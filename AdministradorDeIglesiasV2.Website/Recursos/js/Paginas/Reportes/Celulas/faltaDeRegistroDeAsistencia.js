function obtenerFormPrincipal() {
    o = Ext.getCmp('cphMain_pnlMain');
    return o.getForm();
}

function obtenerFormDeReporte() {
    o = Ext.getCmp('cphMain_pnlReporteGenerado');
    return o.getForm();
}

function habilitarControles() {
    inscribirse = Ext.getCmp("cphMain_chkInscribirse").getValue();
    Ext.getCmp("cphMain_cboDiaSemana").setDisabled(!inscribirse);
    Ext.getCmp("cphMain_cboDiaSemana").reset();
    Ext.getCmp("cphMain_cboHoraDia").setDisabled(!inscribirse);
    Ext.getCmp("cphMain_cboHoraDia").reset();
    Ext.getCmp("cphMain_cboTipoDeReporte").setDisabled(!inscribirse);
    Ext.getCmp("cphMain_cboTipoDeReporte").reset();
}

function guardarCambios() {
    if (obtenerFormPrincipal().isValid()) {
        Ext.net.DirectMethods.GuardarCambios(); 
    } 
}

function mostrarReporte() {
    Ext.getCmp('cphMain_wndReporte').show();
}

function limpiarReporte() {
    Ext.getCmp('cphMain_pnlHtml').clearContent();   
}

function generarReporte() {
    if (obtenerFormDeReporte().isValid()) {
        Ext.net.DirectMethods.GenerarReporte(); 
    } 
}

Ext.onReady(function () {
    habilitarControles();
});