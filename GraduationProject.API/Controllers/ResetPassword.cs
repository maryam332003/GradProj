using System.ComponentModel.DataAnnotations;

namespace GraduationProject.APIs.Controllers
{
    public class ResetPassword
    {
        [Required]
        public string Password { get; set; } = null!;
        [Compare("Password",ErrorMessage = "Password and Confirm Password dosen't match")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}