namespace Shahayatri.Models
{
    public class User
    {
       public int UserId { get; set; }
       public string Name { get; set; }
       public string Address { get; set; }
       public int Contact { get; set; }
       public string EmailAddress { get; set; }

       public  User GetUser()
       {
           User user = new User();
            user.Name = "Kamal Pandit";
           user.Address="Kapan ,Kathmandu";
           user.Contact=986098848;
           user.EmailAddress="testuser@email.com";
           return user;

       }
       

    }
}