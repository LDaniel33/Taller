DROP DATABASE TallerLaUnion

CREATE DATABASE TallerLaUnion
GO

USE TallerLaUnion
GO
drop table  Estado
/*
 ROLES Y USUARIOS*/

 CREATE TABLE TipoEstado(
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50)
);

CREATE TABLE Estado(
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Estado VARCHAR(50),
    IdTipoEstado INT NOT NULL,
    FOREIGN KEY (IdTipoEstado) REFERENCES TipoEstado(Consecutivo)
);


CREATE TABLE Roles (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200)
);


drop table Usuarios
CREATE TABLE Usuarios (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(120) NOT NULL,
    Cedula VARCHAR(50),
    Correo VARCHAR(120) UNIQUE NOT NULL,
    UsuarioLogin VARCHAR(50) UNIQUE NOT NULL,
    Contrasenna VARCHAR(250) NOT NULL,
    Estado INT,
    FechaRegistro DATETIME DEFAULT GETDATE(),
   NombreRol INT NOT NULL,
    FOREIGN KEY (NombreRol) REFERENCES Roles(Consecutivo),
  FOREIGN KEY (Estado) REFERENCES Estado(Consecutivo)
);


 drop table Vehiculos
CREATE TABLE Vehiculos (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Cliente VARCHAR(100),
    Telefono INT,
    Cedula VARCHAR(100),
    Placa VARCHAR(20) UNIQUE NOT NULL,
    Marca VARCHAR(50),
    Modelo VARCHAR(50),
    Anio INT,
    Problema VARCHAR(500),
    Revision VARCHAR(500),
    Estado INT,
    FOREIGN KEY (Estado) REFERENCES Estado(Consecutivo)
);
 

/*
= CITAS*/
drop table Citas
CREATE TABLE Citas (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    NombreCliente VARCHAR(100),
    Cedula VARCHAR(100),
    FechaCita DATE NOT NULL,
    HoraCita TIME NOT NULL,
    Telefono INT,
    Email VARCHAR(100),
    Servicio VARCHAR(200),
    Estado INT,
    CreadaPor INT NOT NULL,

    FOREIGN KEY (CreadaPor) REFERENCES Usuarios(Consecutivo),
   FOREIGN KEY (Estado) REFERENCES Estado(Consecutivo)
);
ALTER TABLE Citas
ADD ModificadoPor INT NULL,
    FechaModificacion DATETIME NULL;




/*INVENTARIO*/
drop table Proveedores
CREATE TABLE Proveedores (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Correo VARCHAR(120),
    Direccion VARCHAR(200)
);

CREATE TABLE Productos (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(120) NOT NULL,
    IdArticulo Varchar (50) NOT NULL,
    Descripcion VARCHAR(200),
    PrecioCompra DECIMAL(10,2) NOT NULL,
    PrecioVenta DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    StockMinimo INT DEFAULT 5,
    Proveedor INT,
    FOREIGN KEY (Proveedor) REFERENCES Proveedores(Consecutivo)
);



/* CONTABILIDAD*/
drop table Ingresos
CREATE TABLE Ingresos (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion VARCHAR(200),
    Monto DECIMAL(10,2) NOT NULL,
    Saldo_Pendiente DECIMAL(10,2),
    Estado INT,
    Fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (Estado) REFERENCES Estado(Consecutivo)
);


drop table Egresos
CREATE TABLE Egresos (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    Motivo VARCHAR(200),
    Monto DECIMAL(10,2) NOT NULL,
    Cantidad INT,
    Fecha DATETIME DEFAULT GETDATE(),
    RegistradoPor INT NOT NULL,
    MetodoPago VARCHAR(50),
    FOREIGN KEY (RegistradoPor) REFERENCES Usuarios(Consecutivo),
);

