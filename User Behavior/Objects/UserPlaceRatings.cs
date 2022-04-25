using System;
using System.Collections.Generic;

namespace UserBehavior.Objects
{
    public class UserPlaceRatings
    {
        public int UserID { get; set; }

        public double[] PlaceRatings { get; set; }

        public double Score { get; set; }

        public UserPlaceRatings(int userId, int placeCount)
        {
            UserID = userId;
            PlaceRatings = new double[placeCount];
        }

        public void AppendRatings(double[] ratings)
        {
            List<double> allRatings = new List<double>();

            allRatings.AddRange(PlaceRatings);
            allRatings.AddRange(ratings);

            PlaceRatings = allRatings.ToArray();
        }
    }
}
