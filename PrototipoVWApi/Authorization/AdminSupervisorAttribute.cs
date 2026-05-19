using PrototipoVWApi.Models;

namespace PrototipoVWApi.Authorization;

public class AdminSupervisorAttribute : RoleAuthorizeAttribute
{
    public AdminSupervisorAttribute()
        : base(UserRole.Admin, UserRole.Supervisor)
    {
    }
}