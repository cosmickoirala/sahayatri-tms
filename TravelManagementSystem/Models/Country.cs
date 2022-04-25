using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class Country
    {
       
        public int Id { get; set; }
        [Display(Name = "Country Name")]
        
        [Required]
        public string CountryName { get; set; }

        [Display(Name = "Country Description")]
        public string Description { get; set; }
        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
