using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Authorization;
using PrototipoVW.Services;

namespace PrototipoVW.Controllers;

[AdminSupervisor]
public class CompletarProyectoController : Controller
{
    private readonly ApiClient _apiClient;

    public CompletarProyectoController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var propuestas = await _apiClient.ListarProyectosAprobadosAsync();
        return View(propuestas);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Completar(int id)
    {
        var completado = await _apiClient.CompletarProyectoAsync(id);

        if (!completado)
        {
            TempData["Error"] = "No se pudo completar el proyecto.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Mensaje"] = "Proyecto completado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}