/*REPORTERÍA*/
drop table Reporteria
CREATE TABLE Reporteria (
    Consecutivo INT IDENTITY(1,1) PRIMARY KEY,
    TipoReporte VARCHAR(50) NOT NULL,
    FechaGeneracion DATETIME DEFAULT GETDATE(),
    IdUsuario INT NOT NULL,
    IdIngreso INT NULL,
    IdEgreso INT NULL,
    IdCita INT NULL,
    IdVehiculo INT NULL,
    --IdOrden INT NULL,
    IdProducto INT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(Consecutivo),
    FOREIGN KEY (IdIngreso) REFERENCES Ingresos(Consecutivo),
    FOREIGN KEY (IdEgreso) REFERENCES Egresos(Consecutivo),
    FOREIGN KEY (IdCita) REFERENCES Citas(Consecutivo),
    FOREIGN KEY (IdVehiculo) REFERENCES Vehiculos(Consecutivo),
    --FOREIGN KEY (IdOrden) REFERENCES OrdenesTrabajo(Consecutivo),
    FOREIGN KEY (IdProducto) REFERENCES Productos(Consecutivo)
);
drop PROCEDURE  sp_RegistroUsuario


/*  PROCEDIMIENTOS ALMACENADOS*/

/*USUARIOS*/
drop procedure sp_RegistroUsuario
CREATE PROCEDURE sp_RegistroUsuario
    @NombreCompleto VARCHAR(100),
    @Cedula VARCHAR(20),
    @Correo VARCHAR(100),
    @UsuarioLogin VARCHAR(50),
    @Contrasenna VARCHAR(200),
    @Estado INT,
    @NombreRol INT
AS
BEGIN

    -- VALIDAR CEDULA
    IF EXISTS (SELECT 1 FROM Usuarios WHERE Cedula = @Cedula)
    BEGIN
        SELECT -1 AS Resultado
        RETURN
    END

    -- VALIDAR USUARIO
    IF EXISTS (SELECT 1 FROM Usuarios WHERE UsuarioLogin = @UsuarioLogin)
    BEGIN
        SELECT -2 AS Resultado
        RETURN
    END

    -- INSERT
    INSERT INTO Usuarios
    (
        NombreCompleto,
        Cedula,
        Correo,
        UsuarioLogin,
        Contrasenna,
        Estado,
        NombreRol
    )
    VALUES
    (
        @NombreCompleto,
        @Cedula,
        @Correo,
        @UsuarioLogin,
        @Contrasenna,
        @Estado,
        @NombreRol
    )

    SELECT 1 AS Resultado

END

drop procedure [dbo].[sp_ConsultarUsuarios]
CREATE PROCEDURE sp_ConsultarUsuarios
AS
BEGIN
    SELECT 
        U.Consecutivo,
        U.NombreCompleto,
        U.Cedula,
        U.Correo,
        U.UsuarioLogin,
        U.FechaRegistro,
        R.NombreRol,
        E.Estado AS Estado
    FROM Usuarios U
    INNER JOIN Roles R ON U.NombreRol = R.Consecutivo
    INNER JOIN Estado E ON U.Estado = E.Consecutivo
    WHERE U.Estado = 1
END



drop procedure [dbo].[sp_ObtenerRoles]
CREATE PROCEDURE [dbo].[sp_ObtenerRoles]
AS
BEGIN

    SELECT 
    Consecutivo, 
      NombreRol,
        Descripcion
    FROM Roles

END
GO


DROP PROCEDURE [dbo].[sp_ObtenerEstado]
CREATE PROCEDURE sp_ObtenerEstado
@IdTipoEstado INT
AS
BEGIN
    SELECT 
        Consecutivo,
        Estado
    FROM Estado
    WHERE IdTipoEstado = @IdTipoEstado
END
GO

CREATE PROCEDURE [dbo].[sp_ObtenerId]
    @Consecutivo INT
AS
BEGIN
    SELECT 
        U.Consecutivo,
        U.NombreCompleto,
        U.Cedula,
        U.Correo,
        U.UsuarioLogin,
        U.Contrasenna,
        U.Estado,
        U.NombreRol
    FROM Usuarios U
    WHERE U.Consecutivo = @Consecutivo
END

EXEC sp_ObtenerId 1
DROP PROCEDURE  [dbo].[sp_EditarUsuario]
CREATE PROCEDURE [dbo].[sp_EditarUsuario]
    @Consecutivo INT,
    @NombreCompleto VARCHAR(120),
    @Cedula VARCHAR(20),
    @Correo VARCHAR(120),
    @UsuarioLogin VARCHAR(50),
    @Estado INT,
    @NombreRol INT,
    @Contrasenna VARCHAR(100) = NULL
