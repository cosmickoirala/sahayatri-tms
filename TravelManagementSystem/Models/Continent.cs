using System;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.Models
{
    public class Continent
    {

        public int  Id { get; set; }

        [Display (Name="Place Name")]
        public string Name { get; set; }
        public string Description { get; set; }



        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [Display(Name = "City Name")]
        public string CityName { get; set; }

        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }



    }
}
