CREATE DATABASE PrototipoVW;

USE PrototipoVW;

/* =========================================================
   BASE DE DATOS - PROTOTIPO VOLKSWAGEN IT PROJECTS
   SQL SERVER
   .NET CORE 8
   ========================================================= */

-- =========================================================
-- 1. TABLAS
-- =========================================================

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL,
    Contrasena NVARCHAR(100) NOT NULL,
    Rol NVARCHAR(20) NOT NULL, -- Admin, Empleado, Supervisor
    Activo BIT NOT NULL DEFAULT 1,
    FechaCreacion DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT CK_Usuarios_Rol
    CHECK (Rol IN ('Admin', 'Empleado', 'Supervisor'))
);
GO

-- Permite reutilizar un correo si el usuario anterior fue eliminado lógicamente
CREATE UNIQUE INDEX UX_Usuarios_Correo_Activo
ON Usuarios(Correo)
WHERE Activo = 1;
GO


CREATE TABLE Propuestas (
    IdPropuesta INT IDENTITY(1,1) PRIMARY KEY,

    IdUsuarioCreador INT NOT NULL,
    IdUsuarioRevisor INT NULL,
    IdUsuarioCompletado INT NULL,

    NombreProyecto NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(1000) NULL,
    AreaSolicitante NVARCHAR(100) NOT NULL,
    Justificacion NVARCHAR(500) NULL,

    PresupuestoSolicitado DECIMAL(18,2) NOT NULL,
    PresupuestoAprobado DECIMAL(18,2) NOT NULL DEFAULT 0,

    Estado NVARCHAR(30) NOT NULL DEFAULT 'Pendiente',
    -- Pendiente, Aprobado, Rechazado, Completado

    ComentarioRevision NVARCHAR(500) NULL,

    FechaCreacion DATETIME2(0) NOT NULL DEFAULT SYSDATETIME(),
    FechaRevision DATETIME2(0) NULL,
    FechaCompletado DATETIME2(0) NULL,

    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Propuestas_UsuarioCreador
    FOREIGN KEY (IdUsuarioCreador) REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Propuestas_UsuarioRevisor
    FOREIGN KEY (IdUsuarioRevisor) REFERENCES Usuarios(IdUsuario),

    CONSTRAINT FK_Propuestas_UsuarioCompletado
    FOREIGN KEY (IdUsuarioCompletado) REFERENCES Usuarios(IdUsuario),

    CONSTRAINT CK_Propuestas_Estado
    CHECK (Estado IN ('Pendiente', 'Aprobado', 'Rechazado', 'Completado')),

    CONSTRAINT CK_Propuestas_PresupuestoSolicitado
    CHECK (PresupuestoSolicitado >= 0),

    CONSTRAINT CK_Propuestas_PresupuestoAprobado
    CHECK (PresupuestoAprobado >= 0)
);
GO


-- =========================================================
-- 2. DATOS INICIALES
-- =========================================================

INSERT INTO Usuarios (NombreCompleto, Correo, Contrasena, Rol)
VALUES
('Administrador General', 'admin@volkswagen.com', '123456', 'Admin'),
('Empleado de Negocio', 'empleado@volkswagen.com', '123456', 'Empleado'),
('Supervisor PMO', 'supervisor@volkswagen.com', '123456', 'Supervisor');
GO


-- =========================================================
-- 3. PROCEDIMIENTOS ALMACENADOS - USUARIOS
-- =========================================================
USE PrototipoVW;
GO
CREATE  PROCEDURE sp_ValidarLogin
    @Correo NVARCHAR(100),
    @Contrasena NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        IdUsuario,
        NombreCompleto,
        Correo,
        Rol
    FROM Usuarios
    WHERE Correo = @Correo
      AND Contrasena = @Contrasena
      AND Activo = 1;
END;
GO


CREATE  PROCEDURE sp_CrearUsuario
    @NombreCompleto NVARCHAR(100),
    @Correo NVARCHAR(100),
    @Contrasena NVARCHAR(100),
    @Rol NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Usuarios (
        NombreCompleto,
        Correo,
        Contrasena,
        Rol
    )
    VALUES (
        @NombreCompleto,
        @Correo,
        @Contrasena,
        @Rol
    );

    SELECT SCOPE_IDENTITY() AS IdUsuario;
END;
GO


CREATE  PROCEDURE sp_ListarUsuariosActivos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdUsuario,
        NombreCompleto,
        Correo,
        Rol,
        FechaCreacion
    FROM Usuarios
    WHERE Activo = 1
    ORDER BY NombreCompleto;
END;
GO


CREATE  PROCEDURE sp_ObtenerUsuarioPorId
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        IdUsuario,
        NombreCompleto,
        Correo,
        Rol,
        FechaCreacion
    FROM Usuarios
    WHERE IdUsuario = @IdUsuario
      AND Activo = 1;
