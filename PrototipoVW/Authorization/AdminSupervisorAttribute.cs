using PrototipoVW.Models;

namespace PrototipoVW.Authorization;

public class AdminSupervisorAttribute : SessionAuthorizeAttribute
{
    public AdminSupervisorAttribute()
        : base(UserRole.Admin, UserRole.Supervisor)
    {
    }
}