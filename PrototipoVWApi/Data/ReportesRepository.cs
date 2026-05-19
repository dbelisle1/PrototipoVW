using System.Data;
using Microsoft.Data.SqlClient;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Data;

public class ReportesRepository
{
    private readonly string _connectionString;

    public ReportesRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("No existe la connection string DefaultConnection.");
    }

    public async Task<ReporteDashboardDto> ObtenerDashboardAsync()
    {
        var dashboard = new ReporteDashboardDto();

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        dashboard.Resumen = await ObtenerResumenAsync(connection);
        dashboard.PropuestasPorEstado = await ObtenerPropuestasPorEstadoAsync(connection);
        dashboard.PresupuestoPorArea = await ObtenerPresupuestoPorAreaAsync(connection);
        dashboard.ProyectosCompletados = await ObtenerProyectosCompletadosAsync(connection);

        return dashboard;
    }

    private static async Task<ReporteResumenDto> ObtenerResumenAsync(SqlConnection connection)
    {
        await using var command = new SqlCommand("dbo.sp_ReporteResumenGeneral", connection);
        command.CommandType = CommandType.StoredProcedure;

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return new ReporteResumenDto();
        }

        return new ReporteResumenDto
        {
            TotalPropuestas = Convert.ToInt32(reader["TotalPropuestas"]),
            TotalPendientes = Convert.ToInt32(reader["TotalPendientes"]),
            TotalAprobadas = Convert.ToInt32(reader["TotalAprobadas"]),
            TotalRechazadas = Convert.ToInt32(reader["TotalRechazadas"]),
            TotalCompletadas = Convert.ToInt32(reader["TotalCompletadas"]),
            PresupuestoTotalSolicitado = Convert.ToDecimal(reader["PresupuestoTotalSolicitado"]),
            PresupuestoTotalAprobado = Convert.ToDecimal(reader["PresupuestoTotalAprobado"])
        };
    }

    private static async Task<List<ReporteEstadoDto>> ObtenerPropuestasPorEstadoAsync(SqlConnection connection)
    {
        var lista = new List<ReporteEstadoDto>();

        await using var command = new SqlCommand("dbo.sp_ReportePropuestasPorEstado", connection);
        command.CommandType = CommandType.StoredProcedure;

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new ReporteEstadoDto
            {
                Estado = reader["Estado"].ToString() ?? "",
                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                PresupuestoSolicitado = Convert.ToDecimal(reader["PresupuestoSolicitado"]),
                PresupuestoAprobado = Convert.ToDecimal(reader["PresupuestoAprobado"])
            });
        }

        return lista;
    }

    private static async Task<List<ReporteAreaDto>> ObtenerPresupuestoPorAreaAsync(SqlConnection connection)
    {
        var lista = new List<ReporteAreaDto>();

        await using var command = new SqlCommand("dbo.sp_ReportePresupuestoPorArea", connection);
        command.CommandType = CommandType.StoredProcedure;

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new ReporteAreaDto
            {
                AreaSolicitante = reader["AreaSolicitante"].ToString() ?? "",
                CantidadPropuestas = Convert.ToInt32(reader["CantidadPropuestas"]),
                PresupuestoSolicitado = Convert.ToDecimal(reader["PresupuestoSolicitado"]),
                PresupuestoAprobado = Convert.ToDecimal(reader["PresupuestoAprobado"])
            });
        }

        return lista;
    }

    private static async Task<List<ReporteProyectoCompletadoDto>> ObtenerProyectosCompletadosAsync(SqlConnection connection)
    {
        var lista = new List<ReporteProyectoCompletadoDto>();

        await using var command = new SqlCommand("dbo.sp_ReporteProyectosCompletados", connection);
        command.CommandType = CommandType.StoredProcedure;

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            lista.Add(new ReporteProyectoCompletadoDto
            {
                IdPropuesta = Convert.ToInt32(reader["IdPropuesta"]),
                NombreProyecto = reader["NombreProyecto"].ToString() ?? "",
                AreaSolicitante = reader["AreaSolicitante"].ToString() ?? "",
                PresupuestoAprobado = Convert.ToDecimal(reader["PresupuestoAprobado"]),
                FechaCompletado = reader["FechaCompletado"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(reader["FechaCompletado"]),
                CompletadoPor = reader["CompletadoPor"] == DBNull.Value
                    ? null
                    : reader["CompletadoPor"].ToString()
            });
        }

        return lista;
    }
}