using PrototipoVWApi.Models;

namespace PrototipoVWApi.Authorization
{
    public class EmpleadoAttribute : RoleAuthorizeAttribute
    {
        public EmpleadoAttribute()
            : base(UserRole.Empleado)
        {
        }
    }
}
