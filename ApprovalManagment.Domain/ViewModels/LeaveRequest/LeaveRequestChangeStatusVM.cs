using ApprovalManagment.Domain.ViewModels.Common;
using System.ComponentModel.DataAnnotations;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.ViewModels.LeaveRequest
{
    public class LeaveRequestChangeStatusVM : AuditableVM
    {
        public int Id { get; set; }
        public LeaveStatusEnum LeaveStatus { get; set; }
    }
}