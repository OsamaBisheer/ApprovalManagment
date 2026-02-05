using System.ComponentModel.DataAnnotations;

namespace ApprovalManagment.Domain.ViewModels.LeaveType
{
    public class LeaveTypeResultVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DefaultYearlyBalance { get; set; }
    }
}