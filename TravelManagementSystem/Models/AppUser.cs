using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TravelManagementSystem.Models
{
    public class AppUser : IdentityUser<int>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
