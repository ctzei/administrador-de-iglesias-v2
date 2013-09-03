function obtenerFormDeFiltros() {
    o = Ext.getCmp('cphMain_pnlFiltros');
    if (o == null) { alert("La pagina de contenido que usa este MasterPage debe de tener un panel llamado 'pnlEdicion' dentro del elemento 'cphMain'."); }
    return o.getForm();
}

function buscarCelulasClick() {
    if (obtenerFormDeFiltros().isValid()) {
        municipio = Ext.getCmp("cphMain_filtroMunicipio").getText();
        colonia = Ext.getCmp("cphMain_filtroColonia").getValue();
        direccion = Ext.getCmp("cphMain_filtroDireccion").getValue();

        obtenerCoordenadasDesdeDireccion(municipio, colonia, direccion, generarMapaDeCelulasProximas, limpiarMapaDeCelulasProximas, limpiarMapaDeCelulasProximas);  
    }
}

function generarMapaDeCelulasProximas(results) {
    lat = results[0].geometry.location.lat();
    lng = results[0].geometry.location.lng();
    el = Ext.get("cphMain_grdCelulas").query("div.x-panel-body")[0];

    latlng = new google.maps.LatLng(lat, lng);
    cargarMapa(latlng, el, 14);
    agregarMarcador(latlng, "Direccion Base");

    Ext.net.DirectMethods.ObtenerCelulasProximas(lat, lng, {
        success: function (rtn) {
            for (i = 0; i < rtn.length; i++) {
                var reg = rtn[i];
                var descripcion = reg.Descripcion + ' \n' + reg.Dia + '@' + reg.Hora + ' \n[' + reg.Direccion + ', ' + reg.Colonia + ', ' + reg.Municipio + ']';
                with ({ registro: reg }) {
                    agregarMarcador(rtn[i].Coordenadas, descripcion, function () {
                        ADMI.VerDetallesDeCelula(registro.Id);
                    });
                }
            }

            numDeCelulas = (rtn.result) ? 0 : rtn.length;
            centrarMapaEnMarcadores();
            Ext.getCmp("cphMain_grdCelulas").setTitle("Celulas Cercanas: " + numDeCelulas);
        }
    });
}

function limpiarMapaDeCelulasProximas() {
    eliminarMarcadores();
    Ext.getCmp("cphMain_grdCelulas").setTitle("Celulas Cercanas: " + 0);
}
