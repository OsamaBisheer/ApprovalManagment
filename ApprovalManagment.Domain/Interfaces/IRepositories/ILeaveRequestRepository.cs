using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.ViewModels.Common;

namespace ApprovalManagment.Domain.Interfaces.IRepositories
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        Task<DataTableResponseVM<LeaveRequest>> Search(DataTableRequestVM requestVM);
    }
}