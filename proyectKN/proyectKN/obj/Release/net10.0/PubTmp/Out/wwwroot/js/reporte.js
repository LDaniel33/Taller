document.addEventListener("DOMContentLoaded", function () {
    const inputDesde = document.getElementById("desde");
    const inputHasta = document.getElementById("hasta");

    if (inputDesde) inputDesde.value = "";
    if (inputHasta) inputHasta.value = "";
    let ultimoTitulo = "Reporte";

    const configuraciones = {
        contabilidad: {
            titulo: "Reporte de Contabilidad",
            columnas: ["Fecha", "Tipo", "Descripción", "Monto"],
            campos: ["fecha", "tipo", "descripcion", "monto"]
        },
        clientes: {
            titulo: "Reporte de Usuarios",
            columnas: ["Nombre", "Cédula", "Correo", "Usuario", "Rol", "Estado", "Fecha Registro"],
            campos: ["nombreCompleto", "cedula", "correo", "usuarioLogin", "nombreRol", "estado", "fechaRegistro"]
        },
        citas: {
            titulo: "Reporte de Citas",
            columnas: ["Fecha", "Hora", "Cliente", "Cédula", "Servicio", "Estado", "Creada por"],
            campos: ["fechaCita", "horaCita", "nombreCliente", "cedula", "servicio", "estado", "creadaPor"]
        },
      
        ingresos_vehiculos: {
            titulo: "Reporte de Ingreso de Vehículos",
            columnas: ["Cliente", "Cédula", "Placa", "Marca", "Modelo", "Problema", "Revisión", "Estado"],
            campos: ["nombre_Cliente", "cedula", "placa", "marca", "modelo", "problema", "revision", "estado"]
        },
        inventario: {
            titulo: "Reporte de Inventario",
            columnas: ["Nombre", "Id Artículo", "Descripción", "Precio Compra", "Precio Venta", "Stock", "Stock Mínimo", "Proveedor"],
            campos: ["nombre", "idArticulo", "descripcion", "precioCompra", "precioVenta", "stock", "stockMinimo", "proveedor"]
        },
        proveedores: {
            titulo: "Reporte de Proveedores",
            columnas: ["Nombre", "Teléfono", "Correo", "Dirección"],
            campos: ["nombre", "telefono", "correo", "direccion"]
        }
    };

    window.preset = async function (tipo, estado = "") {
        console.log("Tipo recibido:", tipo);

        const desde = document.getElementById("desde")?.value || "";
        const hasta = document.getElementById("hasta")?.value || "";

        const config = configuraciones[tipo];

        if (!config) {
            alert("No existe configuración para este reporte: " + tipo);
            return;
        }

        document.getElementById("reportTitle").textContent = config.titulo;
        ultimoTitulo = config.titulo;

        try {
            let url = `/Reporteria/ObtenerReporte?tipo=${encodeURIComponent(tipo)}`;

            if (desde) {
                url += `&desde=${encodeURIComponent(desde)}`;
            }

            if (hasta) {
                url += `&hasta=${encodeURIComponent(hasta)}`;
            }

            if (tipo === "citas" && estado !== "") {
                url += `&estado=${encodeURIComponent(estado)}`;
            }

            console.log("URL enviada:", url);

            const response = await fetch(url);

            if (!response.ok) {
                const errorText = await response.text();
                console.error("Error del servidor:", errorText);
                alert("Error al obtener el reporte");
                return;
            }

            const data = await response.json();
            if (tipo === "contabilidad") {
                let totalIngresos = 0;

                data.forEach(item => {
                    if (item.tipo && item.tipo.toLowerCase().includes("ingreso")) {
                        totalIngresos += Number(item.monto || 0);
                    }
                });

                console.log("Total ingresos:", totalIngresos);

                document.getElementById("totalIngresos").textContent =
                    `Ingresos: ₡${totalIngresos.toLocaleString("es-CR")}`;
            }
            generarTabla(tipo, data);
            //actualizarResumen(tipo, data);
        } catch (error) {
            console.error(error);
            alert("Error al obtener los datos del reporte");
        }
    };
    function generarTabla(tipo, data) {
        const config = configuraciones[tipo];
        const thead = document.getElementById("reportHead");
        const tbody = document.getElementById("reportBody");

        thead.innerHTML = "";
        tbody.innerHTML = "";

        const trHead = document.createElement("tr");

        config.columnas.forEach(columna => {
            const th = document.createElement("th");
            th.textContent = columna;
            trHead.appendChild(th);
        });

        thead.appendChild(trHead);

        if (!data || data.length === 0) {
            const tr = document.createElement("tr");
            const td = document.createElement("td");
            td.colSpan = config.columnas.length;
            td.textContent = "No hay datos para este reporte";
            tr.appendChild(td);
            tbody.appendChild(tr);
            return;
        }

        data.forEach(item => {
            const tr = document.createElement("tr");

            config.campos.forEach(campo => {
                const td = document.createElement("td");
                td.textContent = formatearValor(item[campo], campo);
                tr.appendChild(td);
            });

            tbody.appendChild(tr);
        });
    }

    function formatearValor(valor, campo) {
        if (valor === null || valor === undefined) return "";

        const campoLower = campo.toLowerCase();

        if (campoLower.includes("fecha")) {
            const fecha = new Date(valor);
            if (!isNaN(fecha)) {
                return fecha.toLocaleDateString("es-CR");
            }
        }

        if (campoLower.includes("hora")) {
            return valor;
        }

        if (campoLower === "monto" || campoLower === "preciocompra" || campoLower === "precioventa") {
            const numero = Number(valor);
            if (!isNaN(numero)) {
                return "₡" + numero.toLocaleString("es-CR", {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });
            }
        }

        return valor;
    }

    function actualizarResumen(tipo, data) {
        const totalIngresos = document.getElementById("totalIngresos");

        if (!totalIngresos) return;

        if (!data || data.length === 0) {
            totalIngresos.textContent = "Ingresos: ₡0";
            return;
        }

        if (tipo === "contabilidad") {
            let total = 0;

            data.forEach(item => {
                if (item.tipo === "Ingreso") {
                    total += Number(item.monto || 0);
                }
            });

            totalIngresos.textContent = "Ingresos: ₡" + total.toLocaleString("es-CR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
            return;
        }

        totalIngresos.textContent = "Ingresos: ₡0";
    }
    async function cargarResumenIngresos() {
        const desde = document.getElementById("desde")?.value || "";
        const hasta = document.getElementById("hasta")?.value || "";

        try {
            let url = `/Reporteria/ObtenerReporte?tipo=contabilidad`;

            if (desde) {
                url += `&desde=${encodeURIComponent(desde)}`;
            }

            if (hasta) {
                url += `&hasta=${encodeURIComponent(hasta)}`;
            }

            const response = await fetch(url);

            if (!response.ok) {
                const errorText = await response.text();
                console.error("Error al cargar ingresos:", errorText);
                return;
            }

            const data = await response.json();

            let totalIngresos = 0;

            data.forEach(item => {
                if (item.tipo && item.tipo.toLowerCase().includes("ingreso")) {
                    totalIngresos += Number(item.monto || 0);
                }
            });

            const totalIngresosLabel = document.getElementById("totalIngresos");
            if (totalIngresosLabel) {
                totalIngresosLabel.textContent = "Ingresos: ₡" + totalIngresos.toLocaleString("es-CR", {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                });
            }
        } catch (error) {
            console.error("Error en resumen de ingresos:", error);
        }
    }
    async function cargarResumenCitas() {
        const desde = document.getElementById("desde")?.value || "";
        const hasta = document.getElementById("hasta")?.value || "";

        try {
            let url = `/Reporteria/ObtenerResumenCitas`;

            if (desde || hasta) {
                url += "?";

                if (desde) {
                    url += `desde=${encodeURIComponent(desde)}`;
                }

                if (hasta) {
                    if (desde) url += "&";
                    url += `hasta=${encodeURIComponent(hasta)}`;
                }
            }

            console.log("URL resumen:", url);

            const response = await fetch(url);

            if (!response.ok) {
                const errorText = await response.text();
                console.error("Error al cargar resumen de citas:", errorText);
                return;
            }

            const data = await response.json();

            const confirmadas = document.getElementById("totalConfirmadas");
            const finalizadas = document.getElementById("totalFinalizadas");
            const pendientes = document.getElementById("totalPendientes");
            const canceladas = document.getElementById("totalCanceladas");

            if (confirmadas) confirmadas.textContent = `Confirmadas: ${data.confirmadas ?? 0}`;
            if (finalizadas) finalizadas.textContent = `Finalizadas: ${data.finalizadas ?? 0}`;
            if (pendientes) pendientes.textContent = `Pendientes: ${data.pendientes ?? 0}`;
            if (canceladas) canceladas.textContent = `Canceladas: ${data.canceladas ?? 0}`;
        } catch (error) {
            console.error("Error en resumen de citas:", error);
        }
    }
    document.getElementById("desde")?.addEventListener("change", cargarResumenCitas);
    document.getElementById("hasta")?.addEventListener("change", cargarResumenCitas);
    document.getElementById("desde")?.addEventListener("change", cargarResumenIngresos);
    document.getElementById("hasta")?.addEventListener("change", cargarResumenIngresos);
    cargarResumenCitas();
    cargarResumenIngresos();

    window.exportTable = function () {
        const table = document.getElementById("reportTable");
        let csv = [];

        for (let i = 0; i < table.rows.length; i++) {
            let row = [];
            for (let j = 0; j < table.rows[i].cells.length; j++) {
                row.push('"' + table.rows[i].cells[j].innerText.replace(/"/g, '""') + '"');
            }
            csv.push(row.join(","));
        }

        const csvContent = "data:text/csv;charset=utf-8,\uFEFF" + csv.join("\n");
        const encodedUri = encodeURI(csvContent);
        const link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute("download", `${ultimoTitulo}.csv`);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };

    window.exportPDF = function () {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();

        doc.text(ultimoTitulo, 14, 15);

        const head = [];
        document.querySelectorAll("#reportHead th").forEach(th => {
            head.push(th.innerText);
        });

        const body = [];
        document.querySelectorAll("#reportBody tr").forEach(tr => {
            const row = [];
            tr.querySelectorAll("td").forEach(td => {
                row.push(td.innerText);
            });
            body.push(row);
        });

        doc.autoTable({
            head: [head],
            body: body,
            startY: 25,
            styles: { fontSize: 9 }
        });

        doc.save(`${ultimoTitulo}.pdf`);
    };
    window.filtrarResumenCitas = function (estado) {
        const estadoCita = document.getElementById("estadoCita");

        if (estadoCita) {
            estadoCita.value = estado;
        }

        preset("citas", estado);
    };
});