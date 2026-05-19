using PrototipoVW.Models;
using System.Text.Json;

namespace PrototipoVW.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse?> LoginAsync(LoginViewModel model)
        {
            var request = new LoginRequest
            {
                Correo = model.Correo,
                Contrasena = model.Contrasena
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOptions);
        }
    }
}
