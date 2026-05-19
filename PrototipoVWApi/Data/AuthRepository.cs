using PrototipoVWApi.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace PrototipoVWApi.Data
{
    public class AuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("No existe la connection string DefaultConnection.");
        }

        public async Task<LoginResponse?> ValidarLoginAsync(LoginRequest request)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand("dbo.sp_ValidarLogin", connection);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@Correo", SqlDbType.NVarChar, 100).Value = request.Correo;
            command.Parameters.Add("@Contrasena", SqlDbType.NVarChar, 100).Value = request.Contrasena;

            await connection.OpenAsync();

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return new LoginResponse
            {
                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                NombreCompleto = reader["NombreCompleto"].ToString() ?? "",
                Correo = reader["Correo"].ToString() ?? "",
                Rol = reader["Rol"].ToString() ?? ""
            };
        }
    }
}
