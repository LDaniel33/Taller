 using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using proyectKN_API.Models;
using System.Data;
using proyectKN_API.Services;

namespace proyectKN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPasswordHelper _password;

        public HomeController(IConfiguration config, IPasswordHelper password)
        {
            _config = config;
            _password = password;
        }
        [HttpPost("IniciarSesion")]
        public IActionResult IniciarSesion(LoginRequest model)
        {
            using var context = new SqlConnection(
                _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@CorreoElectronico", model.Correo);
            parametros.Add("@Contrasenna", _password.Encrypt(model.Contrasenna));

            var result = context.QueryFirstOrDefault<UsuarioResponse>(
                "sp_IniciarSesion",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            if (result == null)
                return NotFound("Su información no se autenticó correctamente");

            return Ok(result);
        }

        [HttpPut("RecuperarAcceso")]
        public IActionResult RecuperarAcceso(RecuperarAccesoRequest model)
        {
            using var context = new SqlConnection(
                _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Correo", model.Correo);

            var result = context.QueryFirstOrDefault<UsuarioResponse>(
                "sp_RecuperarContrasenna",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            // Solo usuarios registrados y activos
            if (result == null || string.IsNullOrWhiteSpace(result.Estado) ||
             !result.Estado.Trim().Equals("Activo", StringComparison.OrdinalIgnoreCase))
            {
                return NotFound("El correo no está registrado");
            }
            var nuevaContrasenna = GenerarContrasenna();

            var parametrosActualizacion = new DynamicParameters();
            parametrosActualizacion.Add("@Consecutivo", result.Consecutivo);
            parametrosActualizacion.Add("@Contrasenna", _password.Encrypt(nuevaContrasenna));

            var filas = context.QuerySingle<int>(
            "sp_ActualizarContrasenna",
            parametrosActualizacion,
            commandType: System.Data.CommandType.StoredProcedure
);

            if (filas <= 0)
                return BadRequest("No se pudo actualizar la contraseña");

            var contenido = ObtenerPlantillaCorreo(result.NombreCompleto, nuevaContrasenna);

            _password.EnviarCorreo(
                result.Correo,
                "Recuperación de Acceso",
                contenido
            );

            return Ok("Se envió una nueva contraseña al correo");
        }

        private static string GenerarContrasenna()
        {
            const string letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string([.. Enumerable.Range(0, 8)
                .Select(_ => letras[Random.Shared.Next(letras.Length)])]);
        }

        private static string ObtenerPlantillaCorreo(string nombre, string contrasenna)
        {
            var ruta = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "RecuperarAcceso.html");
            var plantilla = System.IO.File.ReadAllText(ruta);

            return plantilla
                .Replace("{{Nombre}}", nombre)
                .Replace("{{Contrasenna}}", contrasenna);
        }

    }
}

    