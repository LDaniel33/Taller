using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using proyectKN.Filters;
using proyectKN.Models;
using System.Net;


namespace proyectKN.Controllers
{
    [SesionActiva]
 
    public class CitaController : Controller
    {

        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public CitaController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }
        // GET REGISTRO 
        [HttpGet]
        public IActionResult RegistroCita()
        {
            using var client = _http.CreateClient();

            var urlUsuario = _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ObtenerUsuario";
            var urlEstadoCita = _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/2";
            var urlCita = _config.GetValue<string>("Valores:UrlAPI") + "Cita/ConsultarCita";

            // USUARIO
            var responseUsuario = client.GetAsync(urlUsuario).Result;
            if (responseUsuario.IsSuccessStatusCode)
            {
                ViewBag.Usuario = responseUsuario.Content
                                     .ReadFromJsonAsync<List<UsuarioConsulta>>()
                                     .Result;
            }
            else
            {
                ViewBag.Usuario = new List<UsuarioConsulta>();
            }
                //ESTADO
            ViewBag.EstadoCita = new List<EstadoM>();
            var responseEstadoCita = client.GetAsync(urlEstadoCita).Result;
            if (responseEstadoCita.IsSuccessStatusCode)
            {
                var listaEstados = responseEstadoCita.Content
                                     .ReadFromJsonAsync<List<EstadoM>>()
                                     .Result;
                    //FILTTRO
               
                ViewBag.EstadoCita = listaEstados
                    .Where(e => e.Estado == "Pendiente" || e.Estado == "Confirmada")
                    .ToList();
            }

            // CITAS 
            ViewBag.Cita = new List<CitaConsulta>();
            var responseCita = client.PostAsync(urlCita, null).Result;
            if (responseCita.IsSuccessStatusCode)
            {
                var listaCitas = responseCita.Content
                                            .ReadFromJsonAsync<List<CitaConsulta>>()
                                            .Result;

               
                ViewBag.Cita = listaCitas
                    .Where(c => c.Estado != "Cancelada")
                    .ToList();
            }

            return View();
        }


        // POST REGISTRO Cita
        [HttpPost]
        public IActionResult RegistroCita(Cita model)
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");

            if (string.IsNullOrEmpty(idUsuario))
            {
                TempData["Error"] = "No se encontró el usuario en sesión.";
                return RedirectToAction("Login", "Home");
            }

            if (!int.TryParse(idUsuario, out int usuarioId))
            {
                TempData["Error"] = "Usuario inválido en sesión.";
                return RedirectToAction("Login", "Home");
            }

            model.CreadaPor = usuarioId;
            model.Estado = 3;

