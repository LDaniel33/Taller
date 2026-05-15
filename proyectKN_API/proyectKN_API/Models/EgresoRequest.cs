using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class EgresoRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Motivo { get; set; } = string.Empty;
        [Required]
        public Decimal Monto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public int RegistradoPor { get; set; }
        [Required]
        public string MetodoPago { get; set; } = string.Empty;
    }
}
