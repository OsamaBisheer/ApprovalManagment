using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.Interfaces.IServices
{
    public interface ILeaveTypeService
    {
        Task<Tuple<int, ResponseCodeEnum>> Create(LeaveTypeCreateVM workflowVM);

        Task<Tuple<int, ResponseCodeEnum>> Update(LeaveTypeUpdateVM workflowVM);

        Task<Tuple<DataTableResponseVM<LeaveTypeResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM workflowVM);

        Task<Tuple<LeaveTypeResultVM, ResponseCodeEnum>> GetById(int id);

        Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup();

        Task<ResponseCodeEnum> Delete(int id, string lastUpdatedByUserId);
    }
}