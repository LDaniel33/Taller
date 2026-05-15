
using Microsoft.AspNetCore.Mvc;
using proyectKN.Filters;

namespace proyectKN.Controllers
{
    [SesionActiva]
        public class ServicioController : Controller
        {
            [HttpGet]
            public IActionResult Consulta()
            {
                return View();
            }
        }
    }


