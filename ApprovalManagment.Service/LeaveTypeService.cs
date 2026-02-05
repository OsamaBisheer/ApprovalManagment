using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using ApprovalManagment.Domain.ViewModels.User;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Service
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LeaveTypeService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Create(LeaveTypeCreateVM model)
        {
            var product = mapper.Map<LeaveTypeCreateVM, LeaveType>(model);

            await unitOfWork.LeaveTypes.Add(product);

            await unitOfWork.Commit();

            return Tuple.Create(product.Id, ResponseCodeEnum.Success);
        }

        public async Task<ResponseCodeEnum> Delete(int id, string lastUpdatedByUserId)
        {
            var leaveType = await unitOfWork.LeaveTypes.Get(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            leaveType.MarkAsDeleted(lastUpdatedByUserId);
            await unitOfWork.Commit();

            return ResponseCodeEnum.Success;
        }

        public async Task<Tuple<LeaveTypeResultVM, ResponseCodeEnum>> GetById(int id)
        {
            var leaveType = await unitOfWork.LeaveTypes.Find(id);

            if (leaveType == null || leaveType.IsDeleted) return Tuple.Create(new LeaveTypeResultVM(), ResponseCodeEnum.NotFound);

            var model = mapper.Map<LeaveType, LeaveTypeResultVM>(leaveType);

            return Tuple.Create(model, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<DataTableResponseVM<LeaveTypeResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM requestVM)
        {
            var dataTableResponse = await unitOfWork.LeaveTypes.Search(requestVM);
            return Tuple.Create(mapper.Map<DataTableResponseVM<LeaveType>, DataTableResponseVM<LeaveTypeResultVM>>(dataTableResponse), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup()
        {
            var leaveTypes = await unitOfWork.LeaveTypes.GetAll().ToListAsync();
            return Tuple.Create(mapper.Map<List<LeaveType>, List<LookupVM>>(leaveTypes), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> Update(LeaveTypeUpdateVM model)
        {
            var leaveTypeDB = await unitOfWork.LeaveTypes.Get(p => p.Id == model.Id && !p.IsDeleted).AsNoTracking().FirstOrDefaultAsync();

            if (leaveTypeDB == null || leaveTypeDB.IsDeleted) return Tuple.Create(0, ResponseCodeEnum.NotFound);

            var leaveType = mapper.Map<LeaveTypeUpdateVM, LeaveType>(model);

            leaveType.SetCreated(leaveTypeDB.CreatedByUserId, leaveTypeDB.CreatedOn);

            unitOfWork.LeaveTypes.Update(leaveType);

            await unitOfWork.Commit();

            return Tuple.Create(leaveType.Id, ResponseCodeEnum.Success);
        }
    }
}