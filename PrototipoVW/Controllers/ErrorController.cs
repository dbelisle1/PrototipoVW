using Microsoft.AspNetCore.Mvc;

namespace PrototipoVW.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult InvalidRoute()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }
}
