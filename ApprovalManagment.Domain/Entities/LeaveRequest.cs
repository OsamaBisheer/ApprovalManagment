using ApprovalManagment.Domain.Entities.Common;
using ApprovalManagment.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.Entities
{
    public class LeaveRequest : AuditableEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }

        [Column(TypeName = "tinyint")]
        public LeaveStatusEnum LeaveStatus { get; set; }

        public int LeaveTypeId { get; set; }

        [ForeignKey("LeaveTypeId")]
        public LeaveType LeaveType { get; set; }

        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }

        public List<LeaveStatusHistory> LeaveStatusHistories { get; set; }
    }
}