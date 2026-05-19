using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using PrototipoVWApi.Models;

namespace PrototipoVWApi.Authorization
{
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly UserRole[] _allowedRoles;

        public RoleAuthorizeAttribute(params UserRole[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (!request.Headers.TryGetValue("X-User-Id", out var userIdValue) ||
                !request.Headers.TryGetValue("X-User-Role", out var roleValue))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    mensaje = "No existe sesión activa."
                });

                return;
            }

            if (!int.TryParse(userIdValue.ToString(), out var idUsuario) || idUsuario <= 0)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    mensaje = "Usuario inválido."
                });

                return;
            }

            if (!Enum.TryParse<UserRole>(roleValue.ToString(), true, out var userRole))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    mensaje = "Rol inválido."
                });

                return;
            }

            if (_allowedRoles.Length > 0 && !_allowedRoles.Contains(userRole))
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
