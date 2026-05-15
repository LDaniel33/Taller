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