using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Abstractions;
using UserBehavior.Comparers;
using UserBehavior.Mathematics;
using UserBehavior.Objects;
using UserBehavior.Parsers;

namespace UserBehavior.Recommenders
{
    public class ItemCollaborativeFilterRecommender : IRecommender
    {
        private IComparer comparer;
        private IRater rater;
        private UserPlaceRatingsTable ratings;
        private double[][] transposedRatings;

        private int neighborCount;

        public ItemCollaborativeFilterRecommender(IComparer itemComparer, IRater implicitRater, int numberOfNeighbors)
        {
            comparer = itemComparer;
            rater = implicitRater;
            neighborCount = numberOfNeighbors;
        }

        private void FillTransposedRatings()
        {
            int features = ratings.Users.Count;
            transposedRatings = new double[ratings.ArticleIndexToID.Count][];

            // Precompute a transposed ratings matrix where each row becomes an article and each column a user or tag
            for (int a = 0; a < ratings.ArticleIndexToID.Count; a++)
            {
                transposedRatings[a] = new double[features];

                for (int f = 0; f < features; f++)
                {
                    transposedRatings[a][f] = ratings.Users[f].PlaceRatings[a];
                }
            }
        }

        private List<int> GetHighestRatedArticlesForUser(int userIndex)
        {
            List<Tuple<int, double>> items = new List<Tuple<int, double>>();

            for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                // Create a list of every article this user has viewed
                if (ratings.Users[userIndex].PlaceRatings[articleIndex] != 0)
                {
                    items.Add(new Tuple<int, double>(articleIndex, ratings.Users[userIndex].PlaceRatings[articleIndex]));
                }
            }

            // Sort the articles by rating
            items.Sort((c, n) => n.Item2.CompareTo(c.Item2));

            return items.Select(x => x.Item1).ToList();
        }

        public void Train(UserBehaviorDatabase db)
        {
            UserBehaviorTransformer ubt = new UserBehaviorTransformer(db);
            ratings = ubt.GetUserPlaceRatingsTable(rater);

            List<PlaceTypeCounts> articleTags = ubt.GetArticleTagCounts();
            ratings.AppendArticleFeatures(articleTags);

            FillTransposedRatings();
        }
        
        public double GetRating(int userId, int articleId)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            int articleIndex = ratings.ArticleIndexToID.IndexOf(articleId);

            var userRatings = ratings.Users[userIndex].PlaceRatings.Where(x => x != 0);
            var PlaceRatings = ratings.Users.Select(x => x.PlaceRatings[articleIndex]).Where(x => x != 0);

            double averageUser = userRatings.Count() > 0 ? userRatings.Average() : 0;
            double averageArticle = PlaceRatings.Count() > 0 ? PlaceRatings.Average() : 0;

            // For now, just return the average rating across this user and article
            return averageArticle > 0 && averageUser > 0 ? (averageUser + averageArticle) / 2.0 : Math.Max(averageUser, averageArticle);
        }

        public List<Suggestion> GetSuggestions(int userId, int numSuggestions)
        {
            int userIndex = ratings.UserIndexToID.IndexOf(userId);
            List<int> articles = GetHighestRatedArticlesForUser(userIndex).Take(5).ToList();
            List<Suggestion> suggestions = new List<Suggestion>();

            foreach (int articleIndex in articles)
            {
                int articleId = ratings.ArticleIndexToID[articleIndex];
                List<PlaceRating> neighboringArticles = GetNearestNeighbors(articleId, neighborCount);

                foreach (PlaceRating neighbor in neighboringArticles)
                {
                    int neighborArticleIndex = ratings.ArticleIndexToID.IndexOf(neighbor.PlaceID);

                    double averageArticleRating = 0.0;
                    int count = 0;
                    for (int userRatingIndex = 0; userRatingIndex < ratings.UserIndexToID.Count; userRatingIndex++)
                    {
                        if (transposedRatings[neighborArticleIndex][userRatingIndex] != 0)
                        {
                            averageArticleRating += transposedRatings[neighborArticleIndex][userRatingIndex];
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        averageArticleRating /= count;
                    }

                    suggestions.Add(new Suggestion(userId, neighbor.PlaceID, averageArticleRating));
                }
            }

            suggestions.Sort((c, n) => n.Rating.CompareTo(c.Rating));

            return suggestions.Take(numSuggestions).ToList();
        }

        private List<PlaceRating> GetNearestNeighbors(int articleId, int numArticles)
        {
            List<PlaceRating> neighbors = new List<PlaceRating>();
            int mainArticleIndex = ratings.ArticleIndexToID.IndexOf(articleId);
            
            for (int articleIndex = 0; articleIndex < ratings.ArticleIndexToID.Count; articleIndex++)
            {
                int searchArticleId = ratings.ArticleIndexToID[articleIndex];

                double score = comparer.CompareVectors(transposedRatings[mainArticleIndex], transposedRatings[articleIndex]);

                neighbors.Add(new PlaceRating(searchArticleId, score));
            }

            neighbors.Sort((c, n) => n.Rating.CompareTo(c.Rating));

            return neighbors.Take(numArticles).ToList();
        }

        public void Save(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Create))
            using (GZipStream zip = new GZipStream(fs, CompressionMode.Compress))
            using (StreamWriter w = new StreamWriter(zip))
            {
                w.WriteLine(ratings.Users.Count);
                w.WriteLine(ratings.Users[0].PlaceRatings.Length);

                foreach (UserPlaceRatings t in ratings.Users)
                {
                    w.WriteLine(t.UserID);

                    foreach (double v in t.PlaceRatings)
                    {
                        w.WriteLine(v);
                    }
                }

                w.WriteLine(ratings.UserIndexToID.Count);

                foreach (int i in ratings.UserIndexToID)
                {
                    w.WriteLine(i);
                }

                w.WriteLine(ratings.ArticleIndexToID.Count);

                foreach (int i in ratings.ArticleIndexToID)
                {
                    w.WriteLine(i);
                }
            }
        }

        public void Load(string file)
        {
            ratings = new UserPlaceRatingsTable();

            using (FileStream fs = new FileStream(file, FileMode.Open))
            using (GZipStream zip = new GZipStream(fs, CompressionMode.Decompress))
            using (StreamReader r = new StreamReader(zip))
            {
                long total = long.Parse(r.ReadLine());
                int features = int.Parse(r.ReadLine());

                for (long i = 0; i < total; i++)
                {
                    int userId = int.Parse(r.ReadLine());
                    UserPlaceRatings uat = new UserPlaceRatings(userId, features);

                    for (int x = 0; x < features; x++)
                    {
                        uat.PlaceRatings[x] = double.Parse(r.ReadLine());
                    }

                    ratings.Users.Add(uat);
                }

                total = int.Parse(r.ReadLine());

                for (int i = 0; i < total; i++)
                {
                    ratings.UserIndexToID.Add(int.Parse(r.ReadLine()));
                }

                total = int.Parse(r.ReadLine());

                for (int i = 0; i < total; i++)
                {
                    ratings.ArticleIndexToID.Add(int.Parse(r.ReadLine()));
                }
            }
            
            FillTransposedRatings();
        }
    }
}
