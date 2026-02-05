using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.IRepositories;

namespace ApprovalManagment.Domain.Interfaces.ICore
{
    public interface IUnitOfWork : IDisposable
    {
        ILeaveRequestRepository LeaveRequests { get; }
        ILeaveTypeRepository LeaveTypes { get; }
        IUserRepository Users { get; }
        IApplicationUserRoleRepository ApplicationUserRoles { get; }
        IApplicationRoleRepository ApplicationRoles { get; }

        IApprovalManagmentDbContext Context { get; }

        Task<int> Commit();
    }
}