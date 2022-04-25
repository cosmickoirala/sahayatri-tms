namespace UserBehavior.Objects
{
    public class Suggestion
    {
        public int UserID { get; set; }

        public int PlaceID { get; set; }

        public double Rating { get; set; }

        public Suggestion(int userId, int placeID, double assurance)
        {
            UserID = userId;
            PlaceID = placeID;
            Rating = assurance;
        }
    }
}
