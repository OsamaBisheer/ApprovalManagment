using Microsoft.AspNetCore.Identity;

namespace ApprovalManagment.Domain.Entities.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public List<ApplicationUserRole> UserRoles { get; set; }
    }
}