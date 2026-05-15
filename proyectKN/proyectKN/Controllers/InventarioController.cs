using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using proyectKN.Filters;
using proyectKN.Models;
using System.Net;

namespace proyectKN.Controllers
{
    [SesionActiva]
   
    public class InventarioController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public InventarioController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }
        // GET REGISTRO USUARIO
        [HttpGet]
        public IActionResult RegistroInventario()
        {

            using var client = _http.CreateClient();

            var urlProveedores = _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/ObtenerProveedores";
            var urlInventario = _config.GetValue<string>("Valores:UrlAPI") + "Inventario/ConsultarInventario";

            // Proveedor
            var responseProveedores = client.PostAsync(urlProveedores, null).Result;

            if (responseProveedores.IsSuccessStatusCode)
            {
                ViewBag.Proveedor = responseProveedores.Content
                                             .ReadFromJsonAsync<List<Proveedor>>()
                                             .Result;
            }
            else
            {
                ViewBag.Proveedor = new List<Proveedor>();
            }

          

            var responseInventario = client.PostAsync(urlInventario, null).Result;
            if (responseInventario.IsSuccessStatusCode)
            {
                ViewBag.Inventario = responseInventario.Content
                                                    .ReadFromJsonAsync<List<InventarioConsulta>>()
                                                    .Result;
            }
            else
            {
                ViewBag.Inventario = new List<InventarioConsulta>();
            }
            return View();
        }

        // POST REGISTRO USUARIO
        [HttpPost]
        public IActionResult RegistroInventario(Inventario model)
        {
            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Inventario/RegistroInventario";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Producto registrado correctamente";
            }
            else
            {
                var error = result.Content.ReadAsStringAsync().Result;

                TempData["Error"] = string.IsNullOrWhiteSpace(error)
                    ? "No se pudo registrar el producto"
                    : error;
            }

            return RedirectToAction("RegistroInventario", "Inventario");
        }

        [HttpGet]
        public IActionResult EditarInventario(int id)
        {
            using var client = _http.CreateClient();
            List<Proveedor> listaProveedores = new List<Proveedor>();
            // PROVEEDORES
            var responseProveedor = client.PostAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/ObtenerProveedores", null).Result;

            if (responseProveedor.IsSuccessStatusCode)
            {
                listaProveedores = responseProveedor.Content
                    .ReadFromJsonAsync<List<Proveedor>>().Result;
                ViewBag.Proveedor = listaProveedores;
            }

            // INVENTARIO POR ID
            var responseInventario = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + $"Inventario/ObtenerInventarioId/{id}").Result;

            Inventario inventario = null;

            if (responseInventario.IsSuccessStatusCode)
            {
                inventario = responseInventario.Content
                    .ReadFromJsonAsync<Inventario>().Result;
            }
            else
            {
                TempData["Error"] = "No se pudo cargar el producto.";
                return RedirectToAction("RegistroInventario");
            }

            var proveedorActual = listaProveedores
        .FirstOrDefault(p => p.Consecutivo == inventario.Proveedor);

            ViewBag.ProveedorNombre = proveedorActual?.Nombre ?? "";

            return View(inventario);
        }
        

        [HttpPost]
        public IActionResult EditarInventario(Inventario model)
        {
            using var client = _http.CreateClient();

            var response = client.PutAsJsonAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Inventario/EditarInventario", model).Result;

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Producto actualizado correctamente";
            else
                TempData["Error"] = "No se pudo actualizar el producto";

            return RedirectToAction("RegistroInventario");
        }
    }
}

