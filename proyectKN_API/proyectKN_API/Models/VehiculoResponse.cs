namespace proyectKN_API.Models
{
    public class VehiculoResponse
    {

        public int Consecutivo { get; set; }
        public string Nombre_Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Anio { get; set; } = string.Empty;
        public string Problema { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
