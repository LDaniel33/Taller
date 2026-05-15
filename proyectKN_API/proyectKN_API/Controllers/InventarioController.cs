
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
    public class InventarioController : ControllerBase
    {
        private readonly IConfiguration _config;

        public InventarioController(IConfiguration config)
        {
            _config = config;
        }
        //REGISTRO
        [HttpPost("RegistroInventario")]
        public IActionResult RegistroInventario(InventarioRequest model)
        {
            using var context = new SqlConnection(
                _config.GetValue<string>("ConnectionStrings:DefaultConnection"));
            var parametros = new DynamicParameters();
            parametros.Add("@Nombre", model.Nombre);
            parametros.Add("@IdArticulo", model.IdArticulo);
            parametros.Add("@Descripcion", model.Descripcion);
            parametros.Add("@PrecioCompra", model.PrecioCompra);
            parametros.Add("@PrecioVenta", model.PrecioVenta);
            parametros.Add("@Stock", model.Stock);
            parametros.Add("@StockMinimo", model.StockMinimo);
            parametros.Add("@Proveedor", model.Proveedor);

            var result = context.Execute(
                "sp_RegistroInventario",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
                );
            if (result <= 0)
                return BadRequest("EL artículo no sé registró correctamente");

            return Ok("El articulo se registro correctamente");
        }

        //CONSULTA
        [HttpPost("ConsultarInventario")]
        public IActionResult ConsultarInventario()
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var lista = context.Query<InventarioResponse>(
                "sp_ConsultaInventario",
                commandType: System.Data.CommandType.StoredProcedure
            ).ToList();

            if (!lista.Any())
                return NotFound("No hay inventario registrado");

            return Ok(lista);
        }

        [HttpGet("ObtenerProveedoresId/{id}")]
        public IActionResult ObtenerProveedoresId(int id)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Consecutivo", id);

            var proveedor = context.QueryFirstOrDefault<InventarioResponse>(
                "sp_ObtenerIdProvee",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure
            );

            if (proveedor == null)
                return NotFound("Proveedor no encontrado");

            return Ok(proveedor);
        }

            [HttpGet("ObtenerInventarioId/{id}")]
            public IActionResult ObtenerInventarioId(int id)
            {
                using var context = new SqlConnection(
                    _config.GetConnectionString("DefaultConnection"));

                var parametros = new DynamicParameters();
                parametros.Add("@Consecutivo", id);

                var result = context.QueryFirstOrDefault<InventarioRequest>(
                    "sp_ObtenerInventarioId",
                    parametros,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
        
        [HttpPut("EditarInventario")]
        public IActionResult EditarInventario(InventarioRequest model)
        {
            using var context = new SqlConnection(
                _config.GetConnectionString("DefaultConnection"));

            var parametros = new DynamicParameters();
            parametros.Add("@Consecutivo", model.Consecutivo);
            parametros.Add("@Nombre", model.Nombre);
            parametros.Add("@IdArticulo", model.IdArticulo);
            parametros.Add("@Descripcion", model.Descripcion);
            parametros.Add("@PrecioCompra", model.PrecioCompra);
            parametros.Add("@PrecioVenta", model.PrecioVenta);
            parametros.Add("@Stock", model.Stock);
            parametros.Add("@StockMinimo", model.StockMinimo);
            parametros.Add("@Proveedor", model.Proveedor);


            var result = context.Execute(
                "sp_EditarInventario",
                parametros,
                commandType: System.Data.CommandType.StoredProcedure);

            if (result <= 0)
                return BadRequest("No se pudo actualizar el producto");

            return Ok("Producto actualizado correctamente");
        }
    }
}
    

