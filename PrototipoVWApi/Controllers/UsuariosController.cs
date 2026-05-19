using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Authorization;
using PrototipoVWApi.Data;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Admin]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosRepository _usuariosRepository;

        public UsuariosController(UsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var usuarios = await _usuariosRepository.ListarAsync();
            return Ok(usuarios);
        }

        [HttpGet("{idUsuario:int}")]
        public async Task<IActionResult> ObtenerPorId(int idUsuario)
        {
            var usuario = await _usuariosRepository.ObtenerPorIdAsync(idUsuario);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] UsuarioCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NombreCompleto) ||
                string.IsNullOrWhiteSpace(request.Correo) ||
                string.IsNullOrWhiteSpace(request.Contrasena) ||
                string.IsNullOrWhiteSpace(request.Rol))
            {
                return BadRequest(new { mensaje = "Todos los campos son obligatorios." });
            }

            var idUsuario = await _usuariosRepository.CrearAsync(request);

            return Ok(new
            {
                mensaje = "Usuario creado correctamente.",
                idUsuario
            });
        }

        [HttpPut("{idUsuario:int}")]
        public async Task<IActionResult> Actualizar(
            int idUsuario,
            [FromBody] UsuarioUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.NombreCompleto) ||
                string.IsNullOrWhiteSpace(request.Correo) ||
                string.IsNullOrWhiteSpace(request.Rol))
            {
                return BadRequest(new { mensaje = "Nombre, correo y rol son obligatorios." });
            }

            await _usuariosRepository.ActualizarAsync(idUsuario, request);

            return Ok(new
            {
                mensaje = "Usuario actualizado correctamente."
            });
        }

        [HttpDelete("{idUsuario:int}")]
        public async Task<IActionResult> Eliminar(int idUsuario)
        {
            await _usuariosRepository.EliminarAsync(idUsuario);

            return Ok(new
            {
                mensaje = "Usuario eliminado correctamente."
            });
        }
    }
}
