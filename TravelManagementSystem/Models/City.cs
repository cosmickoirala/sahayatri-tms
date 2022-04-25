using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class City
    {
       
        public int Id { get; set; }
        [Display (Name="City Name")]
        public string CityName { get; set; }

        [Display(Name = "City Description")]
        public string Description { get; set; }
        public virtual State State { get; set; }

        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
