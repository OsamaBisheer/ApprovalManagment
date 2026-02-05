namespace ApprovalManagment.Domain.Enums
{
    public class Enumeration
    {
        public enum DepartmentEnum
        {
            CS = 1,
            IT = 2,
            IS = 3
        }

        public enum LeaveStatusEnum
        {
            Draft = 1,
            Approved = 2,
            Rejected = 3,
            Cancelled = 4
        }

        public enum RoleEnum
        {
            Admin = 1,
            Manager = 2,
            Employee = 3
        }

        public enum ResponseCodeEnum
        {
            Success = 200,
            BadRequest = 400,
            UnAuthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            MethodNotAllowed = 405,
            Duplicate = 409,
            LeaveRequestsOverlapped = 460,
            NotEnoughBalance = 461,
            NotValidLeaveStatus = 462,
            InvalidCredentials = 480,
            InternalServerError = 500
        }
    }
}