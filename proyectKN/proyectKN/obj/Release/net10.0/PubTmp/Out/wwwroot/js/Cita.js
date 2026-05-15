$(function () {

    $("#formRegistro").validate({

        rules: {
            NombreCliente: { required: true },
            Cedula: { required: true },
            FechaCita: { required: true },
            HoraCita: { required: true },
            Telefono: { required: true },
            Email: { required: true },
            Servicio: { required: true },
            EstadoCita: { required: true },
            CreadaPor: { required: true }
        },

        messages: {
            NombreCliente: { required: "Campo obligatorio" },
            Cedula: { required: "Campo obligatorio" },
            FechaCita: { required: "Campo obligatorio" },
            HoraCita: { required: "Campo obligatorio" },
            Telefono: { required: "Campo obligatorio" },
            Email: { required: "Campo obligatorio" },
            Servicio: { required: "Campo obligatorio" },
            EstadoCita: { required: "Campo obligatorio" },
            CreadaPor: { required: "Campo obligatorio" }
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

function ConsultarCliente() {

    $("#NombreCliente").val("");
    var cedula = $("#Cedula").val().trim();

    if (cedula.length >= 9) {

        $.ajax({
            url: "https://apis.gometa.org/cedulas/" + cedula,
            type: "GET",

            success: function (data) {

                if (data.results.length > 0) {
                    $("#NombreCliente").val(data.results[0].fullname);
                }
                function eliminarFila(boton) {
                    // Pedimos confirmación
                    if (!confirm("¿Desea cancelar esta cita de la tabla?")) return;

                    // Encontrar la fila del botón
                    var fila = boton.closest("tr");

                    // Remover la fila de la tabla
                    fila.remove();
                }
            }

        });
    }
}
function seleccionarHora(hora, boton) {
    document.getElementById("HoraCita").value = hora;

    document.querySelectorAll(".hora-grid button").forEach(btn => {
        btn.classList.remove("activo");
    });

    boton.classList.add("activo");
}
function cargarHorasOcupadas() {
    const fecha = document.getElementById("FechaCita").value;
    const inputHora = document.getElementById("HoraCita");

    if (!fecha) return;

    const ahora = new Date();

    const anio = ahora.getFullYear();
    const mes = String(ahora.getMonth() + 1).padStart(2, "0");
    const dia = String(ahora.getDate()).padStart(2, "0");
    const hoy = `${anio}-${mes}-${dia}`;

    const horaActual = String(ahora.getHours()).padStart(2, "0") + ":" +
        String(ahora.getMinutes()).padStart(2, "0");

    fetch(`/Cita/ObtenerHorasOcupadas?fecha=${encodeURIComponent(fecha)}`)
        .then(response => response.json())
        .then(horasOcupadas => {
            document.querySelectorAll(".btn-hora").forEach(boton => {
                const hora = boton.dataset.hora;

                boton.disabled = false;
                boton.classList.remove("hora-ocupada", "hora-pasada", "activo");

                // bloquear ocupadas
                if (horasOcupadas.includes(hora)) {
                    boton.disabled = true;
                    boton.classList.add("hora-ocupada");
                }

                // bloquear horas pasadas si la fecha es hoy
                if (fecha === hoy && hora <= horaActual) {
                    boton.disabled = true;
                    boton.classList.add("hora-pasada");

                    if (inputHora.value === hora) {
                        inputHora.value = "";
                    }
                }
            });
        })
        .catch(error => {
            console.error("Error al cargar horas:", error);
        });
}
function confirmarCancelacion(id) {
    Swal.fire({
        title: "¿Desea cancelar esta cita?",
        text: "Esta acción no se puede deshacer",
        icon: "warning",
        background: "#171e27",
        color: "#ffffff",
        showCancelButton: true,
        confirmButtonColor: "#dc3545",
        cancelButtonColor: "#6c757d",
        confirmButtonText: "Sí, cancelar",
        cancelButtonText: "No"
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = "/Cita/CancelarCita/" + id;
        }
    });
}


