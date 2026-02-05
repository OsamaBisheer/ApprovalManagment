using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;
using ApprovalManagment.Repository.Common;

namespace ApprovalManagment.Repository
{
    public class ApplicationRoleRepository : GenericRepository<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(IApprovalManagmentDbContext context) : base(context)
        {
        }
    }
}