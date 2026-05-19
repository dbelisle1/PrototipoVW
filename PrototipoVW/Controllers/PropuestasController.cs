using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Authorization;
using PrototipoVW.Models;
using PrototipoVW.Services;

namespace PrototipoVW.Controllers;

[General]
public class PropuestasController : Controller
{
    private readonly ApiClient _apiClient;

    public PropuestasController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Rol = HttpContext.Session.GetString("Rol");
        ViewBag.IdUsuario = HttpContext.Session.GetInt32("IdUsuario");

        var propuestas = await _apiClient.ListarPropuestasAsync();

        return View(propuestas);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "Admin" && rol != "Empleado")
        {
            return RedirectToAction("InvalidRoute", "Error");
        }

        return View("Form", new PropuestaFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PropuestaFormViewModel model)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "Admin" && rol != "Empleado")
        {
            return RedirectToAction("InvalidRoute", "Error");
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        var creado = await _apiClient.CrearPropuestaAsync(model);

        if (!creado)
        {
            ModelState.AddModelError("", "No se pudo crear la propuesta.");
            return View("Form", model);
        }

        TempData["Mensaje"] = "Propuesta creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "Admin" && rol != "Empleado")
        {
            return RedirectToAction("InvalidRoute", "Error");
        }

        var propuesta = await _apiClient.ObtenerPropuestaPorIdAsync(id);

        if (propuesta == null)
        {
            TempData["Error"] = "Propuesta no encontrada o sin permisos.";
            return RedirectToAction(nameof(Index));
        }

        return View("Form", propuesta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PropuestaFormViewModel model)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "Admin" && rol != "Empleado")
        {
            return RedirectToAction("InvalidRoute", "Error");
        }

        if (!ModelState.IsValid)
        {
            return View("Form", model);
        }

        var actualizado = await _apiClient.ActualizarPropuestaAsync(model);

        if (!actualizado)
        {
            ModelState.AddModelError("", "No se pudo actualizar la propuesta. Solo se pueden modificar propuestas pendientes.");
            return View("Form", model);
        }

        TempData["Mensaje"] = "Propuesta actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Supervisor]
    public async Task<IActionResult> Ver(int id)
    {
        var propuesta = await _apiClient.ObtenerPropuestaPorIdAsync(id);

        if (propuesta == null)
        {
            TempData["Error"] = "Propuesta no encontrada.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.SoloLectura = true;

        return View("Form", propuesta);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "Admin" && rol != "Empleado")
        {
            return RedirectToAction("InvalidRoute", "Error");
        }

        var eliminado = await _apiClient.EliminarPropuestaAsync(id);

        if (!eliminado)
        {
            TempData["Error"] = "No se pudo eliminar la propuesta. Solo se pueden eliminar propuestas pendientes.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Mensaje"] = "Propuesta eliminada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}