END;
GO


CREATE  PROCEDURE sp_ActualizarUsuario
    @IdUsuario INT,
    @NombreCompleto NVARCHAR(100),
    @Correo NVARCHAR(100),
    @Contrasena NVARCHAR(100),
    @Rol NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET
        NombreCompleto = @NombreCompleto,
        Correo = @Correo,
        Contrasena = @Contrasena,
        Rol = @Rol
    WHERE IdUsuario = @IdUsuario
      AND Activo = 1;
END;
GO


CREATE  PROCEDURE sp_EliminarUsuario
    @IdUsuario INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET Activo = 0
    WHERE IdUsuario = @IdUsuario
      AND Activo = 1;
END;
GO


-- =========================================================
-- 4. PROCEDIMIENTOS ALMACENADOS - PROPUESTAS
-- =========================================================

CREATE  PROCEDURE sp_CrearPropuesta
    @IdUsuarioCreador INT,
    @NombreProyecto NVARCHAR(150),
    @Descripcion NVARCHAR(1000),
    @AreaSolicitante NVARCHAR(100),
    @Justificacion NVARCHAR(500),
    @PresupuestoSolicitado DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Propuestas (
        IdUsuarioCreador,
        NombreProyecto,
        Descripcion,
        AreaSolicitante,
        Justificacion,
        PresupuestoSolicitado,
        Estado
    )
    VALUES (
        @IdUsuarioCreador,
        @NombreProyecto,
        @Descripcion,
        @AreaSolicitante,
        @Justificacion,
        @PresupuestoSolicitado,
        'Pendiente'
    );

    SELECT SCOPE_IDENTITY() AS IdPropuesta;
END;
GO


CREATE  PROCEDURE sp_ListarPropuestas
    @Estado NVARCHAR(30) = NULL,
    @IdUsuarioCreador INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPropuesta,
        P.NombreProyecto,
        P.Descripcion,
        P.AreaSolicitante,
        P.Justificacion,
        P.PresupuestoSolicitado,
        P.PresupuestoAprobado,
        P.Estado,
        P.ComentarioRevision,
        P.FechaCreacion,
        P.FechaRevision,
        P.FechaCompletado,

        C.IdUsuario AS IdCreador,
        C.NombreCompleto AS Creador,

        R.IdUsuario AS IdRevisor,
        R.NombreCompleto AS Revisor,

        F.IdUsuario AS IdCompletadoPor,
        F.NombreCompleto AS CompletadoPor

    FROM Propuestas P
    INNER JOIN Usuarios C 
        ON P.IdUsuarioCreador = C.IdUsuario
    LEFT JOIN Usuarios R 
        ON P.IdUsuarioRevisor = R.IdUsuario
    LEFT JOIN Usuarios F 
        ON P.IdUsuarioCompletado = F.IdUsuario
    WHERE P.Activo = 1
      AND (@Estado IS NULL OR P.Estado = @Estado)
      AND (@IdUsuarioCreador IS NULL OR P.IdUsuarioCreador = @IdUsuarioCreador)
    ORDER BY P.FechaCreacion DESC;
END;
GO


CREATE  PROCEDURE sp_ObtenerPropuestaPorId
    @IdPropuesta INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPropuesta,
        P.NombreProyecto,
        P.Descripcion,
        P.AreaSolicitante,
        P.Justificacion,
        P.PresupuestoSolicitado,
        P.PresupuestoAprobado,
        P.Estado,
        P.ComentarioRevision,
        P.FechaCreacion,
        P.FechaRevision,
        P.FechaCompletado,

        C.IdUsuario AS IdCreador,
        C.NombreCompleto AS Creador,

        R.IdUsuario AS IdRevisor,
        R.NombreCompleto AS Revisor,

        F.IdUsuario AS IdCompletadoPor,
        F.NombreCompleto AS CompletadoPor

    FROM Propuestas P
    INNER JOIN Usuarios C 
        ON P.IdUsuarioCreador = C.IdUsuario
    LEFT JOIN Usuarios R 
        ON P.IdUsuarioRevisor = R.IdUsuario
    LEFT JOIN Usuarios F 
        ON P.IdUsuarioCompletado = F.IdUsuario
    WHERE P.IdPropuesta = @IdPropuesta
      AND P.Activo = 1;
END;
GO


CREATE  PROCEDURE sp_ActualizarPropuestaPendiente
    @IdPropuesta INT,
    @NombreProyecto NVARCHAR(150),
    @Descripcion NVARCHAR(1000),
    @AreaSolicitante NVARCHAR(100),
    @Justificacion NVARCHAR(500),
    @PresupuestoSolicitado DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Propuestas
    SET
        NombreProyecto = @NombreProyecto,
        Descripcion = @Descripcion,
        AreaSolicitante = @AreaSolicitante,
        Justificacion = @Justificacion,
        PresupuestoSolicitado = @PresupuestoSolicitado
    WHERE IdPropuesta = @IdPropuesta
      AND Activo = 1
      AND Estado = 'Pendiente';
