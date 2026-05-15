

namespace proyectKN_API.Models
{
    public class EgresoResponse
    {
        public int Consecutivo { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public Decimal Monto { get; set; }
        public int Cantidad { get; set; } 
        public DateTime Fecha { get; set; } 
        public string RegistradoPor { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;

    }
}
