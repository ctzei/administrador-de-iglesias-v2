/// <reference path="../../ExtNET-Intellisense.js" />

if (buscador == null) {
    var buscador = {

        ObjetoBase: Ext.data.Record.create([
            { name: "Id", type: "int" },
            { name: "Descripcion", type: "string" }
        ]),

        agregarObjeto: function (gridDeEncontrados, gridDeListados) {
            var seleccion = gridDeEncontrados.selModel.getSelected();

            if (seleccion) {
                var registro = seleccion.data;
                var o = new this.ObjetoBase({
                    Id: registro.Id,
                    Descripcion: registro.Descripcion
                }, registro.Id);

                //El nuevo registro lo marcamos como "sucio" para que al momento de enviar la informacion de nuevo al servidor, esta se mande
                o.markDirty();
                o.modified = { Id: -1 }; //Le marcamos un ID falso, para que en vdd se marque como "modificado"

                var store = gridDeListados.store;

                //Si el grid de listados es de 1 solo registro a la vez entonces se borran TODOS los datos antes de agregar el nuevo registro
                if (gridDeListados.registroSimple) {
                    store.loadData([],false);
                }

                if (!store.getById(registro.Id)) {
                    store.add(o);
                    //Ext.Msg.notify('Admi', 'Elemento agregado correctamente.');
                }
                else {
                    Ext.Msg.notify('Admi', 'Elemento previamente existente.');
                }
            }
        },

        borrarObjeto: function (grid, registro) {
            var store = grid.store;
            store.remove(registro);
        },

        comandoDelGridDeListadoDeObjetos: function (comando, registro, grid) {
            switch (comando.toLowerCase()) {
                case 'borrar':
                    this.borrarObjeto(grid, registro);
                    break;
            }
        },

        mostrarRegistroSimple: function (grid) {
            alert(grid);
            alert(Ext.getCmp(grid));
            var store = grid.store;

        }
    }
}