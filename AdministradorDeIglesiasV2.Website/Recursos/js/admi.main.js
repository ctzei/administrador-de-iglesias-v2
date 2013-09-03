ADMI = new function(){
  
    this.AbrirVentanaModal = function (titulo, liga) {
        new Ext.Window({
            title: titulo,
            closable: true,
            closeAction: 'close',
            width: Ext.getBody().getViewSize().width * 0.96,
            height: Ext.getBody().getViewSize().height * 0.96,
            plain: true,
            layout: 'fit',
            modal: true,
            draggable: false,
            resizable: false,
            items: [new Ext.Panel({
                layout: 'form',
                closable: false,
                autoLoad: {
                    mode: "iframe",
                    url: liga
                }
            })]
        }).show();
    };


    this.VerDetallesDeMiembro = function (miembroId) {
        this.AbrirVentanaModal("Detalles de Miembro", ResolveUrl("~/Paginas/Miembros/DetallesDeMiembro.aspx?id=" + miembroId));
    };

    this.VerDetallesDeCelula = function (celulaId) {
        this.AbrirVentanaModal("Detalles de Célula", ResolveUrl("~/Paginas/Células/DetallesDeCelula.aspx?id=" + celulaId));
    };

    this.VerDetallesDeEvento = function (eventoId) {
        this.AbrirVentanaModal("Detalles de Evento", ResolveUrl("~/Paginas/Alabanza/DetallesDeEvento.aspx?id=" + eventoId));
    };

    this.VerDetallesDeEnsayo = function (ensayoId) {
        this.AbrirVentanaModal("Detalles de Ensayo", ResolveUrl("~/Paginas/Alabanza/DetallesDeEnsayo.aspx?id=" + ensayoId));
    };

    this.VerDetallesDeCancion = function (cancionId) {
        this.AbrirVentanaModal("Detalles de Cancion", ResolveUrl("~/Paginas/Alabanza/DetallesDeCancion.aspx?id=" + cancionId));
    };

};

//Desactivamos ciertas teclas necesarias
Ext.onReady(function () {
    document.onkeydown = function (e) {
        //return false; = EVITA LA TECLA

        if (!window.event) { window.event = {}; }
        if (e) {

            //Aqui evitamos el F5
            //116->F5
            if (e.keyCode == 116) {
                e.keyCode = 505;
                window.event.keyCode = 505;
                return false;
            }

            //Aqui evitamos el BACKSPACE
            if (e.keyCode == 8) {
                el = document.activeElement;
                val = el.value;

                if (val == undefined) {
                    return false;
                }
                else {
                    if ((val.length == 0) || (el.readOnly)) {
                        return false;
                    }
                    else {
                        val.keyCode = 8;
                    }
                }
            }
        }
    };
});