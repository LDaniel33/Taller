namespace proyectKN.Models
{
    public class Reporte
    {

        public DateTime FechaDesde { get; set; } = DateTime.Today.AddDays(-30);
        public DateTime FechaHasta { get; set; } = DateTime.Today;

        public int Consecutivo { get; set; }
        public string Columna1 { get; set; } = string.Empty;
        public string Columna2 { get; set; } = string.Empty;
        public string Columna3 { get; set; } = string.Empty;
        public string Columna4 { get; set; } = string.Empty;
        public string Columna5 { get; set; } = string.Empty;
        public string Columna6 { get; set; } = string.Empty;
        public string Columna7 { get; set; } = string.Empty;
        public string Columna8 { get; set; } = string.Empty;
    }
}