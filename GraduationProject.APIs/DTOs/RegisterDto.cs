using System.ComponentModel.DataAnnotations;

namespace GraduationProject.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$",
        ErrorMessage = "Password must be at Least 8 Characters Along and contain one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string Password { get; set; }
        public string? Image { get; set; }
        [Required]
        [Phone]
        public string PhoneNumner { get; set; }
        [Required]
        public string Address { get; set; }
  
    }
}
