using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.ViewModels.Common;

namespace ApprovalManagment.Domain.Interfaces.IRepositories
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
        Task<DataTableResponseVM<LeaveType>> Search(DataTableRequestVM requestVM);
    }
}