using System;

namespace TravelManagementSystem.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public decimal value { get; set; }
        public string Review { get; set; }
        public virtual AppUser User { get; set; }
        public virtual Business Business { get; set; }
        public virtual Place1 Place { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        public virtual State State { get; set; }
        public virtual Street Street { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
