using ApprovalManagment.API.Providers;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IRepositories;
using ApprovalManagment.Persistence;
using ApprovalManagment.Repository.Common;
using ApprovalManagment.Service;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.API.JWT;
using ApprovalManagment.Repository;

namespace ApprovalManagment.API.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDIs(this IServiceCollection services)
        {
            services.AddScoped<IApprovalManagmentDbContext, ApprovalManagmentDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityProvider, IdentityProvider>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<ILeaveTypeService, LeaveTypeService>();
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();

            services.AddHttpClient<UserService>();

            services.AddScoped<RevokableJwtSecurityTokenHandler>();
            services.AddScoped<JwtHandlerEvents>();

            return services;
        }
    }
}