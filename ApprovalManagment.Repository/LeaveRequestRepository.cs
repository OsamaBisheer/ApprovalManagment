using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Repository.Common;
using ApprovalManagment.Repository.Helpers;

namespace ApprovalManagment.Repository
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(IApprovalManagmentDbContext context) : base(context)
        {
        }

        public async Task<DataTableResponseVM<LeaveRequest>> Search(DataTableRequestVM requestVM)
        {
            var search = string.IsNullOrEmpty(requestVM.Search) ? string.Empty : requestVM.Search.ToLower();

            var result = GetAll().OrderByDescending(w => w.Id)
                .Where(w => w.Id.ToString() == search || string.IsNullOrEmpty(search))
                .Select(w => new LeaveRequest
                {
                    Id = w.Id,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Reason = w.Reason,
                    LeaveStatus = w.LeaveStatus
                });

            int totalRecords = result.Count();

            result = result.OrderByDynamic(requestVM.OrderColumn, requestVM.OrderDir)
                .Skip(requestVM.PageNumber * requestVM.PageSize)
                .Take(requestVM.PageSize);

            return await result.ToDataTableResult(totalRecords);
        }
    }
}