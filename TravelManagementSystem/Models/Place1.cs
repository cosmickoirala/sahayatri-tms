using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class Place1

    {
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public bool IsVisited { get; set; }
        public bool IsAdminVerified { get; set; }
        public string AdminVerificationStatus { get; set; } = "P";
        public string PlaceType { get; set; }
        public string Description { get; set; }
  
        public virtual Country Country { get; set; }
        public virtual State State { get; set; }
        public virtual City City { get; set; }
        public virtual Street Streets { get; set; }
        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
