using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ZdeskUserPortal.DTOModel
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
