using PrototipoVW.Models;

namespace PrototipoVW.Authorization
{
    public class EmpleadoAttribute : SessionAuthorizeAttribute
    {
        public EmpleadoAttribute()
            : base(UserRole.Empleado)
        {
        }
    }
}
