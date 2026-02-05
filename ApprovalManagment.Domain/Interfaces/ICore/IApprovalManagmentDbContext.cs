using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ApprovalManagment.Domain.Entities;

namespace ApprovalManagment.Domain.Interfaces.ICore
{
    public interface IApprovalManagmentDbContext : IDisposable
    {
        DbSet<LeaveRequest> LeaveRequests { get; set; }
        DbSet<LeaveType> LeaveTypes { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}