namespace proyectKN.Models
{
    public class Usuario
    {
        public int Consecutivo{  get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int NombreRol { get; set; }
        public int Estado { get; set; }
    }
}