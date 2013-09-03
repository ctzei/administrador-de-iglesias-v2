var idCargado = -1;
var esSoloLectura = false;

Catalogo = function () {
    return Ext.apply(new Ext.util.Observable(), {
        events: { 'buscar': true, 'limpiar': true, 'mostrar': true, 'borrar': true, 'guardar': true, 'cancelar': true, }
    });
}();

/* Mensajes para el usuario */

var msgs = {};
msgs.sinPanelDeFiltros = "La pagina de contenido que usa este MasterPage debe de tener un panel llamado 'pnlFiltros' dentro del elemento 'cphFiltros'.";
msgs.sinPanelDeEdicion = "La pagina de contenido que usa este MasterPage debe de tener un panel llamado 'pnlEdicion' dentro del elemento 'cphEdicion'.";
msgs.sinPanelDeResultados = "La pagina de contenido que usa este MasterPage debe de tener un panel llamado 'GridDeResultados' dentro del elemento 'cphGridDeResultados'.";
msgs.gridSinColumnaId = "El grid necesita tener la columna de Id, aunque sea en el 'Store'.";
msgs.filaNoSeleccionada = "Es necesario seleccionar alguna fila.";
msgs.borrarRegistroActual = "El registro actual [Id:@id@] sera eliminado, ¿Desea continuar?";
msgs.borrarFilaSeleccionada = "La fila seleccionada [Id:@id@] sera eliminada, ¿Desea continuar?";
msgs.elementoNoSeleccionadoAlBorrar = "Es necesario seleccionar alguna fila ó cargar algún registro.";

/* Funciones GENERALES */

function obtenerPanelDeFiltros() {
    o = Ext.getCmp('cphMain_cphFiltros_pnlFiltros');
    if (o == null) { o = Ext.getCmp("cphMain_cphFiltros_Filtros_pnlFiltros"); } //Puede que sea un UserControl... en cuyo caso tendra que llamarse "Filtros"
    if (o == null) { o = Ext.getCmp("cphMain_pnlFiltros"); } //Puede que sea una pagina especifica (NO CATALOGO)...
    return o;
}

function obtenerPanelDeEdicion() {
    o = Ext.getCmp('cphMain_cphEdicion_pnlEdicion');
    if (o == null) { o = Ext.getCmp("cphMain_cphEdicion_Edicion_pnlEdicion"); } //Puede que sea un UserControl... en cuyo caso tendra que llamarse "Edicion"
    if (o == null) { o = Ext.getCmp("cphMain_pnlEdicion"); } //Puede que sea una pagina especifica (NO CATALOGO)...
    return o;
}

function obtenerFormDeFiltros() {
    o = obtenerPanelDeFiltros();
    if (o == null) { alert(msgs.sinPanelDeFiltros); }
    return o.getForm();
}

function obtenerFormDeEdicion() {
    o = obtenerPanelDeEdicion();
    if (o == null) { alert(msgs.sinPanelDeEdicion); }
    return o.getForm();
}

function obtenerGridDeResultados() {
    o = Ext.getCmp('cphMain_cphGridDeResultados_GridDeResultados');
    if (o == null) { o = Ext.getCmp("cphMain_cphGridDeResultados_Resultados_GridDeResultados"); } //Puede que sea un UserControl... en cuyo caso tendra que llamarse "Resultados"
    if (o == null) { alert(msgs.sinPanelDeResultados); }
    return o;
}

function obtenerIdDeFilaSeleccionada() {
    o = obtenerGridDeResultados().getSelectionModel();
    s = o.getSelected();
    if (s != null) {
        if (s.data.Id) {
            return parseInt(s.data.Id);
        }
        else {
            alert(msgs.gridSinColumnaId);
            return -1;
        }
    }
    else {
        return -1;
    }
}

function obtenerDatosExtraParaGuardar() {
    grids = obtenerGridsExtras();
    datosExtra = {};
    for (i = 0; i < grids.length; i++) {
        datosExtra[grids[i].id] = getModifiedRows(grids[i]);

        //Luego de obtener los datos del grid los damos por "commited", asi se reinician los modificados/eliminados/nuevos
        grids[i].store.commitChanges();
    }
    return datosExtra;
}

function obtenerGridsExtras() {
    grids = Ext.get("cphMain_pnlEdicion_Content").query("div.x-grid-panel");
    gridsExtendidos = [];
    for (i = 0; i < grids.length; i++) {
        gridsExtendidos[i] = Ext.getCmp(grids[i].id);
    }
    return gridsExtendidos
}

function obtenerTreesExtras() {
    trees = Ext.get("cphMain_pnlEdicion_Content").query("div.x-tree");
    treesExtendidos = [];
    for (i = 0; i < trees.length; i++) {
        treesExtendidos[i] = Ext.getCmp(trees[i].id);
    }
    return treesExtendidos
}

function deseleccionarTreesExtras() {
    trees = obtenerTreesExtras();
    if (trees.length > 0) {
        for (i = 0; i < trees.length; i++) {
            toggleChecksOnTree(trees[i], false);
        }
    }
}

