using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class IniciarSesionRequest
    {
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
