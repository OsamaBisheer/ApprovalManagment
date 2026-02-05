using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Persistence
{
    public class ApprovalManagmentDbContext : IdentityDbContext<User, ApplicationRole, string, IdentityUserClaim<string>,
    ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>, IApprovalManagmentDbContext
    {
        public ApprovalManagmentDbContext(DbContextOptions<ApprovalManagmentDbContext> options) : base(options)
        { }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole
                {
                    Id = "1",
                    Name = nameof(RoleEnum.Admin),
                    NormalizedName = nameof(RoleEnum.Admin)
                },
                new ApplicationRole
                {
                    Id = "2",
                    Name = nameof(RoleEnum.Manager),
                    NormalizedName = nameof(RoleEnum.Manager)
                },
                new ApplicationRole
                {
                    Id = "3",
                    Name = nameof(RoleEnum.Employee),
                    NormalizedName = nameof(RoleEnum.Employee)
                }
            );
        }
    }
}