namespace proyectKN_API.Models
{
    public class ReporteUsuarioResponse
    {
        public int Consecutivo { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string UsuarioLogin { get; set; } = string.Empty;
        public string NombreRol { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }

    public class ReporteInventarioResponse
    {
        public int Consecutivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string IdArticulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string Proveedor { get; set; } = string.Empty;
    }

    public class ReporteCitasResponse
    {
        public int Consecutivo { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public DateTime FechaCita { get; set; }
        public TimeSpan HoraCita { get; set; }
        public string Servicio { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string CreadaPor { get; set; } = string.Empty;
    }

    public class ReporteVehiculosResponse
    {
        public int Consecutivo { get; set; }
        public string Nombre_Cliente { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Placa { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }
        public string Problema { get; set; } = string.Empty;
        public string Revision { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

    public class ReporteIngresosResponse
    {
        public int Consecutivo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal Saldo_Pendiente { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }

    public class ReporteEgresosResponse
    {
        public int Consecutivo { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int Cantidad { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string RegistradoPor { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
    
        public class ReporteProveedorResponse
        {
            public int Consecutivo { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
        }
    }
