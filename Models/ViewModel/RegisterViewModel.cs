using System.ComponentModel.DataAnnotations;

namespace StyleSphere.Models.ViewModel
{
    public class RegisterViewModel
    {
        
        public string? Name { get; set; }

        
        [Required(ErrorMessage = "Email is required!!!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!!!")]
        public required string Email { get; set; }

        
        [Required(ErrorMessage = "Password is required!!!")]
        [StringLength(20 , MinimumLength = 8 , ErrorMessage = "Password doesn't match requirements!!!")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword" , ErrorMessage = "Password doesn't mact!!!")]
        public required string Password { get; set; }


        [Required(ErrorMessage = "Confirm your password!!!")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public required string ConfirmPassword { get; set; } 
    }
}
