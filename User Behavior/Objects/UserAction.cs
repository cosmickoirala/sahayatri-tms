using System;

namespace UserBehavior.Objects
{
    [Serializable]
    public class UserAction
    {
        public int Day { get; set; }

        public string Action { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public int PlaceID { get; set; }

        public string PlaceName { get; set; }

        public UserAction(int day, string action, int userid, string username, int placeid, string placename)
        {
            Day = day;
            Action = action;
            UserID = userid;
            UserName = username;
            PlaceID = placeid;
            PlaceName = placename;
        }

        public override string ToString()
        {
            return Day + "," + Action + "," + UserID + "," + UserName + "," + PlaceID + "," + PlaceName;
        }
    }
}
