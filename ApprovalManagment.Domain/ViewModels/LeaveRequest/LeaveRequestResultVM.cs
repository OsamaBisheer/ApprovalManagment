using System.ComponentModel.DataAnnotations;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.ViewModels.LeaveRequest
{
    public class LeaveRequestResultVM
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatusEnum LeaveStatus { get; set; }
    }
}