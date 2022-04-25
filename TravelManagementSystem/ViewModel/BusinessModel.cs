using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TravelManagementSystem.Models;

namespace TravelManagementSystem.ViewModels
{
    public class BusinessModel
    {

        [Display(Name = "Country Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is Required")]
        public string CountryId { get; set; }

        [Display(Name = "State Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "State is Required")]

        public string StateId { get; set; }
        [Display(Name = "City Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is Required")]

        public string CityId { get; set; }
        [Display(Name = "Street Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "City is Required")]

        public string StreetId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Required")]

        public string Name { get; set; }

        [Display(Name = "Business Type")]
        public string BusinessType { get; set; }
        [Display(Name = "Is Visited")]
        public bool IsVisited { get; set; }
        public bool IsRated { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Rating { get; set; }
        public int Id { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFileCollection Images { get; set; }
        public IFormFile CoverImage { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cover Image is Required")]
        public List<Image> ImagesList { get; set; }
        public string CoverImagePath { get; set; }
        public string LogoPath { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> StreetList { get; set; }
        public List<SelectListItem> PlaceTypeList { get; set; }
        public List<ReviewRating> ReviewsList { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }
        public Place1 Place { get; set; }
        public State state { get; set; }
        public Street street { get; set; }




    }
}
