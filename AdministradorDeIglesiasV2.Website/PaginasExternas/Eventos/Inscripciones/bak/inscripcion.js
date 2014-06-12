WebServicesUrls = {
    Formulario: 'http://www.frocu.com/puertacielo/WebServices/Eventos/GuardarDeInscripcion.aspx',
    Municipios: 'http://www.frocu.com/puertacielo/WebServices/Generales/Municipios.aspx'
};

$(document).ready(function () {
    initFormulario();
    initComboDeEstados();
    initTipoDeInscripcion();
    initTipoDeRegistrante();
    initTipoDeDeposito();
    initLimpiar();
});

function initFormulario() {
    $('#fecha').simpleDatepicker();

    //Establecemos el validador del formulario...
    validator = $("#inscripcion").validate({

        //Cuando el formulario es valido se envia al servidor usando JSONP
        submitHandler: function (form) {
            mostrarMascara();
            $.ajax({
                type: "GET",
                crossDomain: true,
                url: WebServicesUrls.Formulario,
                data: $('#inscripcion').serialize(),
                contentType: "application/json; charset=utf-8",
                dataType: "jsonp",
                success: function (data) {
                    if (!data.success) {
                        alert(data.error);

                        if (data.invalidFields) {
                            errores = {};
                            for (var i = 0; i < data.invalidFields.length; i++) {
                                errores[data.invalidFields[i]] = "Invalido";
                            }

                            validator.showErrors(errores);
                        }
                    }
                    else {
                        mostrarConfirmacion(data.folio);
                    }

                    $.loading(false);
                },
                error: function (data) {
                    mostrarErrorDeServidor();
                }
            });
        }
    });


}

function initComboDeEstados() {
    $("#estado").change(function () {

        mostrarMascara();
        $.ajax({
            type: "GET",
            crossDomain: true,
            url: WebServicesUrls.Municipios,
            data: { estadoId: $(this).val() },
            contentType: "application/json; charset=utf-8",
            dataType: "jsonp",
            success: function (data) {
                municipios = data.municipios;
                if (municipios) {
                    var options = '<option value="" selected="selected">--Seleccione--</option>';
                    for (var i = 0; i < municipios.length; i++) {
                        options += '<option value="' + municipios[i].Id + '">' + municipios[i].Municipio + '</option>';
                    }
                    $("#municipio").html(options);
                }
                else {
                    mostrarErrorDeServidor();
                }

                $.loading(false);
            },
            error: function (data) {
                mostrarErrorDeServidor();
            }
        });


    })
}

function initTipoDeInscripcion() {
    $('#inscripcionGrupal').change(function (e) {
        $('#cant').parent().show();
    });

    $('#inscripcionIndividual').change(function (e) {
        $('#cant').parent().hide();
        $('#cant').val(1);
    });
}

function initTipoDeRegistrante() {
    $('#tregistrante').change(function (e) {
        //Si se selecciono la opcion de "Otra Iglesia..."
        if (parseInt($('#tregistrante').val()) == 2) {
            $('#iregistrante').val('');
            $('#iregistrante').parent().show();
        }
        else {
            $('#iregistrante').parent().hide();
        }
    });
}

function initTipoDeDeposito() {
    var ralfa = $('#ralfa');
    var rnum = $('#rnum');
    var ralfalbl = ralfa.parent().find("label.lbl");

    $('#depositoBancario').change(function (e) {
        ralfa.parent().hide();
        ralfa.val('NA');
        rnum.parent().show();
        rnum.val('');
    });

    $('#transferenciaElectronica').change(function (e) {
        rnum.parent().hide();
        rnum.val(0);
        ralfalbl.text("Folio de Internet:*");
        ralfa.parent().show();
        ralfa.val('');
    });

    $('#efectivo').change(function (e) {
        rnum.parent().hide();
        rnum.val(0);
        ralfalbl.text("Quién recibió?:*");
        ralfa.parent().show();
        ralfa.val('');
    });
}

function initLimpiar() {
    $('#limpiar').click(function (e) {
        $('#depositoBancario').change();
    });
}

function mostrarMascara() {
    $.loading(true, {
        text: 'Cargando...',
        mask: true,
        delay: 200,
        align: 'bottom-center'
    });
}

function mostrarErrorDeServidor() {
    alert('El servidor no respondio el formato correcto. Favor de volverlo a intentar o contactar al administrador del sistema.');
    $.loading(false);  
}

function mostrarConfirmacion(folio) {
    $('#folio').text(folio);
    $('#correo').attr("href", $('#correo').attr("href").replace("@@folio@@", folio));
    $('#registro').hide();
    $('#confirmacion').show();
}