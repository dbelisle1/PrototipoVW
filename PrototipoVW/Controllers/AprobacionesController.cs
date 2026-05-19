using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Authorization;
using PrototipoVW.Models;
using PrototipoVW.Services;

namespace PrototipoVW.Controllers;

[AdminSupervisor]
public class AprobacionesController : Controller
{
    private readonly ApiClient _apiClient;

    public AprobacionesController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var propuestas = await _apiClient.ListarPropuestasPendientesAprobacionAsync();
        return View(propuestas);
    }

    [HttpGet]
    public async Task<IActionResult> Aprobar(int id)
    {
        var propuesta = await _apiClient.ObtenerPropuestaAprobacionAsync(id);

        if (propuesta == null)
        {
            TempData["Error"] = "Propuesta no encontrada o ya no está pendiente.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Accion = "Aprobar";

        return View("Form", propuesta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Aprobar(AprobacionFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Accion = "Aprobar";
            return View("Form", model);
        }

        var aprobado = await _apiClient.AprobarPropuestaAsync(model);

        if (!aprobado)
        {
            ModelState.AddModelError("", "No se pudo aprobar la propuesta.");
            ViewBag.Accion = "Aprobar";
            return View("Form", model);
        }

        TempData["Mensaje"] = "Propuesta aprobada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Rechazar(int id)
    {
        var propuesta = await _apiClient.ObtenerPropuestaAprobacionAsync(id);

        if (propuesta == null)
        {
            TempData["Error"] = "Propuesta no encontrada o ya no está pendiente.";
            return RedirectToAction(nameof(Index));
        }

        propuesta.PresupuestoAprobado = 0;
        ViewBag.Accion = "Rechazar";

        return View("Form", propuesta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rechazar(AprobacionFormViewModel model)
    {
        var rechazado = await _apiClient.RechazarPropuestaAsync(model);

        if (!rechazado)
        {
            ModelState.AddModelError("", "No se pudo rechazar la propuesta.");
            ViewBag.Accion = "Rechazar";
            return View("Form", model);
        }

        TempData["Mensaje"] = "Propuesta rechazada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}