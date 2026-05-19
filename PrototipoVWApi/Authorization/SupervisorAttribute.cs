using PrototipoVWApi.Models;

namespace PrototipoVWApi.Authorization
{
    public class SupervisorAttribute : RoleAuthorizeAttribute
    {
        public SupervisorAttribute()
            : base(UserRole.Supervisor)
        {
        }
    }
}
