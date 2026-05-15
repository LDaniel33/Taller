using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class EstadoM
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Estado { get; set; } = string.Empty;
    }
}
