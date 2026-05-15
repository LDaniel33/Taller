using Dapper;
using proyectKN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
namespace proyectKN_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContabilidadController : ControllerBase
{
    private readonly IConfiguration _config;
    public ContabilidadController(IConfiguration config)
    {
        _config = config;
    }
    #region Ingresos
    [HttpPost("RegistrarIngreso")]
    public IActionResult RegistrarIngreso(IngresoRequest model)
    {
        using var context = new SqlConnection(
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

        if (model.Monto > model.Saldo_Pendiente)
            return BadRequest("El monto no puede ser mayor que la deuda.");

        if (model.Monto == model.Saldo_Pendiente)
            model.Estado = 11;
        else
            model.Estado = 10; 

        var parametros = new DynamicParameters();
        parametros.Add("@Descripcion", model.Descripcion);
        parametros.Add("@Monto", model.Monto);
        parametros.Add("@Saldo_Pendiente", model.Saldo_Pendiente);
        parametros.Add("@Estado", model.Estado);

        var result = context.Execute(
            "sp_RegistroIngreso",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (result <= 0)
            return BadRequest("El ingreso no se registró correctamente");

        return Ok("Ingreso registrado correctamente");
    }

    [HttpPost("ConsultarIngreso")]
    public IActionResult ConsultarIngreso()
    {
        using var context = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var lista = context.Query<IngresoResponse>("sp_ConsultarIngreso",
            commandType: System.Data.CommandType.StoredProcedure).ToList();

        if (!lista.Any())
            return NotFound("No hay Ingresos registrados");

        return Ok(lista);
    }

    [HttpPut("EditarIngreso")]
    public IActionResult EditarIngreso([FromBody] IngresoRequest model)
    {
        if (model.Consecutivo <= 0)
            return BadRequest("El ID no es válido");

        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        if (model.Monto > model.Saldo_Pendiente)
            return BadRequest("El monto no puede ser mayor que la deuda.");

        if (model.Monto == model.Saldo_Pendiente)
            model.Estado = 11; 
        else
            model.Estado = 10; 

        var parametros = new DynamicParameters();
        parametros.Add("@Consecutivo", model.Consecutivo);
        parametros.Add("@Descripcion", model.Descripcion);
        parametros.Add("@Monto", model.Monto);
        parametros.Add("@Saldo_Pendiente", model.Saldo_Pendiente);
        parametros.Add("@Estado", model.Estado);

        var result = context.Execute(
            "sp_EditarIngreso",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (result <= 0)
            return BadRequest("No se pudo actualizar el ingreso");

        return Ok("Ingreso actualizado correctamente");
    }


    [HttpGet("ObtenerIngresoId/{id}")]
    public IActionResult ObtenerIngresoId(int id)
    {
        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@Consecutivo", id);

        var vehiculo = context.QueryFirstOrDefault<IngresoResponse>(
            "sp_ObtenerIngresoId",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (vehiculo == null)
            return NotFound("Ingreso no encontrado");

        return Ok(vehiculo);
    }
    #endregion

    #region Egresos
    [HttpPost("RegistrarEgreso")]
    public IActionResult RegistrarEgreso(EgresoRequest model)
    {
        using var context = new SqlConnection(
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@Motivo", model.Motivo);
        parametros.Add("@Monto", model.Monto);
        parametros.Add("@Cantidad", model.Cantidad);
        parametros.Add("@RegistradoPor", model.RegistradoPor);
        parametros.Add("@MetodoPago", model.MetodoPago);

        var result = context.Execute(
            "sp_RegistroEgreso", parametros);

        return Ok("Egreso registrado correctamente");
    }
    [HttpPost("ConsultarEgreso")]
    public IActionResult ConsultarEgreso()
    {
        using var context = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var lista = context.Query<EgresoResponse>("sp_ConsultarEgreso",
            commandType: System.Data.CommandType.StoredProcedure).ToList();

        if (!lista.Any())
            return NotFound("No hay Egresos registrados");

        return Ok(lista);
    }

    [HttpPut("EditarEgreso")]
    public IActionResult EditarEgreso([FromBody] EgresoRequest model)
    {
        if (model.Consecutivo <= 0)
            return BadRequest("El ID no es válido");

        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@Consecutivo", model.Consecutivo);
        parametros.Add("@Motivo", model.Motivo);
        parametros.Add("@Monto", model.Monto);
        parametros.Add("@Cantidad", model.Cantidad);
        parametros.Add("@RegistradoPor", model.RegistradoPor);
        parametros.Add("@MetodoPago", model.MetodoPago);

        var result = context.Execute(
            "sp_EditarEgreso",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (result <= 0)
            return BadRequest("No se pudo actualizar el Egreso");

        return Ok("Egreso actualizado correctamente");
    }

    [HttpGet("ObtenerEgresoId/{id}")]
    public IActionResult ObtenerEgresoId(int id)
    {
        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@Consecutivo", id);

        var vehiculo = context.QueryFirstOrDefault<EgresoResponse>(
            "sp_ObtenerEgresoId",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (vehiculo == null)
            return NotFound("Egreso no encontrado");

        return Ok(vehiculo);
    }

   
    #endregion
}
