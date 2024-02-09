using System.ComponentModel.DataAnnotations;

namespace Edge.DomainModels
{
    public class Register
    {
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}
