using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using proyectKN_API.Models;
using System.Data;
namespace proyectKN_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EstadoController : ControllerBase
{
    private readonly IConfiguration _config;
    public EstadoController(IConfiguration config)
    {
        _config = config;
    }
    [HttpGet("ObtenerEstado/{idTipo}")]
    public IActionResult ObtenerEstado(int idTipo)
    {
        using var context = new SqlConnection(
            _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

        var parametros = new DynamicParameters();
        parametros.Add("@IdTipoEstado", idTipo);

        var result = context.Query<EstadoM>(
            "sp_ObtenerEstado",
            parametros,
            commandType: CommandType.StoredProcedure
        ).ToList();

        if (result == null || !result.Any())
            return NotFound("No hay estados registrados");

        return Ok(result);
    }
}