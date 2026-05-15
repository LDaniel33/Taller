using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class CitaRequest
        {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string NombreCliente { get; set; } = string.Empty;
        [Required]
        public string Cedula { get; set; } = string.Empty;
        [Required]
        public DateTime FechaCita { get; set; }
        [Required]
        public TimeSpan HoraCita { get; set; }
        [Required]
        public int Telefono { get; set; }
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Servicio { get; set; } = string.Empty;
        [Required]
        public int Estado { get; set; }
        [Required]
        public int CreadaPor { get; set; }
        public int? ModificadoPor { get; set;}
        public DateTime? FechaModificacion { get; set;}
        }
    }

