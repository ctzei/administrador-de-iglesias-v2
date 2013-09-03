$(document).ready(function () {
    initCombos();
    initAsistencia();
});

function initCombos() {

    var oCelula = $('#celulaId'),
        oDia = $('#dia'),
        oMes = $('#mes'),
        oAnio = $('#anio');

    var establecerDiasAbreviados = function () {
        var diaOpts = oDia.get(0).options;
        var mes = oMes.val() - 1;
        var anio = oAnio.val();
        var pad = "00";
        for (var i = 0; i < diaOpts.length; i++) {
            var diaOpt = diaOpts[i];
            var d = new Date(anio, mes, diaOpt.value);

            var str = "" + diaOpt.value
            diaOpt.text = pad.substring(0, pad.length - str.length) + str + " - " + d.getDayName('es');
        }
    };

    var mostrarAsistencia = function (ignorarFecha) {
        mask();

        var params = {
            "celulaId": oCelula.val()
        };

        if (!ignorarFecha) {
            params.dia = oDia.val();
            params.mes = oMes.val();
            params.anio = oAnio.val();
        }

        var action;
        if (ignorarFecha){
            action = "MostrarFaltante";
        }
        else {
             action = "Mostrar";
        }

        $.ajax({
            type: "POST",
            url: baseUrl + "RegistroDeAsistencia/" + action,
            cache: false,
            dataType: "json",
            data: params,
            success: function (data) {
                unmask();
                if (data.html) {
                    $("#asistencias").html(data.html);
                    oDia.val(data.dia);
                    oMes.val(data.mes);
                    oAnio.val(data.anio);
                    initAsistencia();

                    // Si vamos a traer la asistencia faltante necesitamos "recargar" los dias abreviados en el combo de dias
                    if (ignorarFecha) {
                        establecerDiasAbreviados();
                    }

                    // Si recibimos algun mensaje lo mostrarmos
                    if (data.msg) {
                        alert(data.msg);
                    }
                }
                else if (data.error) {
                    alert(data.error);
                }
            },
            error: function (error) {
                console.log(error.responseText);
            }
        });

        return false;
    };

    var mostrarAsistenciaEspecifica = function(){
        mostrarAsistencia(false);
    };

    var mostrarAsistenciaFaltante = function(){
        mostrarAsistencia(true);
    };

    oCelula.change(mostrarAsistenciaFaltante);
    oDia.change(mostrarAsistenciaEspecifica);
    oMes.change(mostrarAsistenciaEspecifica);
    oAnio.change(mostrarAsistenciaEspecifica);

    oMes.change(establecerDiasAbreviados);
    oAnio.change(establecerDiasAbreviados);
}

function initAsistencia() {

    var initGridDeAsistencia = function () {
        $('#gridAsistencia tr').tap(function (event) {
                if (event.target.tagName.toLowerCase() != 'input') {
                var chk = $(this).find("input[type=checkbox]");
                chk.prop('checked', !chk.prop('checked'));
            }
        });
    };

    var initBotons = function () {
        //Guardar Asistencia
        $('#guardarAsistencia').tap(function () {
            mask();
            $.ajax({
                type: "POST",
                url: baseUrl + "RegistroDeAsistencia/Guardar",
                cache: false,
                dataType: "json",
                contentType: 'application/json, charset=utf-8',
                data: JSON.stringify(obtenerJsonDesdeGrid()),
                success: function (result) {
                    unmask();
                    if (result.error) {
                        alert(result.error);
                    }
                    else if (result.msg) {
                        alert(result.msg);
                    }
                },
                error: function (error) {
                    console.log(error.responseText);
                }
            });

            return false;
        });

        //Cancelar Asistencia
        $('#cancelarAsistencia').tap(function () {
            if (confirm("¿Deseas cancelar la celula de ese dia?")) {
                var razon = prompt('Razon de la cancelacion:', '');
                if (razon) {
                    if (razon.length > 20) {
                        mask();
                        $.ajax({
                            type: "POST",
                            url: baseUrl + "RegistroDeAsistencia/Cancelar",
                            cache: false,
                            dataType: "json",
                            contentType: 'application/json, charset=utf-8',
                            data: JSON.stringify({
                                'celulaId': $('#celulaId').val(),
                                'dia': $('#dia').val(),
                                'mes': $('#mes').val(),
                                'anio': $('#anio').val(),
                                'razon': razon
                            }),
                            success: function (result) {
                                unmask();
                                if (result.error) {
                                    alert(result.error);
                                }
                                else if (result.msg) {
                                    alert(result.msg);
                                }
                            },
                            error: function (error) {
                                console.log(error.responseText);
                            }
                        });
                    }
                    else {
                        alert("No se cumplio la longitud minima en la razon de la cancelacion.");
                    }
                }
            };

            return false;
        });
    };

    initBotons();
    initGridDeAsistencia();
}

function obtenerJsonDesdeGrid() {
    var celulaId = $('#celulaId').val(),
        dia = $('#dia').val(),
        mes = $('#mes').val(),
        anio = $('#anio').val(),
        numeroDeInvitados = $('#numeroDeInvitados').val(),
        filas = $('#gridAsistencia tr'),
        registroDeAsistencia = {
            'celulaId': celulaId,
            'dia': dia,
            'mes': mes,
            'anio': anio,
            'numeroDeInvitados': numeroDeInvitados,
            'asistencias': []
        };

    for (var i = 0; i < filas.length; i++) {
        var fila = filas[i],
                id = $(fila).attr("data-asistencia-id"),
                miembroId = $(fila).attr("data-miembro-id"),
                asistencia = $(fila).find("input[type=checkbox]").prop("checked");

        registroDeAsistencia.asistencias.push({
            'Id': id,
            'MiembroId': miembroId,
            'Asistencia': asistencia
        })
    }

    return registroDeAsistencia;
}
