
using Dapper;
using proyectKN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
namespace proyectKN_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProveedorController : ControllerBase
{
    private readonly IConfiguration _config;

    public ProveedorController(IConfiguration config)
    {
        _config = config;
    }

    //  REGISTRO PROVEEDOR
    [HttpPost("RegistroProveedor")]
    public IActionResult RegistroProveedor(ProveedorRequest model)
    {
        using var context = new SqlConnection(
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

        if (model.Telefono == 0)
            return BadRequest("El teléfono es obligatorio.");

        var telefonoTexto = model.Telefono.ToString();

        if (telefonoTexto.Length != 8)
            return BadRequest("El teléfono debe tener 8 dígitos.");

        var parametros = new DynamicParameters();
        parametros.Add("@Nombre", model.Nombre);
        parametros.Add("@Telefono", model.Telefono);
        parametros.Add("@Correo", model.Correo);
        parametros.Add("@Direccion", model.Direccion);

        var result = context.Execute(
            "sp_RegistroProveedor",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure
        );

        if (result <= 0)
            return BadRequest("El proveedor no se registró");

        return Ok("Proveedor registrado correctamente");
    }


    //  CONSULTA INVENTARIO
    [HttpPost("ConsultarProveedor")]
    public IActionResult CConsultarProveedor()
    {
        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));

        var lista = context.Query<ProveedorResponse>(
            "sp_ConsultarProveedor",
            commandType: System.Data.CommandType.StoredProcedure
        ).ToList();

        if (!lista.Any())
            return NotFound("No hay usuarios registrados");

        return Ok(lista);
    }

    //EDITAR    
    [HttpPut("EditarProveedor")]
    public IActionResult EditarProveedor(ProveedorRequest model)
    {
        using var context = new SqlConnection(
            _config.GetConnectionString("DefaultConnection"));
        if (model.Telefono == 0)
            return BadRequest("El teléfono es obligatorio.");

        var telefonoTexto = model.Telefono.ToString();

        if (telefonoTexto.Length != 8)
            return BadRequest("El teléfono debe tener 8 dígitos.");

        var parametros = new DynamicParameters();

        parametros.Add("@Consecutivo", model.Consecutivo);
        parametros.Add("@Nombre", model.Nombre);
        parametros.Add("@Telefono", model.Telefono);
        parametros.Add("@Correo", model.Correo);
        parametros.Add("@Direccion", model.Direccion);

        var result = context.Execute(
            "sp_EditarProveedor",
            parametros,
            commandType: System.Data.CommandType.StoredProcedure);

        if (result <= 0)
            return BadRequest("No se pudo actualizar el proveedor");

        return Ok("Proveedor actualizado correctamente");
    }

    //obtener proveedor para inventario 
    [HttpPost("ObtenerProveedores")]
    public IActionResult ObtenerProveedores()
    {
        using var context = new SqlConnection(
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

        var result = context.Query<ProveedorResponse>(
            "sp_ObtenerProveedores",
            commandType: System.Data.CommandType.StoredProcedure
        ).ToList();

        if (result == null || !result.Any())
            return NotFound("No hay proveedores registrados");

        return Ok(result);
    }
}


