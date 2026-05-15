namespace proyectKN.Models
{
    public class CitaConsulta
    {
        public int Consecutivo { get; set; }

        public string NombreCliente { get; set; } = string.Empty;

        public string Cedula { get; set; } = string.Empty;

        public DateTime FechaCita { get; set; }

        public TimeSpan HoraCita { get; set; }

        public int Telefono { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Servicio { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
        public string CreadaPor { get; set; } = string.Empty;
        public string ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    } 
}
