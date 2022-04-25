using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.ViewModels
{
    public class LoginModel
    {

        [Required]
        [Display(Name = "Username Or Password")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
         public string ReturnUrl { get; set; }

    }
}
