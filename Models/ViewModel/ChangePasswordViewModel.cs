using System.ComponentModel.DataAnnotations;

namespace StyleSphere.Models.ViewModel
{
    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "Email is required!!!")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!!!")]
        public string email { get; set; }


        [Required(ErrorMessage = "Password is required!!!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password doesn't match requirements!!!")]
        [DataType(DataType.Password)]
        [Compare("newconfirmPassword", ErrorMessage = "Password doesn't mact!!!")]
        [Display(Name = "New Password")]

        public string newpassword { get; set; }


        [Required(ErrorMessage = "Confirm your password!!!")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm New Password")]
        public string newconfirmPassword { get; set; }

    }
}
