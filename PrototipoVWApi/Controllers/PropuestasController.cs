using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Authorization;
using PrototipoVWApi.Data;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[General]
public class PropuestasController : ControllerBase
{
    private readonly PropuestasRepository _propuestasRepository;

    public PropuestasController(PropuestasRepository propuestasRepository)
    {
        _propuestasRepository = propuestasRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var idUsuario = ObtenerIdUsuario();
        var rol = ObtenerRol();

        int? idUsuarioCreador = rol == UserRole.Empleado ? idUsuario : null;

        var propuestas = await _propuestasRepository.ListarAsync(null, idUsuarioCreador);

        return Ok(propuestas);
    }

    [HttpGet("{idPropuesta:int}")]
    public async Task<IActionResult> ObtenerPorId(int idPropuesta)
    {
        var idUsuario = ObtenerIdUsuario();
        var rol = ObtenerRol();

        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (rol == UserRole.Empleado && propuesta.IdCreador != idUsuario)
        {
            return Forbid();
        }

        return Ok(propuesta);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] PropuestaCreateRequest request)
    {
        var idUsuario = ObtenerIdUsuario();
        var rol = ObtenerRol();

        if (rol != UserRole.Admin && rol != UserRole.Empleado)
        {
            return Forbid();
        }

        if (string.IsNullOrWhiteSpace(request.NombreProyecto) ||
            string.IsNullOrWhiteSpace(request.AreaSolicitante) ||
            request.PresupuestoSolicitado < 0)
        {
            return BadRequest(new { mensaje = "Datos inválidos para crear la propuesta." });
        }

        var idPropuesta = await _propuestasRepository.CrearAsync(idUsuario, request);

        return Ok(new
        {
            mensaje = "Propuesta creada correctamente.",
            idPropuesta
        });
    }

    [HttpPut("{idPropuesta:int}")]
    public async Task<IActionResult> Actualizar(
        int idPropuesta,
        [FromBody] PropuestaUpdateRequest request)
    {
        var idUsuario = ObtenerIdUsuario();
        var rol = ObtenerRol();

        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (propuesta.Estado != "Pendiente")
        {
            return BadRequest(new { mensaje = "Solo se pueden modificar propuestas pendientes." });
        }

        var puedeModificar =
            rol == UserRole.Admin ||
            rol == UserRole.Empleado && propuesta.IdCreador == idUsuario;

        if (!puedeModificar)
        {
            return Forbid();
        }

        await _propuestasRepository.ActualizarAsync(idPropuesta, request);

        return Ok(new
        {
            mensaje = "Propuesta actualizada correctamente."
        });
    }

    [HttpDelete("{idPropuesta:int}")]
    public async Task<IActionResult> Eliminar(int idPropuesta)
    {
        var idUsuario = ObtenerIdUsuario();
        var rol = ObtenerRol();

        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (propuesta.Estado != "Pendiente")
        {
            return BadRequest(new { mensaje = "Solo se pueden eliminar propuestas pendientes." });
        }

        var puedeEliminar =
            rol == UserRole.Admin ||
            rol == UserRole.Empleado && propuesta.IdCreador == idUsuario;

        if (!puedeEliminar)
        {
            return Forbid();
        }

        await _propuestasRepository.EliminarAsync(idPropuesta);

        return Ok(new
        {
            mensaje = "Propuesta eliminada correctamente."
        });
    }

    private int ObtenerIdUsuario()
    {
        return int.Parse(Request.Headers["X-User-Id"].ToString());
    }

    private UserRole ObtenerRol()
    {
        return Enum.Parse<UserRole>(Request.Headers["X-User-Role"].ToString(), true);
    }
}