END;
GO


CREATE  PROCEDURE sp_AprobarPropuesta
    @IdPropuesta INT,
    @IdUsuarioRevisor INT,
    @PresupuestoAprobado DECIMAL(18,2),
    @ComentarioRevision NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Propuestas
    SET
        Estado = 'Aprobado',
        IdUsuarioRevisor = @IdUsuarioRevisor,
        PresupuestoAprobado = @PresupuestoAprobado,
        ComentarioRevision = @ComentarioRevision,
        FechaRevision = SYSDATETIME()
    WHERE IdPropuesta = @IdPropuesta
      AND Activo = 1
      AND Estado = 'Pendiente';
END;
GO


CREATE  PROCEDURE sp_RechazarPropuesta
    @IdPropuesta INT,
    @IdUsuarioRevisor INT,
    @ComentarioRevision NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Propuestas
    SET
        Estado = 'Rechazado',
        IdUsuarioRevisor = @IdUsuarioRevisor,
        PresupuestoAprobado = 0,
        ComentarioRevision = @ComentarioRevision,
        FechaRevision = SYSDATETIME()
    WHERE IdPropuesta = @IdPropuesta
      AND Activo = 1
      AND Estado = 'Pendiente';
END;
GO


CREATE  PROCEDURE sp_CompletarProyecto
    @IdPropuesta INT,
    @IdUsuarioCompletado INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Propuestas
    SET
        Estado = 'Completado',
        IdUsuarioCompletado = @IdUsuarioCompletado,
        FechaCompletado = SYSDATETIME()
    WHERE IdPropuesta = @IdPropuesta
      AND Activo = 1
      AND Estado = 'Aprobado';
END;
GO


CREATE  PROCEDURE sp_EliminarPropuesta
    @IdPropuesta INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Propuestas
    SET Activo = 0
    WHERE IdPropuesta = @IdPropuesta
      AND Activo = 1;
END;
GO


-- =========================================================
-- 5. PROCEDIMIENTOS ALMACENADOS - REPORTES
-- =========================================================

CREATE  PROCEDURE sp_ReporteResumenGeneral
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        COUNT(*) AS TotalPropuestas,

        SUM(CASE WHEN Estado = 'Pendiente' THEN 1 ELSE 0 END) AS TotalPendientes,
        SUM(CASE WHEN Estado = 'Aprobado' THEN 1 ELSE 0 END) AS TotalAprobadas,
        SUM(CASE WHEN Estado = 'Rechazado' THEN 1 ELSE 0 END) AS TotalRechazadas,
        SUM(CASE WHEN Estado = 'Completado' THEN 1 ELSE 0 END) AS TotalCompletadas,

        ISNULL(SUM(PresupuestoSolicitado), 0) AS PresupuestoTotalSolicitado,
        ISNULL(SUM(PresupuestoAprobado), 0) AS PresupuestoTotalAprobado

    FROM Propuestas
    WHERE Activo = 1;
END;
GO


CREATE  PROCEDURE sp_ReportePropuestasPorEstado
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Estado,
        COUNT(*) AS Cantidad,
        ISNULL(SUM(PresupuestoSolicitado), 0) AS PresupuestoSolicitado,
        ISNULL(SUM(PresupuestoAprobado), 0) AS PresupuestoAprobado
    FROM Propuestas
    WHERE Activo = 1
    GROUP BY Estado
    ORDER BY Estado;
END;
GO


CREATE  PROCEDURE sp_ReportePresupuestoPorArea
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        AreaSolicitante,
        COUNT(*) AS CantidadPropuestas,
        ISNULL(SUM(PresupuestoSolicitado), 0) AS PresupuestoSolicitado,
        ISNULL(SUM(PresupuestoAprobado), 0) AS PresupuestoAprobado
    FROM Propuestas
    WHERE Activo = 1
    GROUP BY AreaSolicitante
    ORDER BY AreaSolicitante;
END;
GO


CREATE  PROCEDURE sp_ReporteProyectosCompletados
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.IdPropuesta,
        P.NombreProyecto,
        P.AreaSolicitante,
        P.PresupuestoAprobado,
        P.FechaCompletado,
        U.NombreCompleto AS CompletadoPor
    FROM Propuestas P
    LEFT JOIN Usuarios U
        ON P.IdUsuarioCompletado = U.IdUsuario
    WHERE P.Activo = 1
      AND P.Estado = 'Completado'
    ORDER BY P.FechaCompletado DESC;
END;
GO