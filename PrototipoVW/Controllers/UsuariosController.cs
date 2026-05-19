using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Models;
using PrototipoVW.Services;
using PrototipoVW.Authorization;

namespace PrototipoVW.Controllers
{
    [Admin]
    public class UsuariosController : Controller
    {
        private readonly ApiClient _apiClient;

        public UsuariosController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _apiClient.ListarUsuariosAsync();
            return View(usuarios);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form", new UsuarioFormViewModel
            {
                Rol = "Empleado"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioFormViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Contrasena))
            {
                ModelState.AddModelError("Contrasena", "Ingrese la contraseña.");
            }

            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            var creado = await _apiClient.CrearUsuarioAsync(model);

            if (!creado)
            {
                ModelState.AddModelError("", "No se pudo crear el usuario. Verifique que el correo no esté repetido.");
                return View("Form", model);
            }

            TempData["Mensaje"] = "Usuario creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _apiClient.ObtenerUsuarioPorIdAsync(id);

            if (usuario == null)
            {
                TempData["Error"] = "Usuario no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            usuario.Contrasena = "";

            return View("Form", usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UsuarioFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", model);
            }

            var actualizado = await _apiClient.ActualizarUsuarioAsync(model);

            if (!actualizado)
            {
                ModelState.AddModelError("", "No se pudo actualizar el usuario.");
                return View("Form", model);
            }

            TempData["Mensaje"] = "Usuario actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _apiClient.EliminarUsuarioAsync(id);

            if (!eliminado)
            {
                TempData["Error"] = "No se pudo eliminar el usuario.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Mensaje"] = "Usuario eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
