using AutoMapper;
using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using ApprovalManagment.Domain.ViewModels.LeaveRequest;

namespace ApprovalManagment.Service.Mappings
{
    public class ViewModelToEntityMappingProfile : Profile
    {
        public ViewModelToEntityMappingProfile()
        {
            CreateMap<LeaveTypeCreateVM, LeaveType>().AfterMap((vm, entity) => entity.SetCreated(entity.CreatedByUserId, DateTime.UtcNow));
            CreateMap<LeaveTypeUpdateVM, LeaveType>().AfterMap((vm, entity) => entity.SetLastUpdated(entity.LastUpdatedByUserId, DateTime.UtcNow));

            CreateMap<LeaveRequestCreateVM, LeaveRequest>().AfterMap((vm, entity) => entity.SetCreated(entity.CreatedByUserId, DateTime.UtcNow));
            CreateMap<LeaveRequestUpdateVM, LeaveRequest>().AfterMap((vm, entity) => entity.SetLastUpdated(entity.LastUpdatedByUserId, DateTime.UtcNow));
        }
    }
}