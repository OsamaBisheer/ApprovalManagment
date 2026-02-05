using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveRequest;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using ApprovalManagment.Domain.ViewModels.User;
using AutoMapper;

namespace ApprovalManagment.Service.Mappings
{
    public class EntityToViewModelMappingProfile : Profile
    {
        public EntityToViewModelMappingProfile()
        {
            CreateMap<User, LookupVM>()
               .ForMember(Dest => Dest.Name, opt => opt.MapFrom(src => src.UserName));
            CreateMap<User, UserResultVM>();
            CreateMap<DataTableResponseVM<User>, DataTableResponseVM<UserResultVM>>();

            CreateMap<LeaveType, LookupVM>();
            CreateMap<LeaveType, LeaveTypeResultVM>();
            CreateMap<DataTableResponseVM<LeaveType>, DataTableResponseVM<LeaveTypeResultVM>>();

            CreateMap<LeaveRequest, LeaveRequestResultVM>();
            CreateMap<DataTableResponseVM<LeaveRequest>, DataTableResponseVM<LeaveRequestResultVM>>();
        }
    }
}