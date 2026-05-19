using PrototipoVW.Models;

namespace PrototipoVW.Authorization
{
    public class AdminAttribute : SessionAuthorizeAttribute
    {
        public AdminAttribute()
            : base(UserRole.Admin)
        {
        }
    }
}
