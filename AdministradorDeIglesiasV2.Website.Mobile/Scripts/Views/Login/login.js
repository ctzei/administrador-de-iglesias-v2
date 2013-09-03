$(document).ready(function () {
    initIniciarSesion();
});

function initIniciarSesion() {
    $('#iniciarSesion').tap(function (event) {
        mask();

        if (getParameterByName('debug') != 'true') {
            var params = {
                'email': $('#email').val(),
                'password': $('#password').val(),
                'recordarme': $('#recordarme').prop('checked')
            }

            $.ajax({
                type: "POST",
                url: baseUrl + 'Login/IniciarSesion',
                cache: false,
                data: params,
                success: function (result) {
                    if (result.error) {
                        unmask();
                        //$('#password').val('');
                        alert(result.error);
                    }
                    else if (result.url) {
                        window.location.href = result.url;
                    }
                },
                error: function (error) {
                    console.log(error.responseText);
                }
            });
        }

        return false;
    });
}