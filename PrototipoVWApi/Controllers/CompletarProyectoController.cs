using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Authorization;
using PrototipoVWApi.Data;

namespace PrototipoVWApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AdminSupervisor]
public class CompletarProyectoController : ControllerBase
{
    private readonly PropuestasRepository _propuestasRepository;

    public CompletarProyectoController(PropuestasRepository propuestasRepository)
    {
        _propuestasRepository = propuestasRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListarAprobadas()
    {
        var propuestas = await _propuestasRepository.ListarAsync("Aprobado", null);
        return Ok(propuestas);
    }

    [HttpPost("{idPropuesta:int}/completar")]
    public async Task<IActionResult> Completar(int idPropuesta)
    {
        var idUsuarioCompletado = ObtenerIdUsuario();

        var propuesta = await _propuestasRepository.ObtenerPorIdAsync(idPropuesta);

        if (propuesta == null)
        {
            return NotFound(new { mensaje = "Propuesta no encontrada." });
        }

        if (propuesta.Estado != "Aprobado")
        {
            return BadRequest(new { mensaje = "Solo se pueden completar proyectos aprobados." });
        }

        await _propuestasRepository.CompletarAsync(idPropuesta, idUsuarioCompletado);

        return Ok(new
        {
            mensaje = "Proyecto completado correctamente."
        });
    }

    private int ObtenerIdUsuario()
    {
        return int.Parse(Request.Headers["X-User-Id"].ToString());
    }
}