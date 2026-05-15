namespace proyectKN_API.Services
{
    public interface IPasswordHelper
    {
        string Encrypt(string texto);
        void EnviarCorreo(string destino, string asunto, string contenido);
    }
}
