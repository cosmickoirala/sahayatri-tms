using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

        public string UserType { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

    }
}
