using Microsoft.Data.SqlClient;
using PrototipoVWApi.Models;
using System.Data;

namespace PrototipoVWApi.Data
{
    public class UsuariosRepository
    {
        private readonly string _connectionString;

        public UsuariosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("No existe la connection string DefaultConnection.");
        }

        public async Task<List<UsuarioDto>> ListarAsync()
        {
            var usuarios = new List<UsuarioDto>();

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.sp_ListarUsuariosActivos", connection);

            command.CommandType = CommandType.StoredProcedure;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                usuarios.Add(new UsuarioDto
                {
                    IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                    NombreCompleto = reader["NombreCompleto"].ToString() ?? "",
                    Correo = reader["Correo"].ToString() ?? "",
                    Rol = reader["Rol"].ToString() ?? "",
                    FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
                });
            }

            return usuarios;
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.sp_ObtenerUsuarioPorId", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new UsuarioDto
            {
                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                NombreCompleto = reader["NombreCompleto"].ToString() ?? "",
                Correo = reader["Correo"].ToString() ?? "",
                Rol = reader["Rol"].ToString() ?? "",
                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"])
            };
        }

        public async Task<int> CrearAsync(UsuarioCreateRequest request)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.sp_CrearUsuario", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = request.NombreCompleto;
            command.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = request.Correo;
            command.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 100).Value = request.Contrasena;
            command.Parameters.Add("@Rol", SqlDbType.NVarChar, 20).Value = request.Rol;

            await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task ActualizarAsync(int idUsuario, UsuarioUpdateRequest request)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.sp_ActualizarUsuario", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;
            command.Parameters.Add("@NombreCompleto", SqlDbType.NVarChar, 100).Value = request.NombreCompleto;
            command.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = request.Correo;
            command.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 100).Value =
                string.IsNullOrWhiteSpace(request.Contrasena) ? DBNull.Value : request.Contrasena;
            command.Parameters.Add("@Rol", SqlDbType.NVarChar, 20).Value = request.Rol;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task EliminarAsync(int idUsuario)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.sp_EliminarUsuario", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@IdUsuario", SqlDbType.Int).Value = idUsuario;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
