using Dapper;
using proyectKN_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
namespace proyectKN_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolController : ControllerBase
{
    private readonly IConfiguration _config;
    public RolController(IConfiguration config)
    {
        _config = config;
    }
    [HttpPost("ObtenerRoles")]
    public IActionResult ObtenerRoles()
    {
        using var context = new SqlConnection( 
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

        var result = context.Query<Rol>(
            "sp_ObtenerRoles",
            commandType: System.Data.CommandType.StoredProcedure
        ).ToList();

        if (result == null || !result.Any())
            return NotFound("No hay roles registrados");

        return Ok(result);
    }
}