function establecerModoSoloLectura(soloLectura) {
    botonDeGuardar = Ext.getCmp("cphMain_cmdGuardar");

    if (soloLectura) {
        obtenerFormDeEdicion().getEl().mask().setStyle("background-color", "rgb(255,255,255)");
        botonDeGuardar.setText("Editar");
    }
    else {
        obtenerFormDeEdicion().getEl().unmask();
        botonDeGuardar.setText("Guardar");
    }

    esSoloLectura = soloLectura;
}

/* Eventos de BOTONES */

function buscarClick() {
    if (obtenerFormDeFiltros().isValid()) {
        Ext.net.DirectMethods.ctl00.Buscar();
    }
    Catalogo.fireEvent('buscar');
    return false;
}

function limpiarClick() {
    resetForm(obtenerFormDeFiltros().id);
    Catalogo.fireEvent('limpiar');
    return false;
}

function mostrarClick() {
    id = obtenerIdDeFilaSeleccionada();
    idCargado = id;
    if (id > 0) {
        tabs = Ext.getCmp("cphMain_pnlSuperior");
        tabs.setActiveTab(1);
        Ext.net.DirectMethods.ctl00.Mostrar(id);
        establecerModoSoloLectura(true);
    }
    else {
        Ext.Msg.alert('', msgs.filaNoSeleccionada);
    }
    Catalogo.fireEvent('mostrar');
    return false;
}

function borrarClick() {
    filaSeleccionada = obtenerIdDeFilaSeleccionada();

    idBorrar = -1;
    msgBorrar = "";
    if (idCargado > 0) {
        idBorrar = idCargado;
        msgBorrar = msgs.borrarRegistroActual;
    }
    else if (filaSeleccionada > 0) {
        idBorrar = filaSeleccionada;
        msgBorrar = msgs.borrarFilaSeleccionada;
    }

    if (idBorrar > 0) {
        Ext.Msg.confirm('Borrar', msgBorrar.replace("@id@", idBorrar), function (btn) {
            if (btn == 'yes') {
                cancelarClick();
                Ext.net.DirectMethods.ctl00.Borrar(idBorrar);
            }
        });
    }
    else {
        Ext.Msg.alert('', msgs.elementoNoSeleccionadoAlBorrar);
    }
    Catalogo.fireEvent('borrar');
    return false;
}

function guardarClick() {
    if (esSoloLectura == false) {
        if (obtenerFormDeEdicion().isValid()) {
            //Ext.net.DirectMethods.ctl00.Guardar(idCargado, obtenerDatosExtraParaGuardar(), { callback: cancelarClick });
            Ext.net.DirectMethods.ctl00.Guardar(idCargado, obtenerDatosExtraParaGuardar());
        }
    }
    else {
        establecerModoSoloLectura(false);
    }
    Catalogo.fireEvent('guardar');
    return false;
}

function cancelarClick() {
    resetForm(obtenerFormDeEdicion().id);
    idCargado = -1;
    deseleccionarTreesExtras();
    establecerModoSoloLectura(false);
    if (obtenerGridsExtras().length > 0) {
        Ext.net.DirectMethods.ctl00.RecargarControles();
    }
    Catalogo.fireEvent('cancelar');
    return false;
}

/* MISC */

function reordenarPanelesBasicos() {
    var a = ((Ext.isIE) && !(Ext.isIE9));
    filtros = obtenerPanelDeFiltros();
    if (filtros) {
        filtros.setWidth(Ext.getBody().getWidth(true));
        if (filtros.layout) {
            filtros.doLayout()
        }

    }
    edicion = obtenerPanelDeEdicion();
    if (edicion) {
        edicion.setWidth(Ext.getBody().getWidth(true));
        if (edicion.layout) {
            edicion.doLayout()
        }

    }
}

function cambiarTab() {
    reordenarPanelesBasicos();
    Ext.EventManager.fireResize()
}

function procesarEnters() {
    //A todas las pantallas de "filtros" le activamos que al presionar ENTER se ejecuta la opcion de "buscar"
    var campos = obtenerFormDeFiltros().items.items;
    for (var i = 0; i < campos.length; i++) {
        var campo = campos[i];

        if (campo.xtype.toLowerCase() != 'compositeField'.toLowerCase()) {
            processOnEnter(campo.id, "cphMain_cmdBuscar");
        }
        else {
            for (var j = 0; j < campo.items.length; j++) {
                processOnEnter(campo.items.items[j].id, "cphMain_cmdBuscar");   
            }
        }
    }
}

/* Inicializadores */

Ext.onReady(function () {
    Ext.EventManager.onWindowResize(reordenarPanelesBasicos)
    Ext.EventManager.fireResize(); //Para eliminar el bug donde no se carga correctamente el layout

    procesarEnters();

    //Seleccionamos el primer campo... de existir...
    var items = obtenerFormDeFiltros().items.items;
    if (items && items.length > 0) {
        items[0].focus('', 10); 
    }
});