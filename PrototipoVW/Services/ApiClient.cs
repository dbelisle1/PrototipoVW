using PrototipoVW.Models;
using System.Text.Json;

namespace PrototipoVW.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };



        public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<List<UsuarioListItemViewModel>> ListarUsuariosAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/usuarios");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<UsuarioListItemViewModel>();
            }

            var usuarios = await response.Content.ReadFromJsonAsync<List<UsuarioListItemViewModel>>(JsonOptions);

            return usuarios ?? new List<UsuarioListItemViewModel>();
        }

        public async Task<UsuarioFormViewModel?> ObtenerUsuarioPorIdAsync(int idUsuario)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"api/usuarios/{idUsuario}");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var usuario = await response.Content.ReadFromJsonAsync<UsuarioFormViewModel>(JsonOptions);

            return usuario;
        }

        public async Task<bool> CrearUsuarioAsync(UsuarioFormViewModel model)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "api/usuarios");
            AgregarHeadersSesion(request);

            request.Content = JsonContent.Create(new
            {
                model.NombreCompleto,
                model.Correo,
                model.Contrasena,
                model.Rol
            });

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarUsuarioAsync(UsuarioFormViewModel model)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, $"api/usuarios/{model.IdUsuario}");
            AgregarHeadersSesion(request);

            request.Content = JsonContent.Create(new
            {
                model.NombreCompleto,
                model.Correo,
                model.Contrasena,
                model.Rol
            });

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarUsuarioAsync(int idUsuario)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/usuarios/{idUsuario}");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        private void AgregarHeadersSesion(HttpRequestMessage request)
        {
            var session = _httpContextAccessor.HttpContext?.Session;

            if (session == null)
            {
                return;
            }

            var idUsuario = session.GetInt32("IdUsuario");
            var rol = session.GetString("Rol");

            if (idUsuario != null && !string.IsNullOrWhiteSpace(rol))
            {
                request.Headers.Add("X-User-Id", idUsuario.Value.ToString());
                request.Headers.Add("X-User-Role", rol);
            }
        }
    }
}
