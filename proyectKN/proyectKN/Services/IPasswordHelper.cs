namespace proyectKN.Services
{
    public interface IPasswordHelper
    {
        string Encrypt(string texto);
        string Decrypt(string texto);
    }
}