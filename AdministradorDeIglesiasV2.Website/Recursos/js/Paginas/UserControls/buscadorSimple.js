if (buscadorSimple == null) {
    var buscadorSimple = {

        ObjetoBase: Ext.data.Record.create([
            { name: "Id", type: "int" },
            { name: "Descripcion", type: "string" }
        ]),

        seleccionarObjeto: function (gridDeEncontrados, objetoSeleccionado) {
            var seleccion = gridDeEncontrados.selModel.getSelected();

            if (seleccion) {
                var registro = seleccion.data;
                var o = new this.ObjetoBase({
                    Id: registro.Id,
                    Descripcion: registro.Descripcion
                }, registro.Id);

                var store = objetoSeleccionado.store;
                store.loadData([], false);
                store.add(o);
                objetoSeleccionado.setValue(registro.Id);
            }
        },

        verObjeto: function (objetoSeleccionado) {
            var tipo = objetoSeleccionado.TipoDeObjeto;
            var id = objetoSeleccionado.getValue();
            if (tipo && (id > 0)) {
                switch (tipo.toLowerCase()) {
                    case "miembro":
                        ADMI.VerDetallesDeMiembro(id);
                        break;
                    case "celula":
                        ADMI.VerDetallesDeCelula(id);
                        break;
                }
            }
            else {
                Ext.Msg.alert('', "Es necesario tener un valor seleccionado antes de poder ver su detalle.");
            }
        },

        borrarObjeto: function (objetoSeleccionado) {
            var store = objetoSeleccionado.store;
            store.loadData([], false);
            objetoSeleccionado.clear();
        },

        abrirVentanaDeBusqueda: function (objetoSeleccionado, ventanaDeBusqueda) {
            if (objetoSeleccionado.getValue() > 0) {
                Ext.Msg.confirm('', 'Al abrir la ventana de búsqueda se perderá la selección actual. ¿Continuar?', function (btn) {
                    if (btn == 'yes') {
                        buscadorSimple.borrarObjeto(objetoSeleccionado);
                        ventanaDeBusqueda.show();
                    }
                });
            }
            else {
                buscadorSimple.borrarObjeto(objetoSeleccionado);
                ventanaDeBusqueda.show();
            }
        }

    }
}