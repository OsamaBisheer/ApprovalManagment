using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;

namespace ApprovalManagment.Repository.Common
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public IApprovalManagmentDbContext Context { get; }
        public IUserRepository Users { get; private set; }
        public ILeaveRequestRepository LeaveRequests { get; private set; }
        public ILeaveTypeRepository LeaveTypes { get; private set; }
        public IApplicationUserRoleRepository ApplicationUserRoles { get; private set; }
        public IApplicationRoleRepository ApplicationRoles { get; private set; }

        public UnitOfWork(IApprovalManagmentDbContext _context)
        {
            Context = _context;

            Users = new UserRepository(_context);
            LeaveRequests = new LeaveRequestRepository(_context);
            LeaveTypes = new LeaveTypeRepository(_context);
            ApplicationUserRoles = new ApplicationUserRoleRepository(_context);
            ApplicationRoles = new ApplicationRoleRepository(_context);
        }

        /// <summary>
        /// Saves all pending changes
        /// </summary>
        /// <returns>The number of objects in an Added, Modified, or Deleted state</returns>
        public async Task<int> Commit()
        {
            // Save changes with the default options
            return await Context.SaveChangesAsync();
        }

        /// <summary>
        /// Disposes the current object
        /// </summary>
        public void Dispose()
        {
            Context.Dispose();
        }
    }
}