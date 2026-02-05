using System.ComponentModel.DataAnnotations;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.ViewModels.User
{
    public class UserUpdateVM
    {
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string ManagerId { get; set; }

        [Required]
        public DepartmentEnum Department { get; set; }

        public string PhoneNumber { get; set; }

        public int RemainingLeaveBalance { get; set; }
    }
}