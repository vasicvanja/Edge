using System.ComponentModel.DataAnnotations;

namespace Edge.DomainModels
{
    public class Register
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        public string PhoneNumber { get; set; }
    }
}