AS
BEGIN

UPDATE Usuarios
SET 
    NombreCompleto = @NombreCompleto,
    Cedula = @Cedula,
    Correo = @Correo,
    UsuarioLogin = @UsuarioLogin,
    Estado = @Estado,
    NombreRol = @NombreRol,

    Contrasenna = CASE 
                    WHEN @Contrasenna IS NULL OR @Contrasenna = ''
                    THEN Contrasenna
                    ELSE @Contrasenna
                  END

WHERE Consecutivo = @Consecutivo

END


/*PROVEEDOR*/
drop procedure  sp_RegistroProveedor
CREATE PROCEDURE sp_RegistroProveedor
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(20),
    @Correo VARCHAR(100),
    @Direccion VARCHAR(MAX)
AS
BEGIN

    INSERT INTO Proveedores
    (
        Nombre,
        Telefono,
        Correo,
       Direccion
    )
    VALUES
    (
    @Nombre,
    @Telefono,
    @Correo,
    @Direccion
    )

END


CREATE PROCEDURE sp_ConsultarProveedor
AS
BEGIN
  SELECT
    Consecutivo,
    Nombre,
    Telefono,
    Correo,
    Direccion
FROM Proveedores
END


drop procedure  sp_EditarProveedor
CREATE PROCEDURE sp_EditarProveedor
    @Consecutivo INT,
    @Nombre VARCHAR(100),
    @Telefono VARCHAR(20),
    @Correo VARCHAR(100),
    @Direccion VARCHAR(MAX)
AS
BEGIN

    UPDATE Proveedores
    SET
        Nombre = @Nombre,
        Telefono = @Telefono,
        Correo = @Correo,
       Direccion = @Direccion
    WHERE Consecutivo = @Consecutivo
   
END

drop procedure sp_ObtenerProveedores
CREATE PROCEDURE sp_ObtenerProveedores
AS
BEGIN

    SELECT 
    Consecutivo, 
      Nombre,
      Telefono,
      Correo,
      Direccion
    FROM Proveedores

END
GO
/*INVENTARIO*/
DROP PROCEDURE sp_RegistroInventario
CREATE PROCEDURE sp_RegistroInventario
@Nombre VARCHAR(120),
@IdArticulo VARCHAR(50),
@Descripcion VARCHAR(200),
@PrecioCompra DECIMAL(10,2),
@PrecioVenta DECIMAL(10,2),
@Stock INT,
@StockMinimo INT,
@Proveedor INT
AS
BEGIN

INSERT INTO Productos
(
Nombre,
IdArticulo,
Descripcion,
PrecioCompra,
PrecioVenta,
Stock,
StockMinimo,
Proveedor
)
VALUES
(
@Nombre,
@IdArticulo,
@Descripcion,
@PrecioCompra,
@PrecioVenta,
@Stock,
@StockMinimo,
@Proveedor
)

END

DROP PROCEDURE sp_ConsultaInventario
CREATE PROCEDURE sp_ConsultaInventario
AS
BEGIN

SELECT
p.Consecutivo,
p.Nombre,
p.IdArticulo,
p.Descripcion,
p.PrecioCompra,
p.PrecioVenta,
p.Stock,
p.StockMinimo,
pr.Nombre AS Proveedor
FROM Productos p
INNER JOIN Proveedores pr
ON p.Proveedor = pr.Consecutivo

END





CREATE PROCEDURE sp_ObtenerProveedorId
@Consecutivo INT
AS
BEGIN

SELECT 
        P.Consecutivo,
        P.Nombre,
        P.Telefono,
        P.Correo,
        P.Direccion
    FROM Proveedores P
    WHERE P.Consecutivo = @Consecutivo
END

CREATE PROCEDURE sp_EditarInventario
    @Consecutivo INT,
    @Nombre VARCHAR(120),
    @IdArticulo VARCHAR(20),
    @Descripcion VARCHAR(50),
    @PrecioCompra INT,
    @PrecioVenta INT, 
    @Stock INT,
    @StockMinimo INT, 
    @Proveedor INT
AS
BEGIN

