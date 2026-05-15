document.addEventListener("DOMContentLoaded", function () {

    setTimeout(function () {

        var success = document.getElementById("mensajeSuccess");
        if (success) {
            success.style.transition = "opacity 0.5s";
            success.style.opacity = "0";
            setTimeout(function () {
                success.remove();
            }, 500);
        }

        var error = document.getElementById("mensajeError");
        if (error) {
            error.style.transition = "opacity 0.5s";
            error.style.opacity = "0";
            setTimeout(function () {
                error.remove();
            }, 500);
        }

    }, 3000);

});