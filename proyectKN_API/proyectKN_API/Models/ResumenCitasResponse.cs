namespace proyectKN_API.Models
{
    public class ResumenCitasResponse
    {
        public int Confirmadas { get; set; }
        public int Finalizadas { get; set; }
        public int Pendientes { get; set; }
        public int Canceladas { get; set; }
    }
}