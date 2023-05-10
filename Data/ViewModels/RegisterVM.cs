using System.ComponentModel.DataAnnotations;

namespace officehours_management.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Full name")]
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Full name length must be at least 6 and maximum 100")]
        public string FullName { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(45, MinimumLength = 6, ErrorMessage = "Username length must be at least 6 and maximum 45")]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(45, MinimumLength = 8, ErrorMessage = "Password length must be at least 8 and maximum 45")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[\d])(?=.*[!@#$%&]).*$", ErrorMessage = 
        "Password must contain at least 1 upper case, 1 lower case, 1 number and 1 special character and no whitespaces.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}