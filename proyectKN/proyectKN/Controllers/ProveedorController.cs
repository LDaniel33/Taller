using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using proyectKN.Filters;
using proyectKN.Models;
using System.Net;

namespace proyectKN.Controllers
{
    [SesionActiva]
   
    public class ProveedorController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public ProveedorController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        // GET REGISTRO PROVEEDOR
        [HttpGet]
        public IActionResult RegistroProveedor()
        {
            using var client = _http.CreateClient();

            var url = _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/ConsultarProveedor";

            var response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Proveedores = response.Content
                                               .ReadFromJsonAsync<List<Proveedor>>()
                                               .Result;
            }
            else
            {
                ViewBag.Proveedores = new List<Proveedor>();
            }

            return View();
        }

        // POST REGISTRO PROVEEDOR
        [HttpPost]
        public IActionResult RegistroProveedor(Proveedor model)
        {
            using var client = _http.CreateClient();

            var url = _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/RegistroProveedor";

            var result = client.PostAsJsonAsync(url, model).Result;


            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Proveedor registrado correctamente";
                return RedirectToAction("RegistroProveedor");
            }
            else
            {
                ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;


                model.Telefono = 0;


                var urlProveedores = _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/ConsultarProveedor";
                var response = client.PostAsync(urlProveedores, null).Result;

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Proveedores = response.Content
                        .ReadFromJsonAsync<List<Proveedor>>()
                        .Result;
                }
                else
                {
                    ViewBag.Proveedores = new List<Proveedor>();
                }

                return View(model);
            }
        }
        //EDITAR GET
        [HttpGet]
        public IActionResult EditarProveedor(int id)
        {
            using var client = _http.CreateClient();

            var url = _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/ConsultarProveedor";

            var response = client.PostAsync(url, null).Result;

            if (response.IsSuccessStatusCode)
            {
                var lista = response.Content
                    .ReadFromJsonAsync<List<Proveedor>>()
                    .Result;

                var proveedor = lista.FirstOrDefault(x => x.Consecutivo == id);

                if (proveedor == null)
                {
                    return Content("No se encontró el proveedor con id: " + id);
                }

                return View(proveedor);
            }

            return RedirectToAction("RegistroProveedor");
        }
        //EDITAR POST
        [HttpPost]
        public IActionResult EditarProveedor(Proveedor model)
        {
            using var client = _http.CreateClient();

            var url = _config.GetValue<string>("Valores:UrlAPI") + "Proveedor/EditarProveedor";

            var result = client.PutAsJsonAsync(url, model).Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Proveedor actualizado correctamente";
                return RedirectToAction("RegistroProveedor");
            }
            else
            {
                ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;

                
                model.Telefono = 0;

                return View(model);
            }
        }
    }
}