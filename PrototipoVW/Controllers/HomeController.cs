using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Models;
using System.Diagnostics;
using PrototipoVW.Authorization;

namespace PrototipoVW.Controllers
{
    public class HomeController : Controller
    {
        [General]
        public IActionResult Index()
        {
            ViewBag.Rol = HttpContext.Session.GetString("Rol");
            ViewBag.NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            return View();
        }
    }
}
