using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBehavior.Objects
{
    public class PlaceRating
    {
        public int PlaceID { get; set; }

        public double Rating { get; set; }

        public PlaceRating(int placeId, double rating)
        {
            PlaceID = placeId;
            Rating = rating;
        }
    }
}
