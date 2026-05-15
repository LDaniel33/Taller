document.addEventListener("DOMContentLoaded", function () {
    marcarHoraActual();
    cargarHorasOcupadasEditar(false); 
});

function seleccionarHora(hora, boton) {
    document.getElementById("HoraCita").value = hora;

    document.querySelectorAll(".btn-hora").forEach(btn => {
        btn.classList.remove("activo");
    });

    boton.classList.add("activo");
}

function marcarHoraActual() {
    const inputHora = document.getElementById("HoraCita");
    if (!inputHora) return;

    const horaActual = (inputHora.value || "").substring(0, 5);

    document.querySelectorAll(".btn-hora").forEach(btn => {
        btn.classList.remove("activo");

        const horaBoton = (btn.dataset.hora || "").substring(0, 5);

        if (horaBoton === horaActual) {
            btn.classList.add("activo");
        }
    });
}

function cargarHorasOcupadasEditar(limpiarSeleccion = true) {
    const fechaInput = document.getElementById("FechaCita");
    const idCitaInput = document.getElementById("Consecutivo");
    const horaInput = document.getElementById("HoraCita");

    if (!fechaInput || !horaInput) return;

    const fecha = fechaInput.value;
    const idCita = idCitaInput ? idCitaInput.value : "";

    if (!fecha) return;

    
    const ahora = new Date();

    const anio = ahora.getFullYear();
    const mes = String(ahora.getMonth() + 1).padStart(2, "0");
    const dia = String(ahora.getDate()).padStart(2, "0");
    const hoy = `${anio}-${mes}-${dia}`;

    const horaActual = String(ahora.getHours()).padStart(2, "0") + ":" +
        String(ahora.getMinutes()).padStart(2, "0");

    
    if (limpiarSeleccion) {
        horaInput.value = "";

        document.querySelectorAll(".btn-hora").forEach(btn => {
            btn.classList.remove("activo");
        });
    }

    fetch('/Cita/ObtenerHorasOcupadas?fecha=' + encodeURIComponent(fecha) + '&idCita=' + encodeURIComponent(idCita))
        .then(response => response.json())
        .then(horasOcupadas => {
            const horasNormalizadas = horasOcupadas.map(h => (h || "").substring(0, 5));

            document.querySelectorAll(".btn-hora").forEach(btn => {
                const horaBoton = (btn.dataset.hora || "").substring(0, 5);

                btn.style.display = "inline-block";
                btn.disabled = false;
                btn.classList.remove("hora-ocupada", "hora-pasada");

               
                if (horasNormalizadas.includes(horaBoton)) {
                    btn.disabled = true;
                    btn.classList.add("hora-ocupada");
                }

                
                if (fecha === hoy && horaBoton <= horaActual) {
                    btn.disabled = true;
                    btn.classList.add("hora-pasada");

                   
                    if (horaInput.value === horaBoton) {
                        horaInput.value = "";
                        btn.classList.remove("activo");
                    }
                }
            });

           
            if (!limpiarSeleccion) {
                marcarHoraActual();
            }
        })
        .catch(error => {
            console.error("Error al cargar horas:", error);
        });
}
document.getElementById("Telefono").addEventListener("input", function () {
    this.value = this.value.replace(/\D/g, '');

    if (this.value.length > 8) {
        this.value = this.value.slice(0, 8);
    }
});
