using ApprovalManagment.Domain.Entities.Common;
using ApprovalManagment.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApprovalManagment.Domain.Entities
{
    public class LeaveType : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int DefaultYearlyBalance { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }
    }
}