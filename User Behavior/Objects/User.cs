namespace UserBehavior.Objects
{
    public class User
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public User(int _UserId, string _Name)
        {
            UserId = _UserId;
            Name = _Name;
        }
    }
}
