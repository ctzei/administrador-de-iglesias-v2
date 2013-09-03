/* Funciones GENERALES */

function cargarPantalla(nombre, url) {
    tabPanel = Ext.getCmp('cphMain_pnlTabsDePantallas');

    Ext.net.Mask.show({ el: tabPanel });
    tab = tabPanel.add({
        title: nombre,
        layout: 'form',
        closable: true,
        autoLoad: {
            mode: "iframe",
            url: url,
            callback: function () {
                Ext.net.Mask.hide.defer(750);
            }
        }
    });
    tabPanel.setActiveTab(tab);
}

function cerrarSesion() {
    Ext.Msg.confirm('Cerrar Sesión', "¿Desea cerrar la sesión actual?", function (btn) {
        if (btn == 'yes') {
            Ext.net.DirectMethods.CerrarSesionActual();
        }
    });
}