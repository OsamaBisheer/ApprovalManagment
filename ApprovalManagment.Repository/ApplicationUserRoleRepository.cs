using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;
using ApprovalManagment.Repository.Common;

namespace ApprovalManagment.Repository
{
    public class ApplicationUserRoleRepository : GenericRepository<ApplicationUserRole>, IApplicationUserRoleRepository
    {
        public ApplicationUserRoleRepository(IApprovalManagmentDbContext context) : base(context)
        {
        }
    }
}