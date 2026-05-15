namespace proyectKN_API.Models
{
    public class IngresoResponse
    {
        public int Consecutivo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int Monto { get; set; }
        public Decimal Saldo_Pendiente { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } = string.Empty;


    }
}
