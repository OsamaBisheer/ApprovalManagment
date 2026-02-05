using static ApprovalManagment.Domain.Enums.Enumeration;

namespace ApprovalManagment.Domain.ViewModels.Common
{
    public class ResponseModel
    {
        public ResponseCodeEnum Code { get; set; }
        public string MessageFL { get; set; }
        public string MessageSL { get; set; }
        public dynamic Result { get; set; }
    }
}