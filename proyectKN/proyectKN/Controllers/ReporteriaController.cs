using Microsoft.AspNetCore.Mvc;
using proyectKN.Filters;

namespace proyectKN.Controllers
{
    [SesionActiva]

    public class ReporteriaController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _config;

        public ReporteriaController(IHttpClientFactory http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        [HttpGet]
        public IActionResult Reporteria()
        {
            return View(new proyectKN.Models.Reporte());
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerReporte(string tipo, string? desde, string? hasta, string? estado = null)
        {
            using var client = _http.CreateClient();

            var baseUrl = _config.GetValue<string>("Valores:UrlAPI");
            var url = $"{baseUrl}Reporte/GenerarReporte?tipo={Uri.EscapeDataString(tipo)}";

            if (!string.IsNullOrWhiteSpace(desde))
                url += $"&desde={Uri.EscapeDataString(desde)}";

            if (!string.IsNullOrWhiteSpace(hasta))
                url += $"&hasta={Uri.EscapeDataString(hasta)}";

            if (!string.IsNullOrWhiteSpace(estado))
                url += $"&estado={Uri.EscapeDataString(estado)}";

            var response = await client.GetAsync(url);
            var contenido = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, contenido);

            return Content(contenido, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerResumenCitas(string? desde, string? hasta)
        {
            using var client = _http.CreateClient();

            var baseUrl = _config.GetValue<string>("Valores:UrlAPI");
            var url = $"{baseUrl}Reporte/ResumenCitas";

            if (!string.IsNullOrWhiteSpace(desde) || !string.IsNullOrWhiteSpace(hasta))
            {
                url += "?";

                if (!string.IsNullOrWhiteSpace(desde))
                    url += $"desde={Uri.EscapeDataString(desde)}";

                if (!string.IsNullOrWhiteSpace(hasta))
                {
                    if (!string.IsNullOrWhiteSpace(desde))
                        url += "&";

                    url += $"hasta={Uri.EscapeDataString(hasta)}";
                }
            }

            var response = await client.GetAsync(url);
            var contenido = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, contenido);

            return Content(contenido, "application/json");
        }
    }
}