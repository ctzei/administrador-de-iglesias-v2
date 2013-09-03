function validarTelefonos(value) {
    telCasa = Ext.getCmp('cphMain_cphEdicion_registroTelCasa');
    telMovil = Ext.getCmp('cphMain_cphEdicion_registroTelMovil');
    telTrabajo = Ext.getCmp('cphMain_cphEdicion_registroTelTrabajo');

    if ((value.toString().trim().length <= 0) && (telMovil.getValue().toString().trim().length <= 0) && (telTrabajo.getValue().toString().trim().length <= 0)) {
        return 'Cuando menos uno de los teléfonos es requerido.';
    } else {
        return true;
    }
}

function determinarEmailFicticio() {
    noTieneEmail = Ext.getCmp('cphMain_cphEdicion_registroNoTieneEmail');
    email = Ext.getCmp('cphMain_cphEdicion_registroEmail');
    if (noTieneEmail.getValue()) {
        email.setReadOnly(true);
        primerNombre = Ext.getCmp('cphMain_cphEdicion_registroPrimerNombre');
        apellidoPaterno = Ext.getCmp('cphMain_cphEdicion_registroApellidoPaterno');
        fechaDeNacimiento = Ext.getCmp('cphMain_cphEdicion_registroFechaDeNacimiento');

        correoFicticio =
            primerNombre.getValue() + '.' +
            apellidoPaterno.getValue() + '.' +
            ((fechaDeNacimiento.getValue().dateFormat) ? fechaDeNacimiento.getValue().dateFormat('dmY') : '000000') +
            '@correo-e.com'

        email.setValue(correoFicticio);
    }
    else {
        email.setReadOnly(false);
    }
}


function cargarDatosDeBoletaAnterior() {
    Ext.Msg.confirm('Cargar', 'Los datos actuales serán reemplazados por los de la boleta anterior. Continuar?', function (btn) {
        if (btn == 'yes') {
            cancelarClick();
            Ext.net.DirectMethods.CargarDatosDeBoletaAnterior();
        }
    });
}



Ext.onReady(function () {
    var gridDeReportes = Ext.getCmp("cphMain_cphEdicion_GridDeReportes").getEl();
    gridDeReportes.mask().setStyle("background-color", "rgb(255,255,255)");

    Catalogo.on('cancelar', function (d) {
        gridDeReportes.mask().setStyle("background-color", "rgb(255,255,255)");
    })

    Catalogo.on('mostrar', function (d) {
        gridDeReportes.unmask();
    })
});

