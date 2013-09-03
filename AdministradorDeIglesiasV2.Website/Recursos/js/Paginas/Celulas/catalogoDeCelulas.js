var clickAgregado = false;

function obtenerVentanaDeMapa() {
    return Ext.getCmp('cphMain_cphEdicion_wndMapa');
}

function obtenerRegistroDeCoordenadas(){
    return Ext.getCmp("cphMain_cphEdicion_registroCoordenadas");  
}

function buscarCordenadasClick() {
    if (obtenerRegistroDeCoordenadas().getValue().trim().length <= 0) {
        municipio = Ext.getCmp("cphMain_cphEdicion_registroMunicipio").getText();
        colonia = Ext.getCmp("cphMain_cphEdicion_registroColonia").getValue();
        direccion = Ext.getCmp("cphMain_cphEdicion_registroDireccion").getValue();
        obtenerCoordenadasDesdeDireccion(municipio, colonia, direccion, generarMapaDeCelula, generarCoordenadasBasicas, generarCoordenadasBasicas);
    }
    else {
        cargarMapaDeCelula(obtenerRegistroDeCoordenadas().getValue().trim());
    }
}

function generarMapaDeCelula(results) {
    lat = results[0].geometry.location.lat();
    lng = results[0].geometry.location.lng();
    latlng = new google.maps.LatLng(lat, lng);
    cargarMapaDeCelula(latlng);
    obtenerRegistroDeCoordenadas().setValue(results[0].geometry.location);
}

function sobreescribirCoordenadas(event) {
    eliminarMarcadores();
    agregarMarcador(event.latLng, "Direccion Base");
    obtenerRegistroDeCoordenadas().setValue(event.latLng);
}

function cargarMapaDeCelula(latlng) {
    el = (Ext.get("cphMain_cphEdicion_wndMapa_Content")) ? Ext.get("cphMain_cphEdicion_wndMapa_Content").parent().dom : null;
    cargarMapa(latlng, el, 14);
    agregarMarcador(latlng, "Direccion Base");
    obtenerVentanaDeMapa().show();

    //Agregamos el manejador de "click" solo una vez
    if (!clickAgregado) {
        agregarEventoAlMapa('click', sobreescribirCoordenadas);
        clickAgregado = true;
    }
}

function generarCoordenadasBasicas() {
    obtenerRegistroDeCoordenadas().setValue('(0,0)');
}

function limpiarCoordenadas() {
    Ext.getCmp("cphMain_cphEdicion_registroCoordenadas").setValue('');
}