using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class SocioRepositorio
{
    private readonly string _cadenaConexion;

    public SocioRepositorio(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena BibliotecaDB.");
    }

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        await using var conexion = new SqlConnection(_cadenaConexion);
        return await conexion.QueryAsync<Socio>("sp_Socios_ListarActivos", commandType: CommandType.StoredProcedure);
    }

    public async Task InsertarAsync(Socio socio)
    {
        await using var conexion = new SqlConnection(_cadenaConexion);
        try
        {
            await conexion.ExecuteAsync("sp_Socios_Insertar",
                new { socio.DNI, socio.Nombre, socio.Email }, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex) when (ex.Number is 50001 or 2601 or 2627)
        {
            throw new DniDuplicadoException(socio.DNI, ex);
        }
    }
}

public sealed class DniDuplicadoException : Exception
{
    public DniDuplicadoException(string dni, Exception innerException)
        : base($"El DNI {dni} ya está registrado.", innerException) { }
}
