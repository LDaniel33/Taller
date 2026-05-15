
//AJAX = estrructura estandar, para que se haan las cosas en timempo real
function ConsultarNombre() {

    $("#NombreCompleto").val("");
    var cedula = $("#Cedula").val().trim();

    if (cedula.length >= 9) {
        $.ajax({
            url: "https://apis.gometa.org/cedulas/" + cedula,
            type: "GET",
            success: function (data) {
                if (data.results.length > 0) {
                    $("#NombreCompleto").val(data.results[0].fullname);
                }
            }

        });
    }
}

        function ConsultarDueno() {

            $("#Nombre_Cliente").val("");
            var cedula = $("#Cedula").val().trim();

            if (cedula.length >= 9) {
                $.ajax({
                    url: "https://apis.gometa.org/cedulas/" + cedula,
                    type: "GET",
                    success: function (data) {
                        if (data.results.length > 0) {
                            $("#Nombre_Cliente").val(data.results[0].fullname);
                        }
                    }

                });
    }

}

document.getElementById("Contrasenna").addEventListener("input", function () {

    const valor = this.value;

    if (valor.length < 6) {
        this.setCustomValidity("Debe tener al menos 6 caracteres");
    } else if (!/\d/.test(valor)) {
        this.setCustomValidity("Debe incluir al menos un número");
    } else {
        this.setCustomValidity("");
    }

});