UPDATE Productos
SET 
    Nombre = @Nombre,
    IdArticulo = @IdArticulo,
    Descripcion = @Descripcion,
    PrecioCompra = @PrecioCompra,
    PrecioVenta  = @PrecioVenta ,
    Stock  = @Stock,
    StockMinimo = @StockMinimo,
    Proveedor = @Proveedor

WHERE Consecutivo = @Consecutivo

END

CREATE PROCEDURE sp_ObtenerInventarioId
@Consecutivo INT
AS
BEGIN

SELECT
Consecutivo,
Nombre,
IdArticulo,
Descripcion,
PrecioCompra,
PrecioVenta,
Stock,
StockMinimo,
Proveedor
FROM Productos
WHERE Consecutivo = @Consecutivo

END


/*CITA*/
drop procedure sp_RegistroCita
CREATE PROCEDURE sp_RegistroCita
@NombreCliente VARCHAR(100),
@Cedula VARCHAR(100),
@FechaCita DATE,
@HoraCita TIME,
@Telefono INT,
@Email VARCHAR(100),
@Servicio VARCHAR(200),
@Estado INT,
@CreadaPor INT
AS
BEGIN

INSERT INTO Citas
(
NombreCliente,
Cedula,
FechaCita,
HoraCita,
Telefono,
Email,
Servicio,
Estado,
CreadaPor
)

VALUES
(
@NombreCliente,
@Cedula,
@FechaCita,
@HoraCita,
@Telefono,
@Email,
@Servicio,
@Estado,
@CreadaPor
)

END
drop procedure sp_ConsultaCita

CREATE PROCEDURE sp_ConsultaCita
AS
BEGIN
    SELECT
        c.Consecutivo,
        c.NombreCliente,
        c.Cedula,
        c.FechaCita,
        c.HoraCita,
        c.Telefono,
        c.Email,
        c.Servicio,
        e.Estado,
        u.NombreCompleto AS CreadaPor,
        um.NombreCompleto AS ModificadoPor,
        c.FechaModificacion
    FROM Citas c
    INNER JOIN Usuarios u ON c.CreadaPor = u.Consecutivo
    LEFT JOIN Usuarios um ON c.ModificadoPor = um.Consecutivo
    INNER JOIN Estado e ON c.Estado = e.Consecutivo
    WHERE c.Estado NOT IN (5, 6)
END
GO

drop procedure sp_ObtenerCitaId
CREATE PROCEDURE sp_ObtenerCitaId
@Consecutivo INT
AS
BEGIN

SELECT
Consecutivo,
NombreCliente AS NombreCliente,
Cedula,
FechaCita,
HoraCita,
Telefono,
Email,
Servicio,
Estado,
CreadaPor

FROM Citas
WHERE Consecutivo = @Consecutivo

END
use TallerLaUnion
drop procedure sp_EditarCita

CREATE PROCEDURE sp_EditarCita
    @Consecutivo INT,
    @NombreCliente VARCHAR(100),
    @Cedula VARCHAR(100),
    @FechaCita DATE,
    @HoraCita TIME,
    @Telefono INT,
    @Email VARCHAR(100),
    @Servicio VARCHAR(200),
    @Estado INT,
    @CreadaPor INT,
    @ModificadoPor INT
AS
BEGIN
    UPDATE Citas
    SET
        NombreCliente = @NombreCliente,
        Cedula = @Cedula,
        FechaCita = @FechaCita,
        HoraCita = @HoraCita,
        Telefono = @Telefono,
        Email = @Email,
        Servicio = @Servicio,
        Estado = @Estado,
        CreadaPor = @CreadaPor,
        ModificadoPor = @ModificadoPor,
        FechaModificacion = GETDATE()
    WHERE Consecutivo = @Consecutivo
END
GO


CREATE PROCEDURE sp_ObtenerUsuarios
AS
BEGIN

SELECT
Consecutivo,
NombreCompleto

FROM Usuarios

END
CREATE PROCEDURE sp_CancelarCita
    @Consecutivo INT,
    @Estado INT
AS
BEGIN
    UPDATE Citas
    SET Estado = @Estado
    WHERE Consecutivo = @Consecutivo
END
GO

-- TIPOS
INSERT INTO TipoEstado (Nombre) VALUES 
('Usuario'),
('Cita'),
('Vehiculo'),
('Financiero');

