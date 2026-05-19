using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Authorization;
using PrototipoVWApi.Data;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AdminSupervisor]
public class AprobacionesController : ControllerBase
{
    private readonly PropuestasRepository _propuestasRepository;

    public AprobacionesController(PropuestasRepository propuestasRepository)
    {
        _propuestasRepository = propuestasRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListarPendientes()
    {
        var propuestas = await _propuestasRepository.ListarAsync("Pendiente", null);
        return Ok(propuestas);
    }

    [HttpGet("{idPropuesta:int}")]
    public async Task<IActionResult> ObtenerPorId(int idPropuesta)
    {
        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (propuesta.Estado != "Pendiente")
        {
            return BadRequest(new { mensaje = "Solo se pueden revisar propuestas pendientes." });
        }

        return Ok(propuesta);
    }

    [HttpPost("{idPropuesta:int}/aprobar")]
    public async Task<IActionResult> Aprobar(
        int idPropuesta,
        [FromBody] AprobacionRequest request)
    {
        var idUsuarioRevisor = ObtenerIdUsuario();

        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (propuesta.Estado != "Pendiente")
        {
            return BadRequest(new { mensaje = "Solo se pueden aprobar propuestas pendientes." });
        }

        if (request.PresupuestoAprobado < 0)
        {
            return BadRequest(new { mensaje = "El presupuesto aprobado no puede ser negativo." });
        }

        await _propuestasRepository.AprobarAsync(
            idPropuesta,
            idUsuarioRevisor,
            request.PresupuestoAprobado,
            request.ComentarioRevision
        );

        return Ok(new { mensaje = "Propuesta aprobada correctamente." });
    }

    [HttpPost("{idPropuesta:int}/rechazar")]
    public async Task<IActionResult> Rechazar(
        int idPropuesta,
        [FromBody] AprobacionRequest request)
    {
        var idUsuarioRevisor = ObtenerIdUsuario();

        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (propuesta.Estado != "Pendiente")
        {
            return BadRequest(new { mensaje = "Solo se pueden rechazar propuestas pendientes." });
        }

        await _propuestasRepository.RechazarAsync(
            idPropuesta,
            idUsuarioRevisor,
            request.ComentarioRevision
        );

        return Ok(new { mensaje = "Propuesta rechazada correctamente." });
    }

    private int ObtenerIdUsuario()
    {
        return int.Parse(Request.Headers["X-User-Id"].ToString());
    }
}