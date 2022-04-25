using System;

namespace TravelManagementSystem.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Place1 Place { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
