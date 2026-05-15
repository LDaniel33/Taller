namespace proyectKN.Models
{
    public class Egreso
    {
        public int Consecutivo { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public Decimal Monto { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int RegistradoPor { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
    }
}
