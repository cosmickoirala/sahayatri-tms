using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Abstractions;
using UserBehavior.Objects;

namespace UserBehavior.Parsers
{
    public class UserBehaviorTransformer
    {
        private UserBehaviorDatabase db;

        public UserBehaviorTransformer(UserBehaviorDatabase database)
        {
            db = database;
        }
        
        /// <summary>
        /// Get a list of all users and their ratings on every article
        /// </summary>
        public UserPlaceRatingsTable GetUserPlaceRatingsTable(IRater rater)
        {
            UserPlaceRatingsTable table = new UserPlaceRatingsTable();
            
            table.UserIndexToID = db.Users.OrderBy(x => x.UserId).Select(x => x.UserId).Distinct().ToList();
            table.ArticleIndexToID = db.Places.OrderBy(x => x.PlaceID).Select(x => x.PlaceID).Distinct().ToList();

            foreach (int userId in table.UserIndexToID)
            {
                table.Users.Add(new UserPlaceRatings(userId, table.ArticleIndexToID.Count));
            }

            var userArticleRatingGroup = db.UserActions
                .GroupBy(x => new { x.UserID, x.PlaceID })
                .Select(g => new { g.Key.UserID, g.Key.PlaceID, Rating = rater.GetRating(g.ToList()) })
                .ToList();

            foreach (var userAction in userArticleRatingGroup)
            {
                int userIndex = table.UserIndexToID.IndexOf(userAction.UserID);
                int articleIndex = table.ArticleIndexToID.IndexOf(userAction.PlaceID);

                table.Users[userIndex].PlaceRatings[articleIndex] = userAction.Rating;
            }

            return table;
        }

        /// <summary>
        /// Get a table of all articles as rows and all tags as columns
        /// </summary>
        public List<PlaceTypeCounts> GetArticleTagCounts()
        {
            List<PlaceTypeCounts> articleTags = new List<PlaceTypeCounts>();

            foreach (var Place in db.Places)
            {
                PlaceTypeCounts placeType = new PlaceTypeCounts(Place.PlaceID, db.Types.Count);

                for (int tag = 0; tag < db.Types.Count; tag++)
                {
                   // placeType.TypeCounts = placeType.TypeCounts.Any(x =>x == db.Types[tag]) ? 1.0 : 0.0;
                }

                articleTags.Add(placeType);
            }

            return articleTags;
        }

        /// <summary>
        /// Get a list of all users and the number of times they viewed articles with a specific tag
        /// </summary>
        [Obsolete]
        public List<UserActionType> GetUserTags()
        {
            List<UserActionType> userData = new List<UserActionType>();
            List<PlaceType> articleTags = db.GetArticleTagLinkingTable();

            int numFeatures = db.Types.Count;

            // Create a dataset that links every user action to a list of tags associated with the article that action was for, then 
            // group them by user, action, and tag so we can get a count of the number of times each user performed a action on an article with a specific tag
            var userActionTags = db.UserActions
                .Join(articleTags, u => u.PlaceID, t => t.PlaceID, (u, t) => new { u.UserID, t.TypeName })
                .GroupBy(x => new { x.UserID, x.TypeName })
                .Select(g => new { g.Key.UserID, g.Key.TypeName, Count = g.Count() })
                .OrderBy(x => x.UserID).ThenBy(x => x.TypeName)
                .ToList();

            int totalUserActions = userActionTags.Count();
            int lastFoundIndex = 0;

            // Get action-tag data
            foreach (var user in db.Users)
            {
                int dataCol = 0;
                UserActionType uat = new UserActionType(user.UserId, numFeatures);

                foreach (var type in db.Types)
                {
                    // Count the number of times this user interacted with an article with this tag
                    // We can loop through like this since the list is sorted
                    int tagActionCount = 0;
                    for (int i = lastFoundIndex; i < totalUserActions; i++)
                    {
                        if (userActionTags[i].UserID == user.UserId && userActionTags[i].TypeName == type.TypeName)
                        {
                            lastFoundIndex = i;
                            tagActionCount = userActionTags[i].Count;
                            break;
                        }
                    }

                    uat.ActionTypeData[dataCol++] = tagActionCount;
                }

                // Normalize data to values between 0 and 5
                double upperLimit = 5.0;
                double max = uat.ActionTypeData.Max();
                if (max > 0)
                {
                    for (int i = 0; i < uat.ActionTypeData.Length; i++)
                    {
                        uat.ActionTypeData[i] = (uat.ActionTypeData[i] / max) * upperLimit;
                    }
                }

                userData.Add(uat);
            }

            return userData;
        }
    }
}
