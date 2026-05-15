using Dapper;
using proyectKN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
namespace proyectKN_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiculoController : ControllerBase
{
    private readonly IConfiguration _config;
    public VehiculoController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("RegistrarVehiculo")]
    public IActionResult RegistrarVehiculo(VehiculoRequest model)
    {
        using var context = new SqlConnection(
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));
       
        if (model.Anio == 0)
            return BadRequest("El año es obligatorio.");

        var anioTexto = model.Anio.ToString();

        if (anioTexto.Length != 4)
            return BadRequest("El año debe tener 4 dígitos.");

        if (model.Telefono == 0)
            return BadRequest("El teléfono es obligatorio.");


        var telefonoTexto = model.Telefono.ToString();

        if (telefonoTexto.Length != 8)
            return BadRequest("El teléfono debe tener 8 dígitos.");


        var parametros = new DynamicParameters();
        parametros.Add("@Nombre_Cliente", model.Nombre_Cliente);
        parametros.Add("@Telefono", model.Telefono);
        parametros.Add("@Cedula", model.Cedula);
        parametros.Add("@Placa", model.Placa);
        parametros.Add("@Marca", model.Marca);
        parametros.Add("@Modelo", model.Modelo);
        parametros.Add("@Anio", model.Anio);
        parametros.Add("@Problema", model.Problema);
        parametros.Add("@Revision", model.Revision);
        parametros.Add("@Estado", model.Estado);

        var result = context.Execute(
            "sp_RegistroVehiculo", parametros);

        if (result == -1)
            return BadRequest("La cédula ya está registrada");

        return Ok("Vehículo registrado correctamente");
    }

    [HttpPost("ConsultarVehiculos")]
    public IActionResult ConsultarVehiculo()
    {
        using var context = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var lista = context.Query<VehiculoResponse>("sp_ConsultarVehiculos",
            commandType: System.Data.CommandType.StoredProcedure).ToList();

        if (!lista.Any())
            return NotFound("No hay Vehículos registrados");

        return Ok(lista);
    }

    [HttpPut("EditarVehiculo")]
    public IActionResult EditarVehiculo([FromBody] VehiculoRequest model)
    {
        if (model.Consecutivo <= 0)
            return BadRequest("El ID no es válido");
        if (model.Anio == 0)
            return BadRequest("El año es obligatorio.");

        var anioTexto = model.Anio.ToString();

        if (anioTexto.Length != 4)
            return BadRequest("El año debe tener 4 dígitos.");

        if (model.Telefono == 0)
            return BadRequest("El teléfono es obligatorio.");


        var telefonoTexto = model.Telefono.ToString();

        if (telefonoTexto.Length != 8)
            return BadRequest("El teléfono debe tener 8 dígitos.");

        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@Consecutivo", model.Consecutivo);
        parametros.Add("@Nombre_Cliente", model.Nombre_Cliente);
        parametros.Add("@Telefono", model.Telefono);
        parametros.Add("@Cedula", model.Cedula);
        parametros.Add("@Placa", model.Placa);
        parametros.Add("@Marca", model.Marca);
        parametros.Add("@Modelo", model.Modelo);
        parametros.Add("@Anio", model.Anio);
        parametros.Add("@Problema", model.Problema);
        parametros.Add("@Revision", model.Revision);
        parametros.Add("@Estado", model.Estado);

        var result = context.Execute(
            "sp_EditarVehiculo",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (result <= 0)
            return BadRequest("No se pudo actualizar el vehículo");

        return Ok("Vehículo actualizado correctamente");
    }

    [HttpGet("ObtenerVehiculoId/{id}")]
    public IActionResult ObtenerVehiculoId(int id)
    {
        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@Consecutivo", id);

        var vehiculo = context.QueryFirstOrDefault<VehiculoResponse>(
            "sp_ObtenerVehiculoId",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (vehiculo == null)
            return NotFound("Vehículo no encontrado");

        return Ok(vehiculo);
    }

}
