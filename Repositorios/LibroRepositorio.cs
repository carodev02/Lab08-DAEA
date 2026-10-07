using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class LibroRepositorio
{
    private readonly string _cadenaConexion;

    public LibroRepositorio(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena BibliotecaDB.");
    }

    private SqlConnection CrearConexion() => new(_cadenaConexion);

    public async Task<IEnumerable<Libro>> ListarAsync(string? titulo)
    {
        await using var conexion = CrearConexion();
        if (string.IsNullOrWhiteSpace(titulo))
            return await conexion.QueryAsync<Libro>("sp_Libros_ListarActivos", commandType: CommandType.StoredProcedure);

        return await conexion.QueryAsync<Libro>("sp_Libros_BuscarPorTitulo",
            new { Titulo = titulo.Trim() }, commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        await using var conexion = CrearConexion();
        return await conexion.QuerySingleOrDefaultAsync<Libro>("sp_Libros_ObtenerPorId",
            new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        await using var conexion = CrearConexion();
        return await conexion.QueryAsync<Autor>("sp_Autores_ListarActivos", commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertarAsync(Libro libro)
    {
        await using var conexion = CrearConexion();
        return await conexion.QuerySingleAsync<int>("sp_Libros_Insertar",
            new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(Libro libro)
    {
        await using var conexion = CrearConexion();
        await conexion.ExecuteAsync("sp_Libros_Actualizar",
            new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares },
            commandType: CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        await using var conexion = CrearConexion();
        await conexion.ExecuteAsync("sp_Libros_EliminarLogico",
            new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }
}
