using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBehavior.Objects
{
    public class PlaceTypeCounts
    {
        public int PlaceID { get; set; }

        public double[] TypeCounts { get; set; }

        public PlaceTypeCounts(int PlaceTagId, int numType)
        {
            PlaceID = PlaceTagId;
            TypeCounts = new double[numType];
        }
    }
}
