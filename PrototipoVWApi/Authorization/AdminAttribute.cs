using PrototipoVWApi.Models;

namespace PrototipoVWApi.Authorization
{
    public class AdminAttribute : RoleAuthorizeAttribute
    {
        public AdminAttribute()
            : base(UserRole.Admin)
        {
        }
    }
}