-- ESTADOS

delete from estado 
INSERT INTO Estado (Estado, IdTipoEstado) VALUES
('Activo',1),('Inactivo',1),
('Pendiente',2),('Confirmada',2),('Cancelada',2),('Finalizada',2),
('Ingresado',3),('Revisado',3),('Reparando',3),
('Pendiente',4),('Pagado',4);

INSERT INTO Roles(NombreRol, Descripcion) VALUES
('Encargado', 'Usuario responsable de supervisar operaciones'),
('Administrador', 'Usuario con control total del sistema'),
('Mecanico', 'Usuario encargado de realizar reparaciones y mantenimiento');


select * from Proveedores

select * from Roles
select * from Estado
select * from Usuarios
select * from Citas


  /*Reportes*/
  drop PROCEDURE sp_ReporteUsuarios
CREATE PROCEDURE sp_ReporteUsuarios
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Consecutivo,
        U.NombreCompleto,
        U.Cedula,
        U.Correo,
        U.UsuarioLogin,
        R.NombreRol,
        E.Estado,
        U.FechaRegistro
    FROM Usuarios U
    INNER JOIN Roles R ON U.NombreRol = R.Consecutivo
    INNER JOIN Estado E ON U.Estado = E.Consecutivo
    WHERE
        (@FechaDesde IS NULL OR CAST(U.FechaRegistro AS DATE) >= @FechaDesde)
        AND (@FechaHasta IS NULL OR CAST(U.FechaRegistro AS DATE) <= @FechaHasta)
    ORDER BY U.FechaRegistro DESC;
END
GO

DROP PROCEDURE IF EXISTS sp_ReporteCitas
GO

CREATE PROCEDURE sp_ReporteCitas
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL,
    @Estado VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        C.Consecutivo,
        C.NombreCliente,
        C.Cedula,
        C.FechaCita,
        C.HoraCita,
        C.Servicio,
        E.Estado,
        U.NombreCompleto AS CreadaPor
    FROM Citas C
    INNER JOIN Usuarios U ON C.CreadaPor = U.Consecutivo
    INNER JOIN Estado E ON C.Estado = E.Consecutivo
    WHERE
        (@FechaDesde IS NULL OR C.FechaCita >= @FechaDesde)
        AND (@FechaHasta IS NULL OR C.FechaCita <= @FechaHasta)
        AND (
            @Estado IS NULL
            OR @Estado = ''
            OR E.Estado = @Estado
        )
    ORDER BY C.FechaCita DESC, C.HoraCita DESC;
END
GO

EXEC sp_ReporteCitas NULL, NULL, NULL
EXEC sp_ResumenCitas NULL, NULL

DROP PROCEDURE IF EXISTS sp_ReporteIngresoVehiculos
GO

CREATE PROCEDURE sp_ReporteIngresoVehiculos
    @FechaDesde DATE,
    @FechaHasta DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        V.Consecutivo,
        V.Nombre_Cliente,
        V.Cedula,
        V.Placa,
        V.Marca,
        V.Modelo,
        V.Anio,
        V.Problema,
        V.Revision,
        E.Estado
    FROM Vehiculos V
    INNER JOIN Estado E ON V.Estado = E.Consecutivo
    ORDER BY V.Consecutivo DESC;
END
GO

DROP PROCEDURE IF EXISTS sp_ReporteIngresos
GO

CREATE PROCEDURE sp_ReporteIngresos
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        I.Consecutivo,
        I.Descripcion,
        I.Monto,
        I.Saldo_Pendiente,
        E.Estado,
        I.Fecha
    FROM Ingresos I
    INNER JOIN Estado E ON I.Estado = E.Consecutivo
    WHERE
        (@FechaDesde IS NULL OR CAST(I.Fecha AS DATE) >= @FechaDesde)
        AND (@FechaHasta IS NULL OR CAST(I.Fecha AS DATE) <= @FechaHasta)
    ORDER BY I.Fecha DESC;
END
GO

DROP PROCEDURE IF EXISTS sp_ReporteEgresos
GO

