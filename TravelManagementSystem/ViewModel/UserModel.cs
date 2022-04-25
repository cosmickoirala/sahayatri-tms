using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.ViewModels
{
    public class UserModel
    {

       public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string BusinessCount { get; set; }
        public string WishCount { get; set; }
        public string PlacesCount { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string CoverImagePath { get; set; }
        public string ProfileImage { get; set; }
        public string ReturnUrl { get; set; }
        public List<PlaceModel> Places { get; set; }
        public List<PlaceModel> WishList { get; set; }
        public List<ReviewRating> ReviewList { get; set; }
    }
}
