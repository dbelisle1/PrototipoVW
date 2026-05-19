using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Authorization;
using PrototipoVWApi.Data;

namespace PrototipoVWApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AdminSupervisor]
public class ReportesController : ControllerBase
{
    private readonly ReportesRepository _reportesRepository;

    public ReportesController(ReportesRepository reportesRepository)
    {
        _reportesRepository = reportesRepository;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var reporte = await _reportesRepository.ObtenerDashboardAsync();
        return Ok(reporte);
    }
}