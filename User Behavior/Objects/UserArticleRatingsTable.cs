using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBehavior.Objects
{
    public class UserPlaceRatingsTable
    {
        public List<UserPlaceRatings> Users { get; set; }

        public List<int> UserIndexToID { get; set; }

        public List<int> ArticleIndexToID { get; set; }

        public UserPlaceRatingsTable()
        {
            Users = new List<UserPlaceRatings>();
            UserIndexToID = new List<int>();
            ArticleIndexToID = new List<int>();
        }
        
        public void AppendUserFeatures(double[][] userFeatures)
        {
            for (int i = 0; i < UserIndexToID.Count; i++)
            {
                Users[i].AppendRatings(userFeatures[i]);
            }
        }

        public void AppendArticleFeatures(double[][] articleFeatures)
        {
            for (int f = 0; f < articleFeatures[0].Length; f++)
            {
                UserPlaceRatings newFeature = new UserPlaceRatings(int.MaxValue, ArticleIndexToID.Count);
                
                for (int a = 0; a < ArticleIndexToID.Count; a++)
                {
                    newFeature.PlaceRatings[a] = articleFeatures[a][f];
                }

                Users.Add(newFeature);
            }
        }

        internal void AppendArticleFeatures(List<PlaceTypeCounts> placeTypes)
        {
            double[][] features = new double[placeTypes.Count][];

            for (int a = 0; a < placeTypes.Count; a++)
            {
                features[a] = new double[placeTypes[a].TypeCounts.Length];

                for (int f = 0; f < placeTypes[a].TypeCounts.Length; f++)
                {
                    features[a][f] = placeTypes[a].TypeCounts[f];
                }
            }

            AppendArticleFeatures(features);
        }

        public void SaveSparcityVisual(string file)
        {
            double min = Users.Min(x => x.PlaceRatings.Min());
            double max = Users.Max(x => x.PlaceRatings.Max());

            Bitmap b = new Bitmap(ArticleIndexToID.Count, UserIndexToID.Count);
            int numPixels = 0;

            for (int x = 0; x < ArticleIndexToID.Count; x++)
            {
                for (int y = 0; y < UserIndexToID.Count; y++)
                {
                    //int brightness = 255 - (int)(((UserPlaceRatings[y].PlaceRatings[x] - min) / (max - min)) * 255);
                    //brightness = Math.Max(0, Math.Min(255, brightness));

                    int brightness = Users[y].PlaceRatings[x] == 0 ? 255 : 0;

                    Color c = Color.FromArgb(brightness, brightness, brightness);

                    b.SetPixel(x, y, c);

                    numPixels += Users[y].PlaceRatings[x] != 0 ? 1 : 0;
                }
            }

            double sparcity = (double)numPixels / (ArticleIndexToID.Count * UserIndexToID.Count);

            b.Save(file);
        }

        /// <summary>
        /// Generate a CSV report of users and how many ratings they've given
        /// </summary>
        public void SaveUserRatingDistribution(string file)
        {
            int bucketSize = 4;            
            int maxRatings = Users.Max(x => x.PlaceRatings.Count(y => y != 0));
            List<int> buckets = new List<int>();

            for (int i = 0; i <= Math.Floor((double)maxRatings / bucketSize); i++)
            {
                buckets.Add(0);
            }

            foreach (UserPlaceRatings ratings in Users)
            {
                buckets[(int)Math.Floor((double)ratings.PlaceRatings.Count(x => x != 0) / bucketSize)]++;
            }

            using (StreamWriter w = new StreamWriter(file))
            {
                w.WriteLine("numArticlesRead,numUsers");

                for (int i = 0; i <= Math.Floor((double)maxRatings / bucketSize); i++)
                {
                    w.WriteLine("=\"" + (i * bucketSize) + "-" + (((i + 1) * bucketSize) - 1) + "\"," + buckets[i / bucketSize]);
                }
            }
        }

        /// <summary>
        /// Generate a CSV report of articles and how many ratings they've gotten
        /// </summary>
        public void SaveArticleRatingDistribution(string file)
        {
            int bucketSize = 2;
            int maxRatings = 3000;
            List<int> buckets = new List<int>();

            for (int i = 0; i <= Math.Floor((double)maxRatings / bucketSize); i++)
            {
                buckets.Add(0);
            }

            for (int i = 0; i < ArticleIndexToID.Count; i ++)
            {
                int readers = Users.Select(x => x.PlaceRatings[i]).Count(x => x != 0);
                buckets[(int)Math.Floor((double)readers / bucketSize)]++;
            }
            
            while (buckets[buckets.Count - 1] == 0)
            {
                buckets.RemoveAt(buckets.Count - 1);
            }

            using (StreamWriter w = new StreamWriter(file))
            {
                w.WriteLine("numReaders,numArticles");

                for (int i = 0; i < buckets.Count; i++)
                {
                    w.WriteLine("=\"" + (i * bucketSize) + "-" + (((i + 1) * bucketSize) - 1) + "\"," + buckets[i]);
                }
            }
        }
    }
}
