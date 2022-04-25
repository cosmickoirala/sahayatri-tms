using System;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class Street

    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "Street Description")]
        public string Description { get; set; }
        
        public virtual City City { get; set; }

        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
