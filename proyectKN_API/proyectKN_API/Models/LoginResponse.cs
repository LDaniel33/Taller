namespace proyectKN_API.Models
{
    public class LoginResponse
    {
        public int Consecutivo { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool EsValido { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}