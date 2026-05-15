using System.ComponentModel.DataAnnotations;

namespace proyectKN_API.Models
{
    public class InventarioRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string IdArticulo { get; set; } = string.Empty;
        [Required]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public Decimal PrecioCompra { get; set; }
        [Required]
        public Decimal PrecioVenta {  get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public int StockMinimo { get; set; }
        [Required]
        public int Proveedor { get; set; }


    }
}
