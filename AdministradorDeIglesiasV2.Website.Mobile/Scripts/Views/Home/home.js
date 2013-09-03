$(document).ready(function () {
    initCerarSesion();
});

function initCerarSesion() {
    $('#salir').tap(function (event) {
        if (confirm("¿Desea cerrar la sesi\xf3n actual?")) {
            mask();
            $.ajax({
                type: "POST",
                url: baseUrl + 'Home/CerrarSesion',
                cache: false,
                success: function (result) {
                    if (result.error) {
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