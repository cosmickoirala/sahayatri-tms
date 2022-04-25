

using System;

namespace TravelManagementSystem.Models
{
    public class Image

    {
        public int Id { get; set; }

        public virtual AppUser User { get; set; }
        public virtual Place1 Place { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
        public virtual State State { get; set; }
        public virtual Street Street { get; set; }
        public virtual Business Bussiness { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool Forgallery { get; set; }
        public bool IsLogo{ get; set; }
        public bool IsCover{ get; set; }
        public bool IsProfile{ get; set; }
        public int  sliderPosition{ get; set; }

        public virtual AppUser AddedBy { get; set; }
        public virtual AppUser UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
