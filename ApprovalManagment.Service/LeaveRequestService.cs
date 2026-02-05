using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveRequest;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LeaveRequestService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Create(LeaveRequestCreateVM model)
        {
            var user = await unitOfWork.Users.GetIncluding(u => u.Id == model.CreatedByUserId, u => u.LeaveRequests).FirstOrDefaultAsync();

            if (user.LeaveRequests.Any(lr => model.StartDate <= lr.EndDate && model.EndDate >= lr.StartDate))
            {
                return Tuple.Create(0, ResponseCodeEnum.LeaveRequestsOverlapped);
            }

            TimeSpan difference = model.EndDate - model.StartDate;
            int duration = (int)difference.TotalDays;
            if (user.RemainingLeaveBalance < duration)
            {
                return Tuple.Create(0, ResponseCodeEnum.NotEnoughBalance);
            }
            user.RemainingLeaveBalance -= duration;

            var leaveRequest = mapper.Map<LeaveRequestCreateVM, LeaveRequest>(model);
            leaveRequest.LeaveStatus = LeaveStatusEnum.Draft;
            leaveRequest.EmployeeId = user.Id;

            await unitOfWork.LeaveRequests.Add(leaveRequest);

            await unitOfWork.Commit();

            return Tuple.Create(leaveRequest.Id, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Update(LeaveRequestUpdateVM model)
        {
            var user = await unitOfWork.Users.GetIncluding(u => u.Id == model.LastUpdatedByUserId, u => u.LeaveRequests.Where(lr => lr.Id != model.Id)).FirstOrDefaultAsync();

            if (user.LeaveRequests.Any(lr => model.StartDate <= lr.EndDate && model.EndDate >= lr.StartDate))
            {
                return Tuple.Create(0, ResponseCodeEnum.LeaveRequestsOverlapped);
            }

            var leaveRequestDB = await unitOfWork.LeaveRequests.Get(p => p.Id == model.Id && !p.IsDeleted).AsNoTracking().FirstOrDefaultAsync();

            if (leaveRequestDB == null || leaveRequestDB.IsDeleted || (leaveRequestDB.LeaveStatus != LeaveStatusEnum.Draft)) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            TimeSpan modelDifference = model.EndDate - model.StartDate;
            int modelDuration = (int)modelDifference.TotalDays;

            TimeSpan dbDifference = leaveRequestDB.EndDate - leaveRequestDB.StartDate;
            int dbDuration = (int)dbDifference.TotalDays;

            if ((dbDuration + user.RemainingLeaveBalance) < modelDuration)
            {
                return Tuple.Create(0, ResponseCodeEnum.NotEnoughBalance);
            }

            user.RemainingLeaveBalance += dbDuration;
            user.RemainingLeaveBalance -= modelDuration;

            var leaveRequest = mapper.Map<LeaveRequestUpdateVM, LeaveRequest>(model);

            leaveRequest.SetCreated(leaveRequestDB.CreatedByUserId, leaveRequestDB.CreatedOn);

            unitOfWork.LeaveRequests.Update(leaveRequest);

            await unitOfWork.Commit();

            return Tuple.Create(leaveRequest.Id, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<LeaveRequestResultVM, ResponseCodeEnum>> GetById(int id)
        {
            var leaveRequest = await unitOfWork.LeaveRequests.Find(id);

            if (leaveRequest == null || leaveRequest.IsDeleted) return Tuple.Create(new LeaveRequestResultVM(), ResponseCodeEnum.NotFound);

            var model = mapper.Map<LeaveRequest, LeaveRequestResultVM>(leaveRequest);

            return Tuple.Create(model, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<DataTableResponseVM<LeaveRequestResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM requestVM)
        {
            var dataTableResponse = await unitOfWork.LeaveRequests.Search(requestVM);
            return Tuple.Create(mapper.Map<DataTableResponseVM<LeaveRequest>, DataTableResponseVM<LeaveRequestResultVM>>(dataTableResponse), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> ChangeStatus(LeaveRequestChangeStatusVM model)
        {
            var leaveRequestDB = await unitOfWork.LeaveRequests.Get(p => p.Id == model.Id && !p.IsDeleted).Include(p => p.LeaveStatusHistories).FirstOrDefaultAsync();

            if (leaveRequestDB == null || leaveRequestDB.IsDeleted || leaveRequestDB.LeaveStatus == LeaveStatusEnum.Cancelled) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            var newHistory = new LeaveStatusHistory { From = leaveRequestDB.LeaveStatus, To = model.LeaveStatus };
            leaveRequestDB.LeaveStatus = model.LeaveStatus;
            leaveRequestDB.SetLastUpdated(leaveRequestDB.CreatedByUserId, DateTime.UtcNow);
            leaveRequestDB.LeaveStatusHistories.Add(newHistory);

            await unitOfWork.Commit();

            return Tuple.Create(leaveRequestDB.Id, ResponseCodeEnum.Success);
        }
    }
}