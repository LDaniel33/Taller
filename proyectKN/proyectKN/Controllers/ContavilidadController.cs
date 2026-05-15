using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectKN.Filters;
using proyectKN.Models;
using System.Linq;
using System.Net;

namespace proyectKN.Controllers
{
    [SesionActiva]

    public class ContavilidadController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        public ContavilidadController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        #region Registro Ingreso
        [HttpGet]
        public IActionResult RegistroIngreso()
        {
            using var client = _http.CreateClient();
            var urlIngreso = _config.GetValue<string>("Valores:UrlAPI") + "Contabilidad/ConsultarIngreso";
            var urlEstados = _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/4";
          
            var response = client.PostAsync(urlIngreso, null).Result;

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Ingreso = response.Content
                                                    .ReadFromJsonAsync<List<IngresoConsulta>>()
                                                    .Result;
            }
            else
            {
                ViewBag.Ingreso = new List<IngresoConsulta>();
            }

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
            return View();

        }

        // POST - CREAR
        [HttpPost]
        public IActionResult RegistroIngreso(Ingreso model)
        {
            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Contabilidad/RegistrarIngreso";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Ingreso registrado correctamente";
            }
            else
            {
                var error = result.Content.ReadAsStringAsync().Result;

                TempData["Error"] = string.IsNullOrWhiteSpace(error)
                    ? "No se pudo registrar el ingreso"
                    : error;
            }

            return RedirectToAction("RegistroIngreso", "Contavilidad");
        }

        #endregion

        #region EDITAR INGRESO
        [HttpGet]
        public IActionResult EditarIngreso(int id)
        {
            using var client = _http.CreateClient();

            var responseEstados = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/4"
            ).Result;
            if (responseEstados.IsSuccessStatusCode)
            {
                ViewBag.Estado = responseEstados.Content
                    .ReadFromJsonAsync<List<EstadoM>>().Result;
            }
            else
            {
                ViewBag.Estado = new List<EstadoM>();
            }
            var response = client.GetAsync(_config.GetValue<string>("Valores:UrlAPI")
                + $"Contabilidad/ObtenerIngresoId/{id}").Result;

            Ingreso ingreso = null;
            if (response.IsSuccessStatusCode)
            {
                ingreso = response.Content.ReadFromJsonAsync<Ingreso>().Result;
            }
            else
            {
                TempData["Error"] = "No se pudo obtener el ingreso.";
                return RedirectToAction("RegistroIngreso");
            }
            return View(ingreso);
        }

        [HttpPost]
        public IActionResult EditarIngreso(Ingreso model)
        {
            using var client = _http.CreateClient();
            var response = client.PutAsJsonAsync(_config.GetValue<string>("Valores:UrlAPI")
                + "Contabilidad/EditarIngreso", model).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Ingreso editado exitosamente.";
                return RedirectToAction("RegistroIngreso");
            }
            else
            {
                TempData["Error"] = "El monto no debe de ser mayor a la deuda.";

            }
            return RedirectToAction("EditarIngreso");
        }
        #endregion

        #region REGISTRO EGRESO
        [HttpGet]
        public IActionResult RegistroEgreso()
        {
            using var client = _http.CreateClient();
            var urlEgreso = _config.GetValue<string>("Valores:UrlAPI") + "Contabilidad/ConsultarEgreso";
            var urlUsuario = _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ObtenerUsuario";


            // USUARIO
            var responseUsuario = client.GetAsync(urlUsuario).Result;
            if (responseUsuario.IsSuccessStatusCode)
            {
                ViewBag.Usuario = responseUsuario.Content
                                     .ReadFromJsonAsync<List<UsuarioConsulta>>()
                                     .  Result;
            }
            else
            {
                ViewBag.Usuario = new List<UsuarioConsulta>();
            }


            var responseEgreso = client.PostAsync(urlEgreso, null).Result;

            if (responseEgreso.IsSuccessStatusCode)
            {
                ViewBag.Egreso = responseEgreso.Content
                                                    .ReadFromJsonAsync<List<EgresoConsulta>>()
                                                    .Result;
            }
            else
            {
                ViewBag.Egreso = new List<EgresoConsulta>();
            }


            return View();
        }

        [HttpPost]
        public IActionResult RegistroEgreso(Egreso model)
        {
            var usuarioId = HttpContext.Session.GetString("IdUsuario");

            if (!int.TryParse(usuarioId, out int idUsuario))
            {
                TempData["Error"] = "No se encontró el usuario en sesión.";
                return RedirectToAction("Login", "Home");
            }

            model.RegistradoPor = idUsuario;

            using var client = _http.CreateClient();

            var url = _config.GetValue<string>("Valores:UrlAPI") + "Contabilidad/RegistrarEgreso";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Egreso registrado correctamente";
            }
            else
            {
                var error = result.Content.ReadAsStringAsync().Result;

                TempData["Error"] = string.IsNullOrWhiteSpace(error)
                    ? "No se pudo registrar el egreso"
                    : error;
            }

            return RedirectToAction("RegistroEgreso", "Contavilidad");
        }
        #endregion


        #region Editar Egreso
        [HttpGet]
        public IActionResult EditarEgreso(int id)
        {
            using var client = _http.CreateClient();
    
            // USUARIOS
            var responseUsuario = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ObtenerUsuario").Result;

            if (responseUsuario.IsSuccessStatusCode)
            {
                ViewBag.Usuario = responseUsuario.Content
                    .ReadFromJsonAsync<List<UsuarioConsulta>>().Result;
            }
            else
            {
                ViewBag.Usuario = new List<UsuarioConsulta>();
            }

            var responseEgreso = client.GetAsync(
           _config.GetValue<string>("Valores:UrlAPI")
                + $"Contabilidad/ObtenerEgresoId/{id}")
                .Result;
            

            Egreso egreso = null;
            if (responseEgreso.IsSuccessStatusCode)
            {
                egreso = responseEgreso.Content
                    .ReadFromJsonAsync<Egreso>().Result;
            }
            else
            {
                TempData["Error"] = "No se pudo obtener el egreso.";
                return RedirectToAction("RegistroEgreso");
            }
            return View(egreso);

        }
        [HttpPost]
        public IActionResult EditarEgreso(Egreso model)
        {
            using var client = _http.CreateClient();
            var response = client.PutAsJsonAsync(_config.GetValue<string>("Valores:UrlAPI")
                + "Contabilidad/EditarEgreso", model).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Egreso editado exitosamente.";
            }
            else
            {
                TempData["Error"] = "No se pudo editar el egreso.";

            }
            return RedirectToAction("RegistroEgreso");

        }
        #endregion
    }
}