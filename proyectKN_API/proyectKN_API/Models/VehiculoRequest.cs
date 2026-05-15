using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class VehiculoRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Nombre_Cliente { get; set; } = string.Empty;
        [Required]
        public int Telefono { get; set; }
        [Required]
        public string Cedula { get; set; } = string.Empty;
        [Required]
        public string Placa { get; set; } = string.Empty;
        [Required]
        public string Marca { get; set; } = string.Empty;
        [Required]
        public string Modelo { get; set; } = string.Empty;
        [Required]
        public int Anio { get; set; }
        [Required]
        public string Problema { get; set; } = string.Empty;
        [Required]
        public string Revision { get; set; } = string.Empty;
        [Required]
        public int Estado { get; set; }

    }
}
