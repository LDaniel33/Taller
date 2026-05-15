namespace proyectKN.Models
{
    public class Vehiculo
    {
        public int Consecutivo { get; set; }
        public string Nombre_Cliente { get; set; } = string.Empty;
        public int Telefono { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Problema { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public int Estado { get; set; }
    }
}
