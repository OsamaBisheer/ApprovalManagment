using ApprovalManagment.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.Entities
{
    public class LeaveStatusHistory : AuditableEntity
    {
        [Column(TypeName = "tinyint")]
        public LeaveStatusEnum From { get; set; }

        [Column(TypeName = "tinyint")]
        public LeaveStatusEnum To { get; set; }

        public int LeaveRequestId { get; set; }

        [ForeignKey("LeaveRequestId")]
        public LeaveRequest LeaveRequest { get; set; }
    }
}