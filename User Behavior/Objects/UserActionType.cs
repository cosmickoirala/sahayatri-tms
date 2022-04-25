namespace UserBehavior.Objects
{
    public class UserActionType
    {
        public int UserID { get; set; }

        public double[] ActionTypeData { get; set; }

        public double Score { get; set; }

        public UserActionType(int userId, int actionTyoeCount)
        {
            UserID = userId;
            ActionTypeData = new double[actionTyoeCount];
        }
    }
}
