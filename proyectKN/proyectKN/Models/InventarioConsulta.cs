namespace proyectKN.Models
{
    public class InventarioConsulta
    
        {
            public int Consecutivo { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string IdArticulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
            public Decimal PrecioCompra { get; set; } 
            public Decimal PrecioVenta { get; set; }
            public int Stock { get; set; }
            public int StockMinimo { get; set; }
            public string Proveedor { get; set; } = string.Empty;


    }
    }

