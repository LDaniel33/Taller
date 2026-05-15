using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using proyectKN.Filters;
using proyectKN.Models;
using proyectKN.Services;
using System.Net;

namespace proyectKN.Controllers
{
    [SesionActiva]
  
    public class UsuarioController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;
        private readonly IPasswordHelper _password;
        public UsuarioController(IHttpClientFactory http, IConfiguration config, IPasswordHelper password)
        {
            _http = http;
            _config = config;
            _password = password;
        }


        // GET REGISTRO USUARIO
        [HttpGet]

        public IActionResult RegistroUsuario()
        {

            using var client = _http.CreateClient();

            var urlRoles = _config.GetValue<string>("Valores:UrlAPI") + "Rol/ObtenerRoles";
            var urlEstados = _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/1";
            var urlUsuarios = _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ConsultarUsuarios";

            // ROL
            var responseRoles = client.PostAsync(urlRoles, null).Result;

            if (responseRoles.IsSuccessStatusCode)
            {
                ViewBag.Roles = responseRoles.Content
                                             .ReadFromJsonAsync<List<Rol>>()
                                             .Result;
            }
            else
            {
                ViewBag.Roles = new List<Rol>();
            }

            // ESTADO
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

            //USUARIOS

            var responseUsuarios = client.PostAsync(urlUsuarios, null).Result;
            if (responseUsuarios.IsSuccessStatusCode)
            {
                ViewBag.Usuarios = responseUsuarios.Content
                                                    .ReadFromJsonAsync<List<UsuarioConsulta>>()
                                                    .Result;
            }
            else
            {
                ViewBag.Usuarios = new List<UsuarioConsulta>();
            }
            return View();
        }




        // POST REGISTRO USUARIO
        [HttpPost]
        public IActionResult RegistroUsuario(Usuario model)
        {
            using var client = _http.CreateClient();
            model.Estado = 1;
            var url = _config.GetValue<string>("Valores:UrlAPI") + "Usuario/RegistroUsuario";
            var result = client.PostAsJsonAsync(url, model).Result;

            if (result.IsSuccessStatusCode)
            {
                TempData["Success"] = "Usuario registrado correctamente.";
                return RedirectToAction("RegistroUsuario");
            }
            else
            {
                ViewBag.Mensaje = result.Content.ReadAsStringAsync().Result;

                model.Contrasenna = null;

                // RECARGAR TABLA
                var urlUsuarios = _config.GetValue<string>("Valores:UrlAPI") + "Usuario/ConsultarUsuarios";
                var responseUsuarios = client.PostAsync(urlUsuarios, null).Result;

                if (responseUsuarios.IsSuccessStatusCode)
                {
                    ViewBag.Usuarios = responseUsuarios.Content
                        .ReadFromJsonAsync<List<UsuarioConsulta>>()
                        .Result;
                }
                else
                {
                    ViewBag.Usuarios = new List<UsuarioConsulta>();
                }

                // RECARGAR ROLES
                var urlRoles = _config.GetValue<string>("Valores:UrlAPI") + "Rol/ObtenerRoles";
                var responseRoles = client.PostAsync(urlRoles, null).Result;

                if (responseRoles.IsSuccessStatusCode)
                {
                    ViewBag.Roles = responseRoles.Content
                        .ReadFromJsonAsync<List<Rol>>()
                        .Result;
                }
                else
                {
                    ViewBag.Roles = new List<Rol>();
                }

                return View(model);
            }
        }

        //GET EDITAR 

        [HttpGet]
        public IActionResult EditarUsuario(int id)
        {
            using var client = _http.CreateClient();

            // ROLES 
            var responseRoles = client.PostAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Rol/ObtenerRoles", null
            ).Result;

            if (responseRoles.IsSuccessStatusCode)
            {
                ViewBag.Roles = responseRoles.Content
                                             .ReadFromJsonAsync<List<Rol>>()
                                             .Result;
            }
            else
            {
                ViewBag.Roles = new List<Rol>();
            }
            // ESTADO
            var responseEstados = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/1"
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

            // USUARIO
            var responseUsuario = client.GetAsync(
                _config.GetValue<string>("Valores:UrlAPI") + $"Usuario/ObtenerUsuarioId/{id}"
            ).Result;

            Usuario usuario = null;

            if (responseUsuario.IsSuccessStatusCode)
            {
                usuario = responseUsuario.Content
                                         .ReadFromJsonAsync<Usuario>()
                                         .Result;
            }
            else
            {
                TempData["Error"] = "No se pudo cargar el usuario.";
                return RedirectToAction("RegistroUsuario");
            }

            return View(usuario);
        }
        [HttpPost]
        public IActionResult EditarUsuario(Usuario model)
        {
            using var client = _http.CreateClient();

            var response = client.PostAsJsonAsync(_config.GetValue<string>("Valores:UrlAPI") + "Usuario/EditarUsuario", model).Result;

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Usuario actualizado correctamente";
                return RedirectToAction("RegistroUsuario");
            }
            else
            {
                ViewBag.Mensaje = response.Content.ReadAsStringAsync().Result;
                model.Contrasenna = null;

                var responseRoles = client.PostAsync(
                    _config.GetValue<string>("Valores:UrlAPI") + "Rol/ObtenerRoles", null).Result;

                if (responseRoles.IsSuccessStatusCode)
                    ViewBag.Roles = responseRoles.Content.ReadFromJsonAsync<List<Rol>>().Result;
                else
                    ViewBag.Roles = new List<Rol>();

                var responseEstados = client.GetAsync(
                    _config.GetValue<string>("Valores:UrlAPI") + "Estado/ObtenerEstado/1").Result;

                if (responseEstados.IsSuccessStatusCode)
                    ViewBag.Estado = responseEstados.Content.ReadFromJsonAsync<List<EstadoM>>().Result;
                else
                    ViewBag.Estado = new List<EstadoM>();

                return View(model);
            }
        }
    }
}

