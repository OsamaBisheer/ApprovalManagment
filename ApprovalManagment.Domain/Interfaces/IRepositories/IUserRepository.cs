using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.ViewModels.Common;

namespace ApprovalManagment.Domain.Interfaces.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<DataTableResponseVM<User>> Search(DataTableRequestVM requestVM);
    }
}