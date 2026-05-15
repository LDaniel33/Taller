using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class LoginRequest
    {
        [Required]
        public string Correo { get; set; } = string.Empty;
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}