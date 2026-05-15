
using Microsoft.AspNetCore.Mvc;
using proyectKN.Filters;
using proyectKN.Models;
using proyectKN.Services;
using System.Linq;
using System.Net;

namespace proyectKN.Controllers
{
    [SesionActiva]

    public class VehiculoController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        public VehiculoController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        #region REGISTRO VEHICULO
        [HttpGet]
        public IActionResult RegistroVehiculo()
        {
            using var client = _http.CreateClient();
            var urlEstados = _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/3";
            var urlVehiculos = _config.GetValue<string>("Valores:UrlAPI") + "Vehiculo/ConsultarVehiculos";


            // Estados
            var responseEstados = client.GetAsync(urlEstados).Result;

            if (responseEstados.IsSuccessStatusCode)
            {
                ViewBag.Estado = responseEstados.Content
                                                 .ReadFromJsonAsync<List<EstadoM>>()
                                                 .Result;
            }
            else
            {
                ViewBag.Estado = new List<EstadoM>();
            }

            // Vehiculos
            var responseVehiculos = client.PostAsync(urlVehiculos, null).Result;
            if (responseVehiculos.IsSuccessStatusCode)
            {
                ViewBag.Vehiculos = responseVehiculos.Content
                                                    .ReadFromJsonAsync<List<VehiculoConsultas>>()
                                                    .Result;
            }
            else
            {
                ViewBag.Vehiculos = new List<VehiculoConsultas>();
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegistroVehiculo(Vehiculo model)
        {
            using var client = _http.CreateClient();
            model.Estado = 7;
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Vehiculo/RegistrarVehiculo";
            var result = client.PostAsJsonAsync(url, model).Result;
            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "El vehículo fue registrado exitosamente";
                return RedirectToAction("RegistroVehiculo");
            }
            else
            {
                ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;

    
                model.Telefono = 0;

              
                var urlVehiculos = _config.GetValue<string>("Valores:UrlAPI") + "Vehiculo/ConsultarVehiculos";
                var responseVehiculos = client.PostAsync(urlVehiculos, null).Result;

                if (responseVehiculos.IsSuccessStatusCode)
                {
                    ViewBag.Vehiculos = responseVehiculos.Content
                        .ReadFromJsonAsync<List<VehiculoConsultas>>()
                        .Result;
                }
                else
                {
                    ViewBag.Vehiculos = new List<VehiculoConsultas>();
                }

                return View(model);
            }
        }

        #endregion

        #region EDITAR VEHICULO
        [HttpGet]
        public IActionResult EditarVehiculo(int id)
        {
            using var client = _http.CreateClient();
            var responseEstados = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/3" 
                ).Result;

            if (responseEstados.IsSuccessStatusCode)
            {
                ViewBag.Estado = responseEstados.Content
                                                .ReadFromJsonAsync<List<EstadoM>>()
                                                .Result;
            }
            else
            {
                ViewBag.Estado = new List<EstadoM>();
            }
            //Vehiculo 
            var responseVehiculos = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") +$"Vehiculo/ObtenerVehiculoId/{id}"
                ).Result;

            Vehiculo vehiculo = null;
            if (responseVehiculos.IsSuccessStatusCode)
            {
                vehiculo = responseVehiculos.Content.
                    ReadFromJsonAsync<Vehiculo>()
                    .Result;
            }
            else
            {
                TempData["Error"] = "No se pudo obtener el vehículo.";
                return RedirectToAction("RegistroVehiculo");
            }
            return View(vehiculo);
        }


        // EDITAR POST
        [HttpPost]
        public IActionResult EditarVehiculo(Vehiculo model)
        {
            using var client = _http.CreateClient();
            var response = client.PutAsJsonAsync(_config.GetValue<string>("Valores:UrlAPI")
                + "Vehiculo/EditarVehiculo", model).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Vehículo editado exitosamente.";
                return RedirectToAction("RegistroVehiculo");
            }
            else
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;

                model.Telefono = 0;

                var urlEstado = _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/3";
                var responseEstado = client.GetAsync(urlEstado).Result;

                if (responseEstado.IsSuccessStatusCode)
                {
                    ViewBag.Estado = responseEstado.Content
                        .ReadFromJsonAsync<List<EstadoM>>()
                        .Result;
                }
                else
                {
                    ViewBag.Estado = new List<EstadoM>();
                }

                return View(model);
            }
        }
        #endregion

    }
}

