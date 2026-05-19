using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Data;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthRepository _authRepository;

        public AuthController(AuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Correo) ||
                string.IsNullOrWhiteSpace(request.Contrasena))
            {
                return BadRequest(new
                {
                    mensaje = "Debe ingresar correo y contraseña."
                });
            }

            var usuario = await _authRepository.ValidarLoginAsync(request);

            if (usuario == null)
            {
                return Unauthorized(new
                {
                    mensaje = "Credenciales inválidas."
                });
            }

            return Ok(usuario);
        }
    }
}
