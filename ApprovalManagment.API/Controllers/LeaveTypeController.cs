using ApprovalManagment.API.Controllers;
using ApprovalManagment.Domain.Interfaces.IServices;
using ApprovalManagment.Domain.ViewModels.Common;
using ApprovalManagment.Domain.ViewModels.LeaveRequest;
using ApprovalManagment.Domain.ViewModels.LeaveType;
using ApprovalManagment.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveTypeController : CommonControllerBase
    {
        private readonly ILeaveTypeService leaveTypeService;

        public LeaveTypeController(ILeaveTypeService _leaveTypeService)
        {
            leaveTypeService = _leaveTypeService;
        }

        [HttpGet, Route("by-id/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> GetById(int id)
        {
            (LeaveTypeResultVM model, ResponseCodeEnum responseCode) = await leaveTypeService.GetById(id);

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
            (DataTableResponseVM<LeaveTypeResultVM> dataTableResponseVM, ResponseCodeEnum responseCode) = await leaveTypeService.Search(requestVM);

            return GetActionResult(new ResponseModel
            {
                Result = dataTableResponseVM,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPost, Route("create")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Create(LeaveTypeCreateVM createVM)
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
            (int id, ResponseCodeEnum responseCode) = await leaveTypeService.Create(createVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpPut, Route("update")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Update(LeaveTypeUpdateVM updateVM)
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

            (int id, ResponseCodeEnum responseCode) = await leaveTypeService.Update(updateVM);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpDelete, Route("delete/{id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult> Delete(int id)
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

            var currentUserId = GetCurrentUserId();
            ResponseCodeEnum responseCode = await leaveTypeService.Delete(id, currentUserId);

            return GetActionResult(new ResponseModel
            {
                Result = id,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }

        [HttpGet, Route("lookup")]
        [Authorize(Policy = "Employee")]
        public async Task<ActionResult> GetLookup()
        {
            (List<LookupVM> lookups, ResponseCodeEnum responseCode) = await leaveTypeService.GetLookup();

            return GetActionResult(new ResponseModel
            {
                Result = lookups,
                Code = responseCode,
                MessageFL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString(),
                MessageSL = responseCode == ResponseCodeEnum.Success ? null : responseCode.ToString()
            });
        }
    }
}