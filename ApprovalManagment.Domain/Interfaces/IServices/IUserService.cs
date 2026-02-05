using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using ApprovalManagment.Domain.ViewModels.User;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.Interfaces.IServices
{
    public interface IUserService
    {
        //Task<Tuple<int, ResponseCodeEnum>> Create(UserAddVM workflowVM);

        Task<Tuple<string, ResponseCodeEnum>> Update(UserUpdateVM workflowVM);

        Task<Tuple<DataTableResponseVM<UserResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM workflowVM);

        Task<Tuple<UserResultVM, ResponseCodeEnum>> GetById(string id);

        Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup();

        Task<Tuple<int, ResponseCodeEnum>> GetCount(RoleEnum role);

        Task<Tuple<string, ResponseCodeEnum>> GetRoleId(RoleEnum role);

        Task<ResponseCodeEnum> AddUserRoles(string userId, List<string> rolesIds);

        //Task<ResponseCodeEnum> Delete(int id, string lastUpdatedByUserId);
    }
}