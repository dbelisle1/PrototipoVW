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


        public async Task<List<PropuestaListItemViewModel>> ListarPropuestasAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/propuestas");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<PropuestaListItemViewModel>();
            }

            var propuestas = await response.Content.ReadFromJsonAsync<List<PropuestaListItemViewModel>>(JsonOptions);

            return propuestas ?? new List<PropuestaListItemViewModel>();
        }

        public async Task<PropuestaFormViewModel?> ObtenerPropuestaPorIdAsync(int idPropuesta)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"api/propuestas/{idPropuesta}");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<PropuestaFormViewModel>(JsonOptions);
        }

        public async Task<bool> CrearPropuestaAsync(PropuestaFormViewModel model)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "api/propuestas");
            AgregarHeadersSesion(request);

            request.Content = JsonContent.Create(new
            {
                model.NombreProyecto,
                model.Descripcion,
                model.AreaSolicitante,
                model.Justificacion,
                model.PresupuestoSolicitado
            });

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarPropuestaAsync(PropuestaFormViewModel model)
        {
            using var request = new HttpRequestMessage(HttpMethod.Put, $"api/propuestas/{model.IdPropuesta}");
            AgregarHeadersSesion(request);

            request.Content = JsonContent.Create(new
            {
                model.NombreProyecto,
                model.Descripcion,
                model.AreaSolicitante,
                model.Justificacion,
                model.PresupuestoSolicitado
            });

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarPropuestaAsync(int idPropuesta)
        {
            using var request = new HttpRequestMessage(HttpMethod.Delete, $"api/propuestas/{idPropuesta}");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<PropuestaListItemViewModel>> ListarPropuestasPendientesAprobacionAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/aprobaciones");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<PropuestaListItemViewModel>();
            }

            var propuestas = await response.Content.ReadFromJsonAsync<List<PropuestaListItemViewModel>>(JsonOptions);

            return propuestas ?? new List<PropuestaListItemViewModel>();
        }

        public async Task<AprobacionFormViewModel?> ObtenerPropuestaAprobacionAsync(int idPropuesta)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"api/aprobaciones/{idPropuesta}");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var propuesta = await response.Content.ReadFromJsonAsync<AprobacionFormViewModel>(JsonOptions);

            if (propuesta != null)
            {
                propuesta.PresupuestoAprobado = propuesta.PresupuestoSolicitado;
            }

            return propuesta;
        }

        public async Task<bool> AprobarPropuestaAsync(AprobacionFormViewModel model)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/aprobaciones/{model.IdPropuesta}/aprobar");

            AgregarHeadersSesion(request);

            request.Content = JsonContent.Create(new
            {
                model.PresupuestoAprobado,
                model.ComentarioRevision
            });

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RechazarPropuestaAsync(AprobacionFormViewModel model)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/aprobaciones/{model.IdPropuesta}/rechazar");

            AgregarHeadersSesion(request);

            request.Content = JsonContent.Create(new
            {
                PresupuestoAprobado = 0,
                model.ComentarioRevision
            });

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<PropuestaListItemViewModel>> ListarProyectosAprobadosAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/completarproyecto");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new List<PropuestaListItemViewModel>();
            }

            var propuestas = await response.Content.ReadFromJsonAsync<List<PropuestaListItemViewModel>>(JsonOptions);

            return propuestas ?? new List<PropuestaListItemViewModel>();
        }

        public async Task<bool> CompletarProyectoAsync(int idPropuesta)
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"api/completarproyecto/{idPropuesta}/completar");

            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        public async Task<ReporteDashboardViewModel> ObtenerReporteDashboardAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "api/reportes/dashboard");
            AgregarHeadersSesion(request);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return new ReporteDashboardViewModel();
            }

            var reporte = await response.Content.ReadFromJsonAsync<ReporteDashboardViewModel>(JsonOptions);

            return reporte ?? new ReporteDashboardViewModel();
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
