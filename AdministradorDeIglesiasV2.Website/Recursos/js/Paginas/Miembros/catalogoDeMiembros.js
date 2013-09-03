var tiposDeImagenesSoportadas = ["png", "gif", "tiff", "bmp", "jpg", "jpeg"];

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

function obtenerFotoUploadForm() {
    return Ext.getCmp('cphMain_cphEdicion_pnlFoto').getForm();
}

function establecerFotoDeMiembro(miembroId) {
    uploadForm = obtenerFotoUploadForm();
    Ext.getCmp('cphMain_cphEdicion_registroFoto').setImageUrl(uploadForm.url + '?miembroId=' + miembroId + '&guid=' + Math.random());
}

function registroFotoClick() {
    if (idCargado > 0) {
        establecerFotoDeMiembro(idCargado);
        Ext.getCmp('cphMain_cphEdicion_wndFoto').show();
    }
    else {
        Ext.Msg.alert('', "Solo es posible ver/agregar la foto de registros previamente cargados.");
    }
}

function mandarArchivoAServidor() {
    uploadForm = obtenerFotoUploadForm();
    if (uploadForm) {
        uploadField = Ext.getCmp('cphMain_cphEdicion_registroUploadFoto');
        if (tiposDeImagenesSoportadas.indexOf(getFileExtension(uploadField.value)) >= 0) {
            Ext.Msg.confirm('Cargar', '¿Está seguro que desea cargar la nueva foto? La foto anterior será reemplazada. Esta operación no se puede deshacer.', function (btn) {
                if (btn == 'yes') {
                    uploadForm.submit({
                        url: uploadForm.url + '?miembroId=' + idCargado,
                        waitMsg: 'Cargando... Favor de esperar...',
                        success: function (fp, o) {
                            establecerFotoDeMiembro(idCargado);
                        },
                        failure: function (form, action) {
                            switch (action.failureType) {
                                case Ext.form.Action.CLIENT_INVALID:
                                    Ext.Msg.alert('Error', 'Alguno de los valores contiene datos inválidos. Favor de volver a intentar.');
                                    break;
                                case Ext.form.Action.CONNECT_FAILURE:
                                    Ext.Msg.alert('Error', 'El servidor no respondió en el tiempo requerido. Favor de volver a intentar.');
                                    break;
                                case Ext.form.Action.SERVER_INVALID:
                                    Ext.Msg.alert('Error', action.result.error);
                            }
                        }
                    });
                }
            });
        }
        else {
            Ext.Msg.alert('Error', "El formato del archivo cargado no es válido.");   
        }
    }

}