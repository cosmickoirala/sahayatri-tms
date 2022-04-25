namespace UserBehavior.Objects
{
    public class PlaceType
    {
        public int PlaceID { get; set; }

        public string TypeName { get; set; }

        public PlaceType(int articleid, string type)
        {
            PlaceID = articleid;
            TypeName = type;
        }
    }
}
