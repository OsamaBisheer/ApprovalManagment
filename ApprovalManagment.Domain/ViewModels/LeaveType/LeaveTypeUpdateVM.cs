using System.ComponentModel.DataAnnotations;
using ApprovalManagment.Domain.ViewModels.Common;

namespace ApprovalManagment.Domain.ViewModels.LeaveType
{
    public class LeaveTypeUpdateVM : AuditableVM
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, double.MaxValue)]
        public int DefaultYearlyBalance { get; set; }
    }
}