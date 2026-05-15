using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class UsuarioRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string NombreCompleto { get; set; } = string.Empty;
        [Required]
        public string Cedula { get; set; } = string.Empty;
        [Required]
        public string Correo { get; set; } = string.Empty;
        [Required]
        public string UsuarioLogin { get; set; } = string.Empty;
        public string? Contrasenna { get; set; }
        public DateTime FechaRegistro { get; set; }
        [Required]
        public int NombreRol { get; set; }
        [Required]
        public int Estado { get; set; }
       
       
    }
}
