using PrototipoVW.Models;

namespace PrototipoVW.Authorization
{
    public class GeneralAttribute : SessionAuthorizeAttribute
    {
        public GeneralAttribute()
            : base(UserRole.Admin, UserRole.Empleado, UserRole.Supervisor)
        {
        }
    }
}