            using var client = _http.CreateClient();
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Cita/RegistroCita";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cita registrada correctamente.";
                return RedirectToAction("RegistroCita");
            }
            else
            {
                ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;

                model.Telefono = 0;

                // RECARGAR USUARIO
                var urlUsuario = _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ObtenerUsuario";
                var responseUsuario = client.GetAsync(urlUsuario).Result;

                if (responseUsuario.IsSuccessStatusCode)
                {
                    ViewBag.Usuario = responseUsuario.Content
                        .ReadFromJsonAsync<List<UsuarioConsulta>>()
                        .Result;
                }
                else
                {
                    ViewBag.Usuario = new List<UsuarioConsulta>();
                }

                // RECARGAR ESTADO
                ViewBag.EstadoCita = new List<EstadoM>();
                var urlEstadoCita = _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/2";
                var responseEstadoCita = client.GetAsync(urlEstadoCita).Result;

                if (responseEstadoCita.IsSuccessStatusCode)
                {
                    var listaEstados = responseEstadoCita.Content
                        .ReadFromJsonAsync<List<EstadoM>>()
                        .Result;

                    ViewBag.EstadoCita = listaEstados
                        .Where(e => e.Estado == "Pendiente" || e.Estado == "Confirmada")
                        .ToList();
                }

                // RECARGAR TABLA
                var urlCitas = _config.GetValue<string>("Valores:UrlAPI") + "Cita/ConsultarCita";
                var responseCitas = client.PostAsync(urlCitas, null).Result;

                if (responseCitas.IsSuccessStatusCode)
                {
                    var listaCitas = responseCitas.Content
                        .ReadFromJsonAsync<List<CitaConsulta>>()
                        .Result;

                    ViewBag.Cita = listaCitas
                        .Where(c => c.Estado != "Cancelada")
                        .ToList();
                }
                else
                {
                    ViewBag.Cita = new List<CitaConsulta>();
                }

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult EditarCita(int id)
        {
            using var client = _http.CreateClient();
            List<UsuarioConsulta> listaUsuarios = new List<UsuarioConsulta>();
            // USUARIOS
            var responseUsuario = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ObtenerUsuario").Result;

            if (responseUsuario.IsSuccessStatusCode)
            {
                listaUsuarios = responseUsuario.Content
                    .ReadFromJsonAsync<List<UsuarioConsulta>>().Result;
                ViewBag.Usuario = listaUsuarios;
            }
            else
            {
                ViewBag.Usuario = new List<UsuarioConsulta>();
            }

            // ESTADO
            ViewBag.EstadoCita = new List<EstadoM>();
            var responseEstado = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/2").Result;

            if (responseEstado.IsSuccessStatusCode)
            {
                var listaEstados = responseEstado.Content
                    .ReadFromJsonAsync<List<EstadoM>>().Result;

                // FILTRO
                ViewBag.EstadoCita = listaEstados
                    .Where(e => e.Estado == "Pendiente" || e.Estado == "Confirmada")
                    .ToList();
            }

            // CITA POR ID
            var responseCita = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + $"Cita/ObtenerCitaId/{id}").Result;

            Cita cita = null;
            if (responseCita.IsSuccessStatusCode)
            {
                cita = responseCita.Content
                     .ReadFromJsonAsync<Cita>().Result;
            }
            else
            {
                TempData["Error"] = "No se pudo cargar la cita.";
                return RedirectToAction("RegistroCita");
            }
            var usuarioActual = listaUsuarios
    .FirstOrDefault(u => u.Consecutivo == cita.CreadaPor);

            ViewBag.UsuarioNombre = usuarioActual?.NombreCompleto ?? "";
            return View(cita);
        }

        [HttpPost]
        public IActionResult EditarCita(Cita model)
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");

            if (!string.IsNullOrEmpty(idUsuario))
                model.ModificadoPor = int.Parse(idUsuario);

            using var client = _http.CreateClient();

            var response = client.PutAsJsonAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Cita/EditarCita", model).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cita actualizada correctamente";
                return RedirectToAction("RegistroCita");
            }
            else
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;

                model.Telefono = 0;

                // USUARIOS
                List<UsuarioConsulta> listaUsuarios = new List<UsuarioConsulta>();

                var responseUsuario = client.GetAsync(
                    _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ObtenerUsuario").Result;

                if (responseUsuario.IsSuccessStatusCode)
                {
                    listaUsuarios = responseUsuario.Content
                        .ReadFromJsonAsync<List<UsuarioConsulta>>().Result;
                    ViewBag.Usuario = listaUsuarios;
                }
                else
                {
                    ViewBag.Usuario = new List<UsuarioConsulta>();
                }

                // ESTADOS
                ViewBag.EstadoCita = new List<EstadoM>();
                var responseEstado = client.GetAsync(
                    _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/2").Result;

                if (responseEstado.IsSuccessStatusCode)
                {
                    var listaEstados = responseEstado.Content
                        .ReadFromJsonAsync<List<EstadoM>>().Result;

                    ViewBag.EstadoCita = listaEstados
                        .Where(e => e.Estado == "Pendiente" || e.Estado == "Confirmada")
                        .ToList();
                }

                var usuarioActual = listaUsuarios.FirstOrDefault(u => u.Consecutivo == model.CreadaPor);
                ViewBag.UsuarioNombre = usuarioActual?.NombreCompleto ?? "";

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CancelarCita(int id)
        {
            using var client = _http.CreateClient();

            var response = client.PutAsync(
                _config.GetValue<string>("Valores:UrlAPI") + $"Cita/CancelarCita/{id}",
                null
            ).Result;

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Cita cancelada correctamente";
            else
                TempData["Error"] = "Error al cancelar la cita";

            return RedirectToAction("RegistroCita");
        }
        [HttpGet]
        public IActionResult ObtenerHorasOcupadas(string fecha, int? idCita)
        {
            using var client = _http.CreateClient();

            var url = _config.GetValue<string>("Valores:UrlAPI") +
                      $"Cita/ObtenerHorasOcupadas?fecha={fecha}&idCita={idCita}";

            var response = client.GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
                return Json(new List<string>());

            var horas = response.Content.ReadFromJsonAsync<List<string>>().Result;
            return Json(horas);
        }
    }
}
