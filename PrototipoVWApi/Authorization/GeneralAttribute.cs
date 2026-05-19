using PrototipoVWApi.Models;

namespace PrototipoVWApi.Authorization
{
    public class GeneralAttribute : RoleAuthorizeAttribute
    {
        public GeneralAttribute()
            : base(UserRole.Admin, UserRole.Empleado, UserRole.Supervisor)
        {
        }
    }
}
