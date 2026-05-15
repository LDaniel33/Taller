using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class IngresoRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public Decimal Monto { get; set; } 
        [Required]
        public  Decimal Saldo_Pendiente { get; set; } 
        public DateTime Fecha { get; set; } 
        [Required]
        public int Estado { get; set; }
    }
}
