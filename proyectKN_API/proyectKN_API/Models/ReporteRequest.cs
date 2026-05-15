namespace proyectKN_API.Models
{
    public class ReporteRequest
    {
        public string TipoReporte { get; set; } = string.Empty;
        public DateTime Desde { get; set; }
        public DateTime Hasta { get; set; }
    }

    public class ReporteResponse
    {
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string ClienteVehiculo { get; set; } = string.Empty;
    }
}