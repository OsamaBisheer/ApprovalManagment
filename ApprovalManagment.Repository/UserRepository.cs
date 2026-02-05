using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Repository.Common;
using ApprovalManagment.Repository.Helpers;

namespace ApprovalManagment.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IApprovalManagmentDbContext context) : base(context)
        {
        }

        public async Task<DataTableResponseVM<User>> Search(DataTableRequestVM requestVM)
        {
            var search = string.IsNullOrEmpty(requestVM.Search) ? string.Empty : requestVM.Search.ToLower();

            var result = GetAll().OrderByDescending(w => w.Id)
                .Where(w => w.Id.ToString() == search || w.Email.ToLower().Contains(search))
                .Select(w => new User
                {
                    Id = w.Id,
                    UserName = w.UserName,
                    Email = w.Email,
                    PhoneNumber = w.PhoneNumber,
                    Department = w.Department,
                });

            int totalRecords = result.Count();

            result = result.OrderByDynamic(requestVM.OrderColumn, requestVM.OrderDir)
                .Skip(requestVM.PageNumber * requestVM.PageSize)
                .Take(requestVM.PageSize);

            return await result.ToDataTableResult(totalRecords);
        }
    }
}