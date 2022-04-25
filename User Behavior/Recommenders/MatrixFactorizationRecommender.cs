using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Abstractions;
using UserBehavior.Mathematics;
using UserBehavior.Objects;
using UserBehavior.Parsers;

namespace UserBehavior.Recommenders
{
    public class MatrixFactorizationRecommender : IRecommender
    {
        private UserPlaceRatingsTable ratings;
        private SvdResult svd;
        private IRater rater;

        private int numUsers;
        private int numArticles;

        private int numFeatures;
        private int learningIterations;        

        public MatrixFactorizationRecommender(IRater implicitRater)
            : this(20, implicitRater)
        {
        }

        public MatrixFactorizationRecommender(int features, IRater implicitRater)
        {
            numFeatures = features;
            learningIterations = 100;
            rater = implicitRater;
        }

        public void Train(UserBehaviorDatabase db)
        {
            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db);
            ratings = ubt.GetUserPlaceRatingsTable(rater);

            SingularValueDecomposition factorizer = new SingularValueDecomposition(numFeatures, learningIterations);
            svd = factorizer.FactorizeMatrix(ratings);

            numUsers = ratings.UserIndexToID.Count;
            numArticles = ratings.ArticleIndexToID.Count;
        }
        
        public double GetRating(int userId, int articleId)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            int articleIndex = ratings.ArticleIndexToID.IndexOf(articleId);

            return GetRatingForIndex(userIndex, articleIndex);
        }

        private double GetRatingForIndex(int userIndex, int articleIndex)
        {
            return svd.AverageGlobalRating + svd.UserBiases[userIndex] + svd.ArticleBiases[articleIndex] + Matrix.GetDotProduct(svd.UserFeatures[userIndex], svd.ArticleFeatures[articleIndex]);
        }

        public List<Suggestion> GetSuggestions(int userId, int numSuggestions)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            UserPlaceRatings user = ratings.Users[userIndex];
            List<Suggestion> suggestions = new List<Suggestion>();

            for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                // If the user in question hasn't rated the given article yet
                if (user.PlaceRatings[articleIndex] == 0)
                {
                    double rating = GetRatingForIndex(userIndex, articleIndex);

                    suggestions.Add(new Suggestion(userId, ratings.ArticleIndexToID[articleIndex], rating));
                }
            }

            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));

            return suggestions.Take(numSuggestions).ToList();
        }
        
    }
}
