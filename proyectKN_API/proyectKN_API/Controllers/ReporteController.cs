using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using proyectKN_API.Models;
using System.Data;

namespace proyectKN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ReporteController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("GenerarReporte")]
        public IActionResult GenerarReporte(string tipo, string? desde, string? hasta, string? estado = null)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            if (string.IsNullOrEmpty(tipo))
                return BadRequest("Tipo requerido");

            DateTime? fechaDesde = null;
            DateTime? fechaHasta = null;

            if (!string.IsNullOrWhiteSpace(desde))
            {
                if (!DateTime.TryParse(desde, out DateTime fd))
                    return BadRequest("Fecha desde inválida");
                fechaDesde = fd;
            }

            if (!string.IsNullOrWhiteSpace(hasta))
            {
                if (!DateTime.TryParse(hasta, out DateTime fh))
                    return BadRequest("Fecha hasta inválida");
                fechaHasta = fh;
            }

            switch (tipo.ToLower())
            {

                case "citas":
                    var citas = conn.Query<ReporteCitasResponse>(
                        "sp_ReporteCitas",
                        new { FechaDesde = fechaDesde, FechaHasta = fechaHasta, Estado = estado },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return Ok(citas);

                case "clientes":
                    var usuarios = conn.Query<ReporteUsuarioResponse>(
                        "sp_ReporteUsuarios",
                        new { FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return Ok(usuarios);


                case "ingresos_vehiculos":
                    var ingresosVehiculos = conn.Query<ReporteVehiculosResponse>(
                        "sp_ReporteIngresoVehiculos",
                        new { FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return Ok(ingresosVehiculos);

                case "contabilidad":
                    var ingresos = conn.Query<ReporteIngresosResponse>(
                        "sp_ReporteIngresos",
                        new { FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    var egresos = conn.Query<ReporteEgresosResponse>(
                        "sp_ReporteEgresos",
                        new { FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                        commandType: CommandType.StoredProcedure
                    ).ToList();

                    var contabilidad = ingresos.Select(i => new ReporteResponse
                    {
                        Fecha = i.Fecha,
                        Tipo = "Ingreso",
                        Descripcion = i.Descripcion,
                        Monto = i.Monto,
                        ClienteVehiculo = ""
                    })
                    .Concat(egresos.Select(e => new ReporteResponse
                    {
                        Fecha = e.Fecha,
                        Tipo = "Egreso",
                        Descripcion = e.Motivo,
                        Monto = e.Monto,
                        ClienteVehiculo = ""
                    }))
                    .OrderByDescending(x => x.Fecha)
                    .ToList();

                    return Ok(contabilidad);

                case "inventario":
                    var inventario = conn.Query<ReporteInventarioResponse>(
                        "sp_ReporteInventario",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return Ok(inventario);

                case "proveedores":
                    var proveedores = conn.Query<ReporteProveedorResponse>(
                        "sp_ReporteProveedores",
                        commandType: CommandType.StoredProcedure
                    ).ToList();
                    return Ok(proveedores);

                default:
                    return BadRequest("Tipo de reporte inválido");
            }
        }
        [HttpGet("ResumenCitas")]
        public IActionResult ResumenCitas(string? desde, string? hasta)
        {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            DateTime? fechaDesde = null;
            DateTime? fechaHasta = null;

            if (!string.IsNullOrWhiteSpace(desde))
            {
                if (!DateTime.TryParse(desde, out DateTime fd))
                    return BadRequest("Fecha desde inválida");
                fechaDesde = fd;
            }

            if (!string.IsNullOrWhiteSpace(hasta))
            {
                if (!DateTime.TryParse(hasta, out DateTime fh))
                    return BadRequest("Fecha hasta inválida");
                fechaHasta = fh;
            }

            var resumen = conn.QueryFirstOrDefault<ResumenCitasResponse>(
                "sp_ResumenCitas",
                new { FechaDesde = fechaDesde, FechaHasta = fechaHasta },
                commandType: CommandType.StoredProcedure
            );

            return Ok(resumen);
        }
    }

    }
