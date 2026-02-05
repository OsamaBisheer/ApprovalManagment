using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public List<ApplicationUserRole> UserRoles { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }
        public string ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        public User Manager { get; set; }

        [Column(TypeName = "tinyint")]
        public DepartmentEnum Department { get; set; }

        public int RemainingLeaveBalance { get; set; }
    }
}