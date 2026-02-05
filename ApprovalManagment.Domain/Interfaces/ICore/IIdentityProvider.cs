using ApprovalManagment.Domain.Entities.Identity;

namespace ApprovalManagment.Domain.Interfaces.ICore
{
    public interface IIdentityProvider
    {
        User GetUser();
    }
}