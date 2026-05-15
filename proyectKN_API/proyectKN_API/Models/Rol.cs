using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class Rol
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string NombreRol { get; set; } = string.Empty;
        [Required]
        public string Descripcion { get; set; } = string.Empty;
    }
}