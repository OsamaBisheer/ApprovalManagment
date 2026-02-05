using ApprovalManagment.Domain.Entities;
using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.User;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public UserService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<User> _userManager)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            userManager = _userManager;
        }

        //public Task<Tuple<int, ResponseCodeEnum>> Create(UserAddVM workflowVM)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<ResponseCodeEnum> Delete(int id, string lastUpdatedByUserId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Tuple<UserResultVM, ResponseCodeEnum>> GetById(string id)
        {
            var user = await unitOfWork.Users.Find(id);

            if (user == null) return Tuple.Create(new UserResultVM(), ResponseCodeEnum.NotFound);

            var model = mapper.Map<User, UserResultVM>(user);

            return Tuple.Create(model, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<int, ResponseCodeEnum>> GetCount(RoleEnum role)
        {
            var count = await unitOfWork.Users.Get(p => p.UserRoles.Any(r => r.Role.Name == role.ToString())).CountAsync();

            return Tuple.Create(count, ResponseCodeEnum.Success);
        }

        public async Task<Tuple<DataTableResponseVM<UserResultVM>, ResponseCodeEnum>> Search(DataTableRequestVM requestVM)
        {
            var dataTableResponse = await unitOfWork.Users.Search(requestVM);
            return Tuple.Create(mapper.Map<DataTableResponseVM<User>, DataTableResponseVM<UserResultVM>>(dataTableResponse), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<List<LookupVM>, ResponseCodeEnum>> GetLookup()
        {
            var users = await unitOfWork.Users.GetAll().ToListAsync();
            return Tuple.Create(mapper.Map<List<User>, List<LookupVM>>(users), ResponseCodeEnum.Success);
        }

        public async Task<Tuple<string, ResponseCodeEnum>> GetRoleId(RoleEnum role)
        {
            var id = await unitOfWork.ApplicationRoles.Get(p => p.Name == role.ToString()).Select(p => p.Id).FirstOrDefaultAsync();

            return Tuple.Create(id, ResponseCodeEnum.Success);
        }

        public async Task<ResponseCodeEnum> AddUserRoles(string userId, List<string> rolesIds)
        {
            var userRoles = rolesIds.Select(id => new ApplicationUserRole { UserId = userId, RoleId = id });
            await unitOfWork.ApplicationUserRoles.AddRange(userRoles);
            await unitOfWork.Commit();

            return ResponseCodeEnum.Success;
        }

        public async Task<Tuple<string, ResponseCodeEnum>> Update(UserUpdateVM model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return Tuple.Create("", ResponseCodeEnum.NotFound);
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Department = model.Department;
            user.ManagerId = model.ManagerId;

            await userManager.UpdateAsync(user);

            return Tuple.Create(user.Id, ResponseCodeEnum.Success);
        }
    }
}