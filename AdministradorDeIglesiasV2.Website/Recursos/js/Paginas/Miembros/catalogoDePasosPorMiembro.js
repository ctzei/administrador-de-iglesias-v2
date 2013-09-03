function agregarPaso() {
    var pasoNuevoForm = Ext.getCmp('cphMain_cphEdicion_pnlNuevoPaso').getForm();
    if (pasoNuevoForm.isValid()) {

        var PasoBase = Ext.data.Record.create([
            { name: "Id", type: "int" },
            { name: "Paso", type: "string" },
            { name: "CategoriaId", type: "int" },
            { name: "Categoria", type: "string" },
            { name: "CicloId", type: "int" },
            { name: "Ciclo", type: "string" }
        ]);

        var ciclo = Ext.getCmp('cphMain_cphEdicion_registroNuevoCicloId');
        var paso = Ext.getCmp('cphMain_cphEdicion_registroNuevoPasoId');
        var categoria = Ext.getCmp('cphMain_cphEdicion_registroNuevoPasoCategoria');

        var pasoId = paso.getValue(); //PasoId es usado como ID de todo el grid, debe de ser el unico valor unico

        var gridDePasos = Ext.getCmp('cphMain_cphEdicion_registroPasos');
        var store = gridDePasos.store;

        var o = new PasoBase({
            Id: pasoId,
            Paso: paso.getText(),
            CategoriaId: categoria.getValue(),
            Categoria: categoria.getText(),
            CicloId: ciclo.getValue(),
            Ciclo: ciclo.getText()
        }, pasoId);

        //El nuevo registro lo marcamos como "sucio" para que al momento de enviar la informacion de nuevo al servidor, esta se mande
        o.markDirty();
        o.modified = { Id: -1 }; //Le marcamos un ID falso, para que en vdd se marque como "modificado"

        if (!store.getById(pasoId)) {
            store.add(o);
            //Ext.Msg.notify('Admi', 'Elemento agregado correctamente.');
        }
        else {
            Ext.Msg.notify('Admi', 'Paso previamente existente.');
        }

        return true;
    }
    else {
        return false;
    }
}

function cambiarCategoria() {
    var paso = Ext.getCmp('cphMain_cphEdicion_registroNuevoPasoId');
    var categoria = Ext.getCmp('cphMain_cphEdicion_registroNuevoPasoCategoria');
    var pasoId = paso.getValue();

    categoriaId = cphMain_cphStores_StorePasos.getById(pasoId).data['PadreId'];
    categoria.setValue(categoriaId);
}

function comandoDelGridDePasos(comando, registro, grid) {
    var store = grid.store;
    switch (comando.toLowerCase()){
        case "borrar":
            store.remove(registro);
            break;
    }
}