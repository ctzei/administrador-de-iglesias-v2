Ext.onReady(function () {
    grdAlumnos = Ext.getCmp('cphMain_grdAlumnos');
    regNumeroDeAsistencias = Ext.getCmp('cphMain_registroNumeroDeAsistencias');
    regNumeroDeAlumnos = Ext.getCmp('cphMain_registroNumeroDeAlumnos');
    pnlFiltros = Ext.getCmp('cphMain_pnlFiltros');
});

function limpiarPantalla() {
    grdAlumnos.store.removeAll();
    regNumeroDeAsistencias.setValue('');
    regNumeroDeAlumnos.setValue('');
}

function mostrarAvance() {
    if (pnlFiltros.getForm().isValid()) {
        Ext.net.DirectMethods.MostrarAvanceClick(); 
    }
}

function guardarAvance() {
    if (pnlFiltros.getForm().isValid()) {
        Ext.net.DirectMethods.GuardarAvanceClick(getAllRows(grdAlumnos)); //Mandamos TODAS las flas, con cambios o sin cambios...
    }
}

function registroModificado(e) {
    if (e.field == 'Asistencia') {
        cambioDeAsistencia(e.value);
    }
}

function registroPresionado(e) {
    var cc = e.getCharCode();
    if (cc == 32) { //Si es la barra espaciadora...
        e.preventDefault();
        e.stopPropagation();
        e.stopEvent();

        var seleccion = grdAlumnos.selModel.getSelected();
        if (seleccion) {
            var store = grdAlumnos.store;

            //Invertimos el valor de la asistencia
            var registro = store.getById(seleccion.id), tuvoAsistencia = !registro.data.Asistencia;
            registro.set("Asistencia", tuvoAsistencia);
            cambioDeAsistencia(tuvoAsistencia);
        }
    }
}

function cambioDeAsistencia(tuvoAsistencia) {
    var asistenciasActuales = parseInt(regNumeroDeAsistencias.getValue());
    if (tuvoAsistencia == true) {
        asistenciasActuales++;
    }
    else {
        asistenciasActuales--;
    }
    regNumeroDeAsistencias.setValue(asistenciasActuales + " Asistencias");
}