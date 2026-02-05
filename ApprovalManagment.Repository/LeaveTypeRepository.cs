using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Repository.Common;
using ApprovalManagment.Repository.Helpers;

namespace ApprovalManagment.Repository
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        public LeaveTypeRepository(IApprovalManagmentDbContext context) : base(context)
        {
        }

        public async Task<DataTableResponseVM<LeaveType>> Search(DataTableRequestVM requestVM)
        {
            var search = string.IsNullOrEmpty(requestVM.Search) ? string.Empty : requestVM.Search.ToLower();

            var result = GetAll().OrderByDescending(w => w.Id)
                .Where(w => w.Id.ToString() == search || w.Name.ToLower().Contains(search))
                .Select(w => new LeaveType
                {
                    Id = w.Id,
                    Name = w.Name,
                    Description = w.Description,
                    DefaultYearlyBalance = w.DefaultYearlyBalance
                });

            int totalRecords = result.Count();

            result = result.OrderByDynamic(requestVM.OrderColumn, requestVM.OrderDir)
                .Skip(requestVM.PageNumber * requestVM.PageSize)
                .Take(requestVM.PageSize);

            return await result.ToDataTableResult(totalRecords);
        }
    }
}