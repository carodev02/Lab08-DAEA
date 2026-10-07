using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios;

public class PrestamoRepositorio
{
    private readonly string _cadenaConexion;

    public PrestamoRepositorio(IConfiguration configuration)
    {
        _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena BibliotecaDB.");
    }

    public async Task<IEnumerable<PrestamoReporte>> ReporteAsync(DateTime desde, DateTime hasta)
    {
        await using var conexion = new SqlConnection(_cadenaConexion);
        return await conexion.QueryAsync<PrestamoReporte>("sp_Prestamos_ReportePorFechas",
            new { Desde = desde.Date, Hasta = hasta.Date }, commandType: CommandType.StoredProcedure);
    }
}
