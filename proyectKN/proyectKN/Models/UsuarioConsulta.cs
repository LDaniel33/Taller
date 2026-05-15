namespace proyectKN.Models
{
    public class UsuarioConsulta
    {
        public int Consecutivo { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
