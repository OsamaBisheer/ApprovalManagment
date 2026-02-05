using System.ComponentModel.DataAnnotations;

namespace ApprovalManagment.Domain.ViewModels.User
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}