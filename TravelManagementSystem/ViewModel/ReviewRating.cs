using System.ComponentModel.DataAnnotations;

namespace TravelManagementSystem.ViewModels
{
    public class ReviewRating
    {
        public int Id { get; set; }
        [Required]
        public decimal Rating { get; set; }
        public string Review { get; set; }
        public string HtmlRating { get; set; }
        public UserModel Reviewer { get; set; }
    }
}
