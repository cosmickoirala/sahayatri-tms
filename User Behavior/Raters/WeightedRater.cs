using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserBehavior.Abstractions;
using UserBehavior.Objects;

namespace UserBehavior.Raters
{
    public class WeightedRater : IRater
    {
        private double downVoteWeight;
        private double baseWeight;
        private double upVoteWeight;
        private double viewWeight;
        private double wishListWeight;
        private double maxWeight;

        public WeightedRater()
            : this (0.1, 3.0, 1.0, 0.5, 0.5, 5.0)
        {
        }

        public WeightedRater(double downVote, double baseVal, double upvote, double view, double wishList)
            : this (downVote, baseVal, upvote, view, wishList, 5.0)
        {
        }

        public WeightedRater(double downVote, double baseVal, double upvote, double view, double wishList, double max)
        {
            downVoteWeight = downVote;
            baseWeight = baseVal;
            upVoteWeight = upvote;
            viewWeight = view;
            wishListWeight = wishList;
            maxWeight = max;
        }

        public double GetRating(List<UserAction> actions)
        {
            double rating;
            string lastVote = actions.LastOrDefault(x => x.Action == "DownVote" || x.Action == "UpVote")?.Action ?? "";
            int viewCount = actions.Count(x => x.Action == "View");
            bool downloaded = actions.Any(x => x.Action == "wishList");

            if (lastVote == "DownVote")
            {
                rating = downVoteWeight;
            }
            else
            {
                rating = baseWeight;

                rating += lastVote == "UpVote" ? upVoteWeight : 0.0;
                rating += viewCount * viewWeight;
                rating += downloaded ? wishListWeight : 0.0;
            }

            return Math.Min(maxWeight, Math.Max(0.0, rating));
        }
    }
}
