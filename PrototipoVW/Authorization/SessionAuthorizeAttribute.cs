using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PrototipoVW.Models;

namespace PrototipoVW.Authorization
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly UserRole[] _allowedRoles;

        public SessionAuthorizeAttribute(params UserRole[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;

            var idUsuario = session.GetInt32("IdUsuario");
            var rolTexto = session.GetString("Rol");

            if (idUsuario == null || string.IsNullOrWhiteSpace(rolTexto))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (!Enum.TryParse<UserRole>(rolTexto, true, out var rol))
            {
                session.Clear();
                context.Result = new RedirectToActionResult("InvalidRoute", "Error", null);
                return;
            }

            if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(rol))
            {
                session.Clear();
                context.Result = new RedirectToActionResult("InvalidRoute", "Error", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
