using System;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class State
    {



       
        public int Id { get; set; }

        [Required]
        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [Display(Name = "State Description")]
        public string Description { get; set; }
        
        [Display(Name ="Zip Code")]
        public string zipCode { get; set; }

        public virtual Country Country { get; set; }
        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }







    }
}
