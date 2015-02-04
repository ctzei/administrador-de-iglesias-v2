//Libreria con funciones genericas para el manejo de mapas #REQUIERE EXTJS#

function obtenerCoordenadasDesdeDireccion(municipio, colonia, direccion, onFinish, onZeroResults, onError, ocultarvalidaciones) {
    if (!ocultarvalidaciones == true) {
        if (!(colonia.length > 0) || !(municipio.length > 0)) {
            Ext.Msg.alert('', 'Antes de continuar es necesario llenar los datos de colonia y municpio.');
            return;
        }
    }

    Ext.net.Mask.show({ el: Ext.getBody() });
    address = municipio + ", " + colonia + ", " + direccion;
    geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'address': address }, function (results, status) {
        Ext.net.Mask.hide();
        if (status == google.maps.GeocoderStatus.OK) {
            if (typeof (onFinish) == 'function') {
                onFinish(results);
            }
        }
        else if (status == google.maps.GeocoderStatus.ZERO_RESULTS) {
            if (typeof (onZeroResults) == 'function') {
                onZeroResults();
            }
            if (!ocultarvalidaciones == true) {
                Ext.Msg.alert('', 'No se encontraron coordenadas válidas para dicha dirección; es necesario cambiar de dirección o dejar la dirección actual, pero no sera posible rastrearla.');
            }
        } else {
            if (typeof (onError) == 'function') {
                onError(status);
            }
            if (!ocultarvalidaciones == true) {
                Ext.Msg.alert('', 'No se encontraron coordenadas válidas para dicha dirección; ocurrió un error desconocido: ' + status);
            }
        }
    });
}

function obtenerCoordenadasDesdeString(s) {
    c = s.split(',');
    if (c.length >= 2) {
        c[0] = c[0].replace('(', '').replace(')', '');
        c[1] = c[1].replace('(', '').replace(')', '');
        var latlng = new google.maps.LatLng(c[0], c[1]);
    }
    else {
        var latlng = new google.maps.LatLng(0, 0);
    }
    return latlng
}

function cargarMapa(latlng, el, zoom) {
    if (typeof (latlng) == 'string') { latlng = obtenerCoordenadasDesdeString(latlng); }
    if (window.gMapMapaPrimario) {
        eliminarMarcadores();
        window.gMapMapaPrimario.setCenter(latlng);
    }
    else {
        var mOptions = {
            zoom: zoom,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        }
        window.gMapMapaPrimario = new google.maps.Map(el, mOptions);
    }
}

function cargarMapaDesdeDireccion(idPanelContenedor, pais, estado, municipio, colonia, direccion) {
    var el = Ext.get(idPanelContenedor).dom;
    obtenerCoordenadasDesdeDireccion(pais + ', ' + estado + ', ' + municipio, colonia, direccion, function (results) {
        lat = results[0].geometry.location.lat();
        lng = results[0].geometry.location.lng();
        latlng = new google.maps.LatLng(lat, lng);
        cargarMapa(latlng, el, 14);
        agregarMarcador(latlng, "Direccion Base");
    }, null, null, true);
}


function cargarMapaDesdeDireccionEnPanel(idPanelContenedor, pais, estado, municipio, colonia, direccion) {
    cargarMapaDesdeDireccion(Ext.get(idPanelContenedor).query("div.x-panel-body")[0].id, pais, estado, municipio, colonia, direccion);
}


function agregarEventoAlMapa(evento, manejador) {
    if (typeof (manejador) != 'function') { alert("Es necesario que el manejador sea una funcion."); return; }
    google.maps.event.addListener(window.gMapMapaPrimario, evento, manejador);
}

function agregarMarcador(latlng, titulo, evento) {
    if (typeof (latlng) == 'string') {latlng = obtenerCoordenadasDesdeString(latlng); }
    if (!window.gMapMarcadoresPrimarios) { window.gMapMarcadoresPrimarios = []; }
    if (!window.gMapLocacionesPrimarias) { window.gMapLocacionesPrimarias = []; }
    window.gMapLocacionesPrimarias.push(latlng);

    marker = new google.maps.Marker({
        position: latlng,
        map: window.gMapMapaPrimario,
        title: titulo
    });
    window.gMapMarcadoresPrimarios.push(marker);

    if ((evento) && (typeof (evento) == 'function')) {
        google.maps.event.addListener(marker, 'click', evento);
    }
}

function eliminarMarcadores() {
    if (window.gMapMarcadoresPrimarios) {
        for (i = 0; i < window.gMapMarcadoresPrimarios.length; i++) {
            window.gMapMarcadoresPrimarios[i].setMap(null);
        }
        window.gMapMarcadoresPrimarios.length = 0;
        window.gMapLocacionesPrimarias.length = 0;
    }
}

function centrarMapaEnMarcadores() {
    var bounds = new google.maps.LatLngBounds();
    for (i = 0; i < window.gMapLocacionesPrimarias.length; i++) {
        bounds.extend(window.gMapLocacionesPrimarias[i]);
    }
    window.gMapMapaPrimario.fitBounds(bounds);
}