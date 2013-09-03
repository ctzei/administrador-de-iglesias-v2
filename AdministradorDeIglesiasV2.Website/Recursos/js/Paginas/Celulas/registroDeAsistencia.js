function limpiarPantalla() {
    Ext.getCmp('cphMain_grdAsistencias').store.removeAll();
    Ext.getCmp('cphMain_registroNumeroDeAsistencias').setValue('');
    Ext.getCmp('cphMain_registroNumeroDeMiembros').setValue('');
}

function obtenerFormDeFiltros(){
    return Ext.getCmp('cphMain_pnlFiltros');
}

function obtenerFormDeRazonDeCancelacion(){
    return Ext.getCmp('cphMain_pnlRazonCancelacion');
}

function obtenerVentanaDeRazonDeCancelacion(){
    return Ext.getCmp('cphMain_wndRazonCancelacion');
}

function mostrarAsistencia() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        Ext.net.DirectMethods.MostrarAsistenciaClick(); 
    }
}

function guardarAsistencia() {
    if (obtenerFormDeFiltros().getForm().isValid() && Ext.getCmp('cphMain_registroNumeroDeInvitados').getValue() >= 0) {
        Ext.net.DirectMethods.GuardarAsistenciaClick(getModifiedRows(Ext.getCmp('cphMain_grdAsistencias')));
    } else {
        Ext.Msg.alert('Admi', 'Parametros no válidos, es necesario seleccionar una célula, fecha y número de invitados válidos para continuar.');
    }
}

function mostrarRazonDeCancelacion() {
    if (obtenerFormDeFiltros().getForm().isValid()) {
        obtenerVentanaDeRazonDeCancelacion().show();
    }
}

function mostrarUltimaAsistencia() {
    Ext.net.DirectMethods.MostrarUltimaAsistenciaClick(); 
}

function confirmarCancelacionDeAsistencia() {
    if (obtenerFormDeRazonDeCancelacion().getForm().isValid()) {
        Ext.net.DirectMethods.CancelarAsistenciaClick();
        obtenerVentanaDeRazonDeCancelacion().hide();
        obtenerFormDeRazonDeCancelacion().getForm().reset();
    }
}

function registroModificado(e) {
    if (e.field == 'Asistencia') {
        cambioDeAsistencia(e.value);
    }
}

function cambioDeAsistencia(tuvoAsistencia) {
    var campoDeAsistencias = Ext.getCmp('cphMain_registroNumeroDeAsistencias');
    var asistenciasActuales = parseInt(campoDeAsistencias.getValue());
    if (tuvoAsistencia == true) {
        asistenciasActuales++;
    }
    else {
        asistenciasActuales--;
    }
    campoDeAsistencias.setValue(asistenciasActuales + " Asistencias");
}

function registroPresionado(e) {
    var cc = e.getCharCode();
    if (cc == 32) { //Si es la barra espaciadora...
        e.preventDefault();
        e.stopPropagation();
        e.stopEvent();

        var seleccion = Ext.getCmp('cphMain_grdAsistencias').selModel.getSelected();
        if (seleccion) {
            var store = Ext.getCmp('cphMain_grdAsistencias').store;

            //Invertimos el valor de la asistencia
            var registro = store.getById(seleccion.id), tuvoAsistencia = !registro.data.Asistencia;
            registro.set("Asistencia", tuvoAsistencia);
            cambioDeAsistencia(tuvoAsistencia);
        }
    }
}