
document.getElementById("Telefono").addEventListener("input", function () {
    this.value = this.value.replace(/\D/g, '');

    if (this.value.length > 8) {
        this.value = this.value.slice(0, 8);
    }
});

    document.querySelector("#formRegistro").addEventListener("submit", function (e) {

        let valido = true;

    const campos = this.querySelectorAll("[required]");

    campos.forEach(campo => {
        if (!campo.value.trim()) {
        campo.classList.add("is-invalid");
    valido = false;
        } else {
        campo.classList.remove("is-invalid");
        }
    });

    if (!valido) {
        e.preventDefault();
    document.getElementById("errorGeneral").style.display = "block";
    }
});


document.querySelectorAll("[required]").forEach(campo => {
        campo.addEventListener("input", function () {
            if (this.value.trim()) {
                this.classList.remove("is-invalid");
            }
        });
});
