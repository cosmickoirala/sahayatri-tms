using Microsoft.AspNetCore.Identity;
namespace TravelManagementSystem.Models
{
    public class AppRole:IdentityRole<int>
    {

        public bool IsAdmin { get; set; }
        public bool IsGuest { get; set; }
        public bool IsUser { get; set; }


    }
}