CREATE PROCEDURE sp_ReporteEgresos
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Egr.Consecutivo,
        Egr.Motivo,
        Egr.Monto,
        Egr.Cantidad,
        Egr.MetodoPago,
        U.NombreCompleto AS RegistradoPor,
        Egr.Fecha
    FROM Egresos Egr
    INNER JOIN Usuarios U ON Egr.RegistradoPor = U.Consecutivo
    WHERE
        (@FechaDesde IS NULL OR CAST(Egr.Fecha AS DATE) >= @FechaDesde)
        AND (@FechaHasta IS NULL OR CAST(Egr.Fecha AS DATE) <= @FechaHasta)
    ORDER BY Egr.Fecha DESC;
END
GO


DROP PROCEDURE IF EXISTS sp_ReporteInventario
GO 
CREATE PROCEDURE sp_ReporteInventario
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        P.Consecutivo,
        P.Nombre,
        P.IdArticulo,
        P.Descripcion,
        P.PrecioCompra,
        P.PrecioVenta,
        P.Stock,
        P.StockMinimo,
        PR.Nombre AS Proveedor
    FROM Productos P
    INNER JOIN Proveedores PR ON P.Proveedor = PR.Consecutivo
    ORDER BY P.Nombre;
END
GO

DROP PROCEDURE IF EXISTS sp_ResumenCitas
GO

CREATE PROCEDURE sp_ResumenCitas
    @FechaDesde DATE = NULL,
    @FechaHasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        SUM(CASE WHEN E.Estado = 'Confirmada' THEN 1 ELSE 0 END) AS Confirmadas,
        SUM(CASE WHEN E.Estado = 'Finalizada' THEN 1 ELSE 0 END) AS Finalizadas,
        SUM(CASE WHEN E.Estado = 'Pendiente' THEN 1 ELSE 0 END) AS Pendientes,
        SUM(CASE WHEN E.Estado = 'Cancelada' THEN 1 ELSE 0 END) AS Canceladas
    FROM Citas C
    INNER JOIN Estado E ON C.Estado = E.Consecutivo
    WHERE
        (@FechaDesde IS NULL OR C.FechaCita >= @FechaDesde)
        AND (@FechaHasta IS NULL OR C.FechaCita <= @FechaHasta);
END
GO

IF OBJECT_ID('sp_ReporteProveedores', 'P') IS NOT NULL
    DROP PROCEDURE sp_ReporteProveedores;
GO

CREATE PROCEDURE sp_ReporteProveedores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Consecutivo,
        Nombre,
        Telefono,
        Correo,
        Direccion
    FROM Proveedores
    ORDER BY Nombre;
END
GO


SELECT 
    C.Consecutivo,
    C.NombreCliente,
    C.Cedula,
    C.FechaCita,
    C.HoraCita,
    C.Servicio,
    E.Estado,
    U.NombreCompleto AS CreadaPor
FROM Citas C
INNER JOIN Usuarios U ON C.CreadaPor = U.Consecutivo
INNER JOIN Estado E ON C.Estado = E.Consecutivo;


/*vehiculos*/

--registro vehiculo:

CREATE PROCEDURE sp_RegistroVehiculo
    @Nombre_Cliente VARCHAR(100),
    @Telefono INT,
    @Cedula VARCHAR(100),
    @Placa VARCHAR(20),
    @Marca VARCHAR(50),
    @Modelo VARCHAR(50),
    @Anio INT,
    @Problema VARCHAR(500),
    @Revision VARCHAR(500),
    @Estado INT
AS
BEGIN
    -- VALIDAR PLACA
    IF EXISTS (SELECT 1 FROM Vehiculos WHERE Placa = @Placa)
    BEGIN
        SELECT -1 AS Resultado
        RETURN
    END
    -- INSERTAR VEHICULO
    INSERT INTO Vehiculos
    (
        Nombre_Cliente,
        Telefono,
        Cedula,
        Placa,
        Marca,
        Modelo,
        Anio,
        Problema,
        Revision,
        Estado
    )
    VALUES
    (
        @Nombre_Cliente,
        @Telefono,
        @Cedula,
        @Placa,
        @Marca,
        @Modelo,
        @Anio,
        @Problema,
        @Revision,
        @Estado
    )
    SELECT 1 AS Resultado 

END

