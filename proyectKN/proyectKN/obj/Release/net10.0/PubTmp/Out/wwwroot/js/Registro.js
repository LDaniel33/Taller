$(function () {

    $("#formRegistro").validate({
        rules: {
           NombreCompleto: {
                required: true
            },
          
            Correo: {
                required: true,
                email: true
            },
            Cedula: {
                required: true
            },

            UsuarioLogin: {
                required: true
            },

            Contrasenna: {
                required: true
            }
        },
            messages: {
                NombreCompleto: {
                    required: "Campo obligatorio",
                },

                Correo: {
                    required: "Campo obligatorio",
                    email: "Formato incorrecto"
                },

                Cedula: {
                    required: "Campo obligatorio",
                },
                UsuarioLogin: {
                    required: "Campo obligatorio",
                },
                Contrasenna: {
                    required: "Campo obligatorio",
                }
            },
            errorElement: "span",
            errorClass: "text-white",
            highlight: function (element) {
                $(element).addClass("is-invalid");
            },
            unhighlight: function (element) {
                $(element).removeClass("is-invalid");
            }
        });

});
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