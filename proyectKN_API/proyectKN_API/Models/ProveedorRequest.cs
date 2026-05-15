using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class ProveedorRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public int Telefono { get; set; } 
        [Required]
        public string Correo { get; set; } = string.Empty;
        [Required]
        public string Direccion { get; set; } = string.Empty;
    }
}
