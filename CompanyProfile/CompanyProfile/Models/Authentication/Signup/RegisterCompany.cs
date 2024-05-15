using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CompanyProfile.Models.Authentication.Signup
{
    public class RegisterCompany
    {
        [Required(ErrorMessage = "Companyname is Required")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
