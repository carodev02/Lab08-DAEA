USE BibliotecaDB;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Libros_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, l.Ejemplares, l.Activo,
           a.Nombre AS AutorNombre
    FROM dbo.Libros AS l
    INNER JOIN dbo.Autores AS a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1
    ORDER BY l.Titulo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Libros_BuscarPorTitulo
    @Titulo NVARCHAR(180)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, l.Ejemplares, l.Activo,
           a.Nombre AS AutorNombre
    FROM dbo.Libros AS l
    INNER JOIN dbo.Autores AS a ON a.AutorId = l.AutorId
    WHERE l.Activo = 1 AND l.Titulo LIKE N'%' + @Titulo + N'%'
    ORDER BY l.Titulo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Libros_ObtenerPorId
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT l.LibroId, l.Titulo, l.ISBN, l.AutorId, l.Ejemplares, l.Activo,
           a.Nombre AS AutorNombre
    FROM dbo.Libros AS l
    INNER JOIN dbo.Autores AS a ON a.AutorId = l.AutorId
    WHERE l.LibroId = @LibroId AND l.Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Libros_Insertar
    @Titulo NVARCHAR(180),
    @ISBN VARCHAR(20),
    @AutorId INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT dbo.Libros (Titulo, ISBN, AutorId, Ejemplares, Activo)
    VALUES (@Titulo, @ISBN, @AutorId, @Ejemplares, 1);
    SELECT CONVERT(INT, SCOPE_IDENTITY());
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Libros_Actualizar
    @LibroId INT,
    @Titulo NVARCHAR(180),
    @ISBN VARCHAR(20),
    @AutorId INT,
    @Ejemplares INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Libros
       SET Titulo = @Titulo, ISBN = @ISBN, AutorId = @AutorId, Ejemplares = @Ejemplares
     WHERE LibroId = @LibroId AND Activo = 1;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Libros_EliminarLogico
    @LibroId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Libros SET Activo = 0 WHERE LibroId = @LibroId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Autores_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT AutorId, Nombre
    FROM dbo.Autores
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Socios_ListarActivos
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SocioId, DNI, Nombre, Email, Activo
    FROM dbo.Socios
    WHERE Activo = 1
    ORDER BY Nombre;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Socios_Insertar
    @DNI VARCHAR(15),
    @Nombre NVARCHAR(140),
    @Email NVARCHAR(180)
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM dbo.Socios WHERE DNI = @DNI)
        THROW 50001, 'El DNI ya existe.', 1;

    INSERT dbo.Socios (DNI, Nombre, Email, Activo)
    VALUES (@DNI, @Nombre, @Email, 1);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_Prestamos_ReportePorFechas
    @Desde DATE,
    @Hasta DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT p.PrestamoId,
           s.Nombre AS Socio,
           STRING_AGG(CONVERT(NVARCHAR(MAX), l.Titulo), N', ') AS Libros,
           p.FechaPrestamo,
           p.FechaLimite,
           CASE
               WHEN p.Estado = N'Devuelto' THEN N'Devuelto'
               WHEN p.FechaLimite < CAST(GETDATE() AS DATE) THEN N'Vencido'
               ELSE p.Estado
           END AS Estado
    FROM dbo.Prestamos AS p
    INNER JOIN dbo.Socios AS s ON s.SocioId = p.SocioId
    INNER JOIN dbo.DetallePrestamo AS dp ON dp.PrestamoId = p.PrestamoId
    INNER JOIN dbo.Libros AS l ON l.LibroId = dp.LibroId
    WHERE p.FechaPrestamo BETWEEN @Desde AND @Hasta
    GROUP BY p.PrestamoId, s.Nombre, p.FechaPrestamo, p.FechaLimite, p.Estado
    ORDER BY p.FechaPrestamo DESC, p.PrestamoId DESC;
END;
GO
