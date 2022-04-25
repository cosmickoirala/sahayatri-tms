
using System;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class Business

    {
    
        
        public int Id { get; set; }
        [Required]
        [Display(Name = "Business Name")]
        public string Name { get; set; }
        public string BusinessType { get; set; }
        public bool IsAdminVerified { get; set; }
        public string AdminVerificationStatus { get; set; } = "P";
        [Display(Name = "Business Description")]
        public string Description { get; set; }
        public virtual AppUser UpdatedBY { get; set; }
        public virtual AppUser AddedBY { get; set; }
        public virtual Place1 Place { get; set; }
        public DateTime UpdatedDate{ get; set; }
        public DateTime CreatedDate{ get; set; }



    }
}
