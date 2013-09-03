Ext.onReady(function () {
    qs = getQueryString();
    if (qs.pantallaNoPermitida) {
        Ext.Msg.alert('', 'Usuario con permisos insuficientes.');
    }

    inicializarManejadoresDeEnters();
    establecerFocoInicial();
});

function inicializarManejadoresDeEnters() {
    processOnEnter('cphMain_txtUsername', 'cphMain_txtPassword');
    processOnEnter('cphMain_txtPassword', 'cphMain_cmdAceptar');
    processOnEnter('cphMain_txtNewPassword', 'cphMain_cmdCambiarContrasena'); 
}

function establecerFocoInicial(){
    Ext.getCmp('cphMain_txtUsername').focus();
}