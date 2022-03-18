using Microsoft.AspNetCore.Identity;

namespace ClassPort.Domain.Entities.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
