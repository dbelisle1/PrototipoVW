using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Authorization;
using PrototipoVW.Services;

namespace PrototipoVW.Controllers;

[AdminSupervisor]
public class ReportesController : Controller
{
    private readonly ApiClient _apiClient;

    public ReportesController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var reporte = await _apiClient.ObtenerReporteDashboardAsync();
        return View(reporte);
    }
}