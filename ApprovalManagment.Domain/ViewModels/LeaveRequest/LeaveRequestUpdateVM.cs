using ApprovalManagment.Domain.ViewModels.Common;
using System.ComponentModel.DataAnnotations;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.ViewModels.LeaveRequest
{
    public class LeaveRequestUpdateVM : AuditableVM
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
    }
}