drop procedure sp_ConsultarVehiculos
-- obtener datos del vehiculo
CREATE OR ALTER PROCEDURE sp_ConsultarVehiculos
AS
BEGIN
    SELECT 
        V.Consecutivo,
        V.Nombre_Cliente,
        V.Telefono,
        V.Cedula,
        V.Placa,
        V.Marca,
        V.Modelo,
        V.Anio,
        V.Problema,
        V.Revision,
        E.Estado AS Estado
    FROM dbo.Vehiculos V
    INNER JOIN dbo.Estado E ON V.Estado = E.Consecutivo
END


-- actualizar datos del vehiculo
CREATE PROCEDURE sp_EditarVehiculo
    @Consecutivo INT,
    @Nombre_Cliente VARCHAR(100),
    @Telefono INT,
    @Cedula VARCHAR(100),
    @Placa VARCHAR(20),
    @Marca VARCHAR(50),
    @Modelo VARCHAR(50),
    @Anio INT,
    @Problema VARCHAR(500),
    @Revision VARCHAR(500),
    @Estado INT
AS
BEGIN

UPDATE Vehiculos
SET 
    Nombre_Cliente = @Nombre_Cliente,
    Telefono = @Telefono,
    Cedula = @Cedula,
    Placa = @Placa,
    Marca = @Marca,
    Modelo = @Modelo,
    Anio = @Anio,
    Problema = @Problema,
    Revision = @Revision,
    Estado = @Estado

WHERE Consecutivo = @Consecutivo
END


CREATE PROCEDURE [dbo].[sp_ObtenerVehiculoId]
    @Consecutivo INT
AS
BEGIN
    SELECT 
        V.Consecutivo,
        V.Nombre_Cliente,
        V.Telefono,
        V.Cedula,
        V.Placa,
        V.Marca,
        V.Modelo,
        V.Anio,
        V.Problema,
        V.Revision,
        V.Estado,
        E.Estado AS NombreEstado
    FROM Vehiculos V
    INNER JOIN Estado E ON V.Estado = E.Consecutivo
    WHERE V.Consecutivo = @Consecutivo
END

-- Registrar un ingreso
CREATE PROCEDURE sp_RegistroIngreso
    @Descripcion VARCHAR(200),
    @Monto DECIMAL(10,2),
    @Saldo_Pendiente DECIMAL(10,2),
    @Estado INT
AS
BEGIN
    -- INSERT
    INSERT INTO Ingresos
    (
        Descripcion,
        Monto,
        Saldo_Pendiente,
        Estado
    )
    VALUES
    (
        @Descripcion,
        @Monto,
        @Saldo_Pendiente,
        @Estado
    )
END

--Consultar Ingresos
DROP PROCEDURE sp_ConsultarIngreso
CREATE OR ALTER PROCEDURE sp_ConsultarIngreso
AS
BEGIN
    SELECT 
        I.Consecutivo,
        I.Descripcion,
        I.Monto,
        I.Saldo_Pendiente,
        I.Fecha,
        E.Estado AS Estado
    FROM Ingresos I
    INNER JOIN Estado E ON I.Estado = E.Consecutivo
END

-- editar los ingresos
CREATE PROCEDURE sp_EditarIngreso
    @Consecutivo INT,
    @Descripcion VARCHAR(200),
    @Monto DECIMAL(10,2),
    @Saldo_Pendiente DECIMAL(10,2),
    @Estado INT
AS
BEGIN

    UPDATE Ingresos
    SET 
        Descripcion = @Descripcion,
        Monto = @Monto,
        Saldo_Pendiente = @Saldo_Pendiente,
        Estado = @Estado
    WHERE Consecutivo = @Consecutivo

    SELECT 1 AS Resultado

END

CREATE PROCEDURE [dbo].[sp_ObtenerIngresoId]
    @Consecutivo INT
AS
BEGIN
    SELECT 
        I.Consecutivo,
        I.Descripcion,
        I.Monto,
        I.Saldo_Pendiente,
        I.Fecha,
        I.Estado,
        E.Estado AS NombreEstado
    FROM Ingresos I
    INNER JOIN Estado E ON I.Estado = E.Consecutivo
    WHERE I.Consecutivo = @Consecutivo
END

