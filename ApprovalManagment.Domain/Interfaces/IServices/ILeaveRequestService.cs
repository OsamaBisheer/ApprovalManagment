using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveRequest;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.Interfaces.IServices
{
    public interface ILeaveRequestService
    {
        Task<Tuple<int, ResponseCodeEnum>> Create(LeaveRequestCreateVM workflowVM);

        Task<Tuple<int, ResponseCodeEnum>> Update(LeaveRequestUpdateVM workflowVM);

        Task<Tuple<DataTableResponseVM<LeaveRequestResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM workflowVM);

        Task<Tuple<LeaveRequestResultVM, ResponseCodeEnum>> GetById(int id);

        Task<Tuple<int, ResponseCodeEnum>> ChangeStatus(LeaveRequestChangeStatusVM model);
    }
}