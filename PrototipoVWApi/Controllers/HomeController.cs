using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Authorization;

namespace PrototipoVWApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [General]
        public IActionResult Index()
        {
            var rol = Request.Headers["X-User-Role"].ToString();

            return Ok(new
            {
                mensaje = $"Estas en el API como {rol}"
            });
        }
    }
}
