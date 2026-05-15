using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using proyectKN_API.Models;
using System.Data;

namespace proyectKN_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CitaController(IConfiguration config)
        {
            _config = config;
        }
        //REGISTRO
        [HttpPost("RegistroCita")]
        public IActionResult RegistroCita(CitaRequest model)
        {
            using var context = new SqlConnection(
                _config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            
            if (model.Telefono == 0)
                return BadRequest("El teléfono es obligatorio.");

            var telefonoTexto = model.Telefono.ToString();

            if (telefonoTexto.Length != 8)
                return BadRequest("El teléfono debe tener 8 dígitos.");

            var fechaHoraCita = model.FechaCita.Date + model.HoraCita;
            var ahora = DateTime.Now;

            if (fechaHoraCita <= ahora)
                return BadRequest("No se puede registrar una cita en una hora que ya pasó.");

            var existe = context.QueryFirstOrDefault<int>(
                @"SELECT COUNT(1)
          FROM Citas
          WHERE FechaCita = @FechaCita
            AND HoraCita = @HoraCita
            AND Estado <> 5",
                new
                {
                    FechaCita = model.FechaCita,
                    HoraCita = model.HoraCita
                }
            );

            if (existe > 0)
                return BadRequest("Esa hora ya está ocupada para la fecha seleccionada.");
            var citaActivaCliente = context.QueryFirstOrDefault<int>(
                    @"SELECT COUNT(1)
          FROM Citas
          WHERE Cedula = @Cedula
            AND Estado IN (3, 4)",
                    new
                    {
                        Cedula = model.Cedula
                    }
                );

            if (citaActivaCliente > 0)
                return BadRequest("El cliente ya tiene agendada una cita en el sistema.");

            model.Estado = 3; 

            var parametros = new DynamicParameters();
            parametros.Add("@NombreCliente", model.NombreCliente);
            parametros.Add("@Cedula", model.Cedula);
            parametros.Add("@FechaCita", model.FechaCita);
            parametros.Add("@HoraCita", model.HoraCita);
            parametros.Add("@Telefono", model.Telefono);
            parametros.Add("@Email", model.Email);
            parametros.Add("@Servicio", model.Servicio);
            parametros.Add("@Estado", model.Estado);
            parametros.Add("@CreadaPor", model.CreadaPor);

            var result = context.Execute(
                "sp_RegistroCita",
                parametros,
                commandType: CommandType.StoredProcedure
            );

            if (result <= 0)
                return BadRequest("La cita no se registró correctamente");

            return Ok("La cita se registró correctamente");
        }

        //CONSULTA
        [HttpPost("ConsultarCita")]
        public IActionResult ConsultarCita()
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            
            context.Execute(@"
        UPDATE Citas
        SET Estado = 6
        WHERE Estado IN (3, 4)
          AND DATEADD(SECOND, DATEDIFF(SECOND, '00:00:00', HoraCita), CAST(FechaCita AS DATETIME)) < GETDATE()
    ");

            var lista = context.Query<CitaResponse>(
                "sp_ConsultaCita",
                commandType: System.Data.CommandType.StoredProcedure
            ).ToList();

            return Ok(lista);
        }




        [HttpGet("ObtenerCitaId/{id}")]
        public IActionResult ObtenerCitaId(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Consecutivo", id);

            var result = context.QueryFirstOrDefault<CitaRequest>(
                "sp_ObtenerCitaId",
                parametros,
                commandType: CommandType.StoredProcedure);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("EditarCita")]
        public IActionResult EditarCita(CitaRequest model)
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
            parametros.Add("@NombreCliente", model.NombreCliente);
            parametros.Add("@Cedula", model.Cedula);
            parametros.Add("@FechaCita", model.FechaCita);
            parametros.Add("@HoraCita", model.HoraCita);
            parametros.Add("@Telefono", model.Telefono);
            parametros.Add("@Email", model.Email);
            parametros.Add("@Servicio", model.Servicio);
            parametros.Add("@Estado", model.Estado);
            parametros.Add("@CreadaPor", model.CreadaPor);
            parametros.Add("@ModificadoPor", model.ModificadoPor);


            var result = context.Execute(
                "sp_EditarCita",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure);

            if (result <= 0)
                return BadRequest("No se pudo actualizar la cita");

            return Ok("Cita actualizada correctamente");
        }
        [HttpPut("CancelarCita/{id}")]
        public IActionResult CancelarCita(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Consecutivo", id);
            parametros.Add("@Estado", 5);

            var result = context.Execute(
                "sp_CancelarCita",
                parametros,
                commandType: CommandType.StoredProcedure);

            if (result <= 0)
                return BadRequest("No se pudo cancelar");

            return Ok();
        }
        [HttpGet("ObtenerHorasOcupadas")]
        public IActionResult ObtenerHorasOcupadas(string fecha, int? idCita)
        {
            using var context = new SqlConnection(
                _config.GetValue<string>("ConnectionStrings:DefaultConnection"));

            var horas = context.Query<string>(
                @"SELECT CONVERT(VARCHAR(5), HoraCita, 108)
          FROM Citas
          WHERE FechaCita = @FechaCita
          AND Estado != 5
          AND (@IdCita IS NULL OR Consecutivo <> @IdCita)",
                new
                {
                    FechaCita = fecha,
                    IdCita = idCita
                }).ToList();

            return Ok(horas);
        }


    }
}

