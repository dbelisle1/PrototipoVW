using PrototipoVW.Models;

namespace PrototipoVW.Authorization
{
    public class SupervisorAttribute : SessionAuthorizeAttribute
    {
        public SupervisorAttribute()
            : base(UserRole.Supervisor)
        {
        }
    }
}
