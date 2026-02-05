using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.ViewModels.User
{
    public class UserResultVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DepartmentEnum Department { get; set; }
        public int RemainingLeaveBalance { get; set; }
    }
}