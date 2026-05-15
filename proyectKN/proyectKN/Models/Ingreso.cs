namespace proyectKN.Models
{
    public class Ingreso
    {
        public int Consecutivo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public Decimal Monto { get; set; }
        public  Decimal Saldo_Pendiente { get; set; }
        public DateTime Fecha { get; set; }
        public int Estado { get; set; }
    }
}
