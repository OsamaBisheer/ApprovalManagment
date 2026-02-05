using ApprovalManagment.API.Controllers;
using ApprovalManagment.Domain.Entities.Identity;
using ApprovalManagment.Domain.Interfaces.ICore;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using ApprovalManagment.Domain.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : CommonControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration config;
        private readonly IIdentityProvider identityProvider;
        private readonly IUserService userService;

        public UserController(IUserService _userService, UserManager<User> _userManager, SignInManager<User> _signInManager, IConfiguration _config, IIdentityProvider _identityProvider)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            config = _config;
            identityProvider = _identityProvider;
            userService = _userService;
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return GetActionResult(new ResponseModel
            {
                Result = null,
                Code = ResponseCodeEnum.InvalidCredentials,
                MessageFL = nameof(ResponseCodeEnum.InvalidCredentials),
                MessageSL = nameof(ResponseCodeEnum.InvalidCredentials)
            });

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null) return GetActionResult(new ResponseModel
            {
                Result = null,
                Code = ResponseCodeEnum.InvalidCredentials,
                MessageFL = nameof(ResponseCodeEnum.InvalidCredentials),
                MessageSL = nameof(ResponseCodeEnum.InvalidCredentials)
            });

            var signInResult = signInManager.PasswordSignInAsync(user, model.Password, true, false).Result;
            if (!signInResult.Succeeded) return GetActionResult(new ResponseModel
            {
                Result = null,
                Code = ResponseCodeEnum.InvalidCredentials,
                MessageFL = nameof(ResponseCodeEnum.InvalidCredentials),
                MessageSL = nameof(ResponseCodeEnum.InvalidCredentials)
            });

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id),
            };
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToInt32(config["Jwt:ExpiresInHours"])),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return GetActionResult(new ResponseModel
            {
                Result = tokenString,
                Code = ResponseCodeEnum.Success
            });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            var user = identityProvider.GetUser();
            if (user != null)
            {
                await signInManager.SignOutAsync();
            }
            return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.Success
            });
        }

        [HttpPost("add-user")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> AddUser(UserAddVM model)
        {
            if (!ModelState.IsValid) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.BadRequest,
                MessageFL = nameof(ResponseCodeEnum.BadRequest),
                MessageSL = nameof(ResponseCodeEnum.BadRequest)
            });

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                ManagerId = model.ManagerId
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.InternalServerError,
                MessageFL = nameof(ResponseCodeEnum.InternalServerError),
                MessageSL = nameof(ResponseCodeEnum.InternalServerError)
            });

            (string employeeRoleId, ResponseCodeEnum responseCode) = await userService.GetRoleId(RoleEnum.Employee);
            if (responseCode != ResponseCodeEnum.Success) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.InternalServerError,
                MessageFL = nameof(ResponseCodeEnum.InternalServerError),
                MessageSL = nameof(ResponseCodeEnum.InternalServerError)
            });

            var resultCode = await userService.AddUserRoles(user.Id, new List<string> { employeeRoleId });

            return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.Success,
                MessageFL = null,
                MessageSL = null
            });
        }

        [HttpGet, Route("by-id/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetById(string id)
        {
            (UserResultVM model, ResponseCodeEnum responseCode) = await userService.GetById(id);

            return GetActionResult(new ResponseModel
            {
                Result = model,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpGet, Route("search")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetForPagination([FromQuery] DataTableRequestVM requestVM)
        {
            (DataTableResponseVM<UserResultVM> dataTableResponseVM, ResponseCodeEnum responseCode) = await userService.Search(requestVM);

            return GetActionResult(new ResponseModel
            {
                Result = dataTableResponseVM,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPut, Route("update")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Update(UserUpdateVM model)
        {
            if (!ModelState.IsValid) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.BadRequest,
                MessageFL = nameof(ResponseCodeEnum.BadRequest),
                MessageSL = nameof(ResponseCodeEnum.BadRequest)
            });

            (string userId, ResponseCodeEnum responseCode) = await userService.Update(model);

            return GetActionResult(new ResponseModel
            {
                Result = userId,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpDelete, Route("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (!ModelState.IsValid) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.BadRequest,
                MessageFL = nameof(ResponseCodeEnum.BadRequest),
                MessageSL = nameof(ResponseCodeEnum.BadRequest)
            });

            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.InternalServerError,
                MessageFL = nameof(ResponseCodeEnum.InternalServerError),
                MessageSL = nameof(ResponseCodeEnum.InternalServerError)
            });

            return GetActionResult(new ResponseModel
            {
                Code = ResponseCodeEnum.Success,
                MessageFL = null,
                MessageSL = null
            });
        }

        [HttpGet, Route("lookup")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetLookup()
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await userService.GetLookup();

            return GetActionResult(new ResponseModel
            {
                Result = lookups,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpGet, Route("count/{roleEnum}")]
        [Authorize]
        public async Task<ActionResult> GetCount(RoleEnum roleEnum)
        {
            (int count, ResponseCodeEnum responseCode) = await userService.GetCount(roleEnum);

            return GetActionResult(new ResponseModel
            {
                Result = count,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }
    }
}