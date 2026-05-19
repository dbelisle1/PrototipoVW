using System.Data;
using Microsoft.Data.SqlClient;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Data;

public class PropuestasRepository
{
    private readonly string _connectionString;

    public PropuestasRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("No existe la connection string DefaultConnection.");
    }

    public async Task<List<PropuestaDto>> ListarAsync(string? estado, int? idUsuarioCreador)
    {
        var propuestas = new List<PropuestaDto>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("dbo.sp_ListarPropuestas", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@Estado", SqlDbType.NVarChar, 30).Value =
            string.IsNullOrWhiteSpace(estado) ? DBNull.Value : estado;
        command.Parameters.Add("@IdUsuarioCreador", SqlDbType.Int).Value =
            idUsuarioCreador.HasValue ? idUsuarioCreador.Value : DBNull.Value;

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            propuestas.Add(MapearPropuesta(reader));
        }

        return propuestas;
    }

    public async Task<PropuestaDto?> ObtenerPorIdAsync(int idPropuesta)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("dbo.sp_ObtenerPropuestaPorId", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@IdPropuesta", SqlDbType.Int).Value = idPropuesta;

        await connection.OpenAsync();

        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        return MapearPropuesta(reader);
    }

    public async Task<int> CrearAsync(int idUsuarioCreador, PropuestaCreateRequest request)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("dbo.sp_CrearPropuesta", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@IdUsuarioCreador", SqlDbType.Int).Value = idUsuarioCreador;
        command.Parameters.Add("@NombreProyecto", SqlDbType.NVarChar, 150).Value = request.NombreProyecto;
        command.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000).Value =
            string.IsNullOrWhiteSpace(request.Descripcion) ? DBNull.Value : request.Descripcion;
        command.Parameters.Add("@AreaSolicitante", SqlDbType.NVarChar, 100).Value = request.AreaSolicitante;
        command.Parameters.Add("@Justificacion", SqlDbType.NVarChar, 500).Value =
            string.IsNullOrWhiteSpace(request.Justificacion) ? DBNull.Value : request.Justificacion;
        command.Parameters.Add("@PresupuestoSolicitado", SqlDbType.Decimal).Value = request.PresupuestoSolicitado;

        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task ActualizarAsync(int idPropuesta, PropuestaUpdateRequest request)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("dbo.sp_ActualizarPropuestaPendiente", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@IdPropuesta", SqlDbType.Int).Value = idPropuesta;
        command.Parameters.Add("@NombreProyecto", SqlDbType.NVarChar, 150).Value = request.NombreProyecto;
        command.Parameters.Add("@Descripcion", SqlDbType.NVarChar, 1000).Value =
            string.IsNullOrWhiteSpace(request.Descripcion) ? DBNull.Value : request.Descripcion;
        command.Parameters.Add("@AreaSolicitante", SqlDbType.NVarChar, 100).Value = request.AreaSolicitante;
        command.Parameters.Add("@Justificacion", SqlDbType.NVarChar, 500).Value =
            string.IsNullOrWhiteSpace(request.Justificacion) ? DBNull.Value : request.Justificacion;
        command.Parameters.Add("@PresupuestoSolicitado", SqlDbType.Decimal).Value = request.PresupuestoSolicitado;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task EliminarAsync(int idPropuesta)
    {
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand("dbo.sp_EliminarPropuesta", connection);

        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.Add("@IdPropuesta", SqlDbType.Int).Value = idPropuesta;

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    private static PropuestaDto MapearPropuesta(SqlDataReader reader)
    {
        return new PropuestaDto
        {
            IdPropuesta = Convert.ToInt32(reader["IdPropuesta"]),

            IdCreador = Convert.ToInt32(reader["IdCreador"]),
            Creador = reader["Creador"].ToString() ?? "",

            IdRevisor = reader["IdRevisor"] == DBNull.Value ? null : Convert.ToInt32(reader["IdRevisor"]),
            Revisor = reader["Revisor"] == DBNull.Value ? null : reader["Revisor"].ToString(),

            IdCompletadoPor = reader["IdCompletadoPor"] == DBNull.Value ? null : Convert.ToInt32(reader["IdCompletadoPor"]),
            CompletadoPor = reader["CompletadoPor"] == DBNull.Value ? null : reader["CompletadoPor"].ToString(),

            NombreProyecto = reader["NombreProyecto"].ToString() ?? "",
            Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString(),
            AreaSolicitante = reader["AreaSolicitante"].ToString() ?? "",
            Justificacion = reader["Justificacion"] == DBNull.Value ? null : reader["Justificacion"].ToString(),

            PresupuestoSolicitado = Convert.ToDecimal(reader["PresupuestoSolicitado"]),
            PresupuestoAprobado = Convert.ToDecimal(reader["PresupuestoAprobado"]),

            Estado = reader["Estado"].ToString() ?? "",
            ComentarioRevision = reader["ComentarioRevision"] == DBNull.Value ? null : reader["ComentarioRevision"].ToString(),

            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
            FechaRevision = reader["FechaRevision"] == DBNull.Value ? null : Convert.ToDateTime(reader["FechaRevision"]),
            FechaCompletado = reader["FechaCompletado"] == DBNull.Value ? null : Convert.ToDateTime(reader["FechaCompletado"])
        };
    }
}