--- Registrar un nuevo Egreso
CREATE PROCEDURE sp_RegistroEgreso
    @Motivo VARCHAR(200),
    @Monto DECIMAL(10,2),
    @Cantidad INT,
    @RegistradoPor INT,
    @MetodoPago VARCHAR(50)
AS
BEGIN
    INSERT INTO Egresos
    (
        Motivo,
        Monto,
        Cantidad,
        RegistradoPor,
        MetodoPago
    )
    VALUES
    (
        @Motivo,
        @Monto,
        @Cantidad,
        @RegistradoPor,
        @MetodoPago
    )
    SELECT 1 AS Resultado
END

-- Consultar Egresos
CREATE PROCEDURE sp_ConsultarEgreso
AS
BEGIN
    SELECT 
        E.Consecutivo,
        E.Motivo,
        E.Monto,
        E.Cantidad,
        E.Fecha,
        E.MetodoPago,
        U.NombreCompleto AS RegistradoPor
    FROM Egresos E
    INNER JOIN Usuarios U ON E.RegistradoPor = U.Consecutivo
END

-- Editar Egreso
CREATE PROCEDURE sp_EditarEgreso
    @Consecutivo INT,
    @Motivo VARCHAR(200),
    @Monto DECIMAL(10,2),
    @Cantidad INT,
    @RegistradoPor INT,
    @MetodoPago VARCHAR(50)
AS
BEGIN
    UPDATE Egresos
    SET 
        Motivo = @Motivo,
        Monto = @Monto,
        Cantidad = @Cantidad,
        RegistradoPor = @RegistradoPor,
        MetodoPago = @MetodoPago
    WHERE Consecutivo = @Consecutivo

    SELECT 1 AS Resultado

END

-- Id del egreso
CREATE PROCEDURE [dbo].[sp_ObtenerEgresoId]
    @Consecutivo INT
AS
BEGIN
    SELECT 
        E.Consecutivo,
        E.Motivo,
        E.Monto,
        E.Cantidad,
        E.Fecha,
        E.MetodoPago,
        E.RegistradoPor,
        U.NombreCompleto AS NombreUsuario
    FROM Egresos E
    INNER JOIN Usuarios U ON E.RegistradoPor = U.Consecutivo
    WHERE E.Consecutivo = @Consecutivo
END
drop procedure sp_ObtenerUsuarioConta
CREATE PROCEDURE sp_ObtenerUsuarioConta
AS
BEGIN

SELECT

Consecutivo,
NombreCompleto
FROM
Usuarios
END
use TallerLaUnion
/*INICIO SESION*/
drop procedure sp_IniciarSesion
CREATE PROCEDURE sp_IniciarSesion
  @CorreoElectronico VARCHAR(100),
    @Contrasenna VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.Consecutivo,
        u.NombreCompleto,
        u.Cedula,
        u.Correo,
        u.UsuarioLogin,
        u.Contrasenna,
        r.NombreRol,
        e.Estado,
        u.FechaRegistro
    FROM Usuarios u
    INNER JOIN Roles r ON u.NombreRol = r.Consecutivo
    INNER JOIN Estado e ON u.Estado = e.Consecutivo
    WHERE u.Correo = @CorreoElectronico
      AND u.Contrasenna = @Contrasenna
      AND e.Estado = 'Activo'
   
END
GO
/*Recuperar Acceso*/


USE tallerLaUnion
drop PROCEDURE sp_RecuperarContrasenna
CREATE PROCEDURE sp_RecuperarContrasenna
    @Correo VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        U.Consecutivo,
        U.NombreCompleto,
        U.Correo,
        U.UsuarioLogin,
        E.Estado
    FROM Usuarios U
    INNER JOIN Estado E ON U.Estado = E.Consecutivo
    WHERE U.Correo = @Correo;
END
GO
drop procedure sp_ActualizarContrasenna

CREATE PROCEDURE sp_ActualizarContrasenna
    @Consecutivo INT,
    @Contrasenna VARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET Contrasenna = @Contrasenna
    WHERE Consecutivo = @Consecutivo
      AND Estado = 1;

    SELECT @@ROWCOUNT AS FilasActualizadas;
END
GO

use TallerLaUnion
select * from estado;
select * from usuarios;
select * from Citas
select * from Estado
select * from Vehiculos;
select * from   Egresos
select * from   Ingresos




