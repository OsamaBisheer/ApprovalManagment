using ApprovalManagment.API.Controllers;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveRequest;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : CommonControllerBase
    {
        private readonly ILeaveRequestService leaveRequestService;

        public LeaveRequestController(ILeaveRequestService _leaveRequestService)
        {
            leaveRequestService = _leaveRequestService;
        }

        [HttpGet, Route("by-id/{id}")]
        [Authorize]
        public async Task<ActionResult> GetById(int id)
        {
            (LeaveRequestResultVM model, ResponseCodeEnum responseCode) = await leaveRequestService.GetById(id);

            return GetActionResult(new ResponseModel
            {
                Result = model,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpGet, Route("search")]
        [Authorize]
        public async Task<ActionResult> GetForPagination([FromQuery] DataTableRequestVM requestVM)
        {
            (DataTableResponseVM<LeaveRequestResultVM> dataTableResponseVM, ResponseCodeEnum responseCode) = await leaveRequestService.Search(requestVM);

            return GetActionResult(new ResponseModel
            {
                Result = dataTableResponseVM,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("create")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult> Create(LeaveRequestCreateVM createVM)
        {
            if (!ModelState.IsValid)
            {
                return GetActionResult(new ResponseModel
                {
                    Result = 0,
                    Code = ResponseCodeEnum.BadRequest,
                    MessageFL = nameof(ResponseCodeEnum.BadRequest),
                    MessageSL = nameof(ResponseCodeEnum.BadRequest)
                });
            }

            createVM.CreatedByUserId = GetCurrentUserId();
            (int id, ResponseCodeEnum responseCode) = await leaveRequestService.Create(createVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPut, Route("update")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult> Update(LeaveRequestUpdateVM updateVM)
        {
            if (!ModelState.IsValid)
            {
                return GetActionResult(new ResponseModel
                {
                    Result = 0,
                    Code = ResponseCodeEnum.BadRequest,
                    MessageFL = nameof(ResponseCodeEnum.BadRequest),
                    MessageSL = nameof(ResponseCodeEnum.BadRequest)
                });
            }

            updateVM.LastUpdatedByUserId = GetCurrentUserId();
            (int id, ResponseCodeEnum responseCode) = await leaveRequestService.Update(updateVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("change-status")]
        [Authorize]
        public async Task<ActionResult> ChangeStatus(LeaveRequestChangeStatusVM updateVM)
        {
            if (!ModelState.IsValid)
            {
                return GetActionResult(new ResponseModel
                {
                    Result = 0,
                    Code = ResponseCodeEnum.BadRequest,
                    MessageFL = nameof(ResponseCodeEnum.BadRequest),
                    MessageSL = nameof(ResponseCodeEnum.BadRequest)
                });
            }

            var role = GetClaimValue(ClaimTypes.Role);
            if ((updateVM.LeaveStatus == LeaveStatusEnum.Draft) ||
                (updateVM.LeaveStatus == LeaveStatusEnum.Cancelled && role != nameof(RoleEnum.Employee)) ||
                (updateVM.LeaveStatus != LeaveStatusEnum.Cancelled && role == nameof(RoleEnum.Employee)))
            {
                return GetActionResult(new ResponseModel
                {
                    Result = 0,
                    Code = ResponseCodeEnum.NotValidLeaveStatus,
                    MessageFL = nameof(ResponseCodeEnum.NotValidLeaveStatus),
                    MessageSL = nameof(ResponseCodeEnum.NotValidLeaveStatus)
                });
            }

            (int id, ResponseCodeEnum responseCode) = await leaveRequestService.ChangeStatus(updateVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }
    }
}