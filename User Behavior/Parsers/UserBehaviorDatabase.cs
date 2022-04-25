using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Objects;
//using TravelManagementSystem.Data;


namespace UserBehavior.Parsers
{
    [Serializable]
    public class UserBehaviorDatabase
    {
        public List<PlaceType> Types { get; set; }

        public List<PlaceRating> Places { get; set; }

        public List<User> Users { get; set; }

        public List<UserAction> UserActions { get; set; }

        //public ApplicationDbContext db; 
        public UserBehaviorDatabase()
        {
            Types = new List<PlaceType>();
            Places = new List<PlaceRating>();
            Users = new List<User>();
            UserActions = new List<UserAction>();
        }

        public List<PlaceType> GetArticleTagLinkingTable()
        {
            List<PlaceType> articleTags = new List<PlaceType>();

            foreach ( var place in Places)
            {
                foreach (var type in Types)
                {
                    articleTags.Add(new PlaceType(place.PlaceID, type.TypeName));
                }
            }

            return articleTags;
        }

        public UserBehaviorDatabase Clone()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;
                return (UserBehaviorDatabase)formatter.Deserialize(ms);
            }
        }
    }
}
