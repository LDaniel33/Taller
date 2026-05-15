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

document.getElementById("Anio").addEventListener("input", function () {
    this.value = this.value.replace(/\D/g, '');

    if (this.value.length > 4) {
        this.value = this.value.slice(0, 4);
    }
});
document.getElementById("Telefono").addEventListener("input", function () {
    this.value = this.value.replace(/\D/g, '');

    if (this.value.length > 8) {
        this.value = this.value.slice(0, 8);
    }
});
