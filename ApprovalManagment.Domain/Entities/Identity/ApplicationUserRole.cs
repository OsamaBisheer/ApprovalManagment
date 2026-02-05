using Microsoft.AspNetCore.Identity;

namespace ApprovalManagment.Domain.Entities.Identity
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public User User { get; set; }
        public ApplicationRole Role { get; set; }
    }
}