namespace Order.Domains
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Address { get; set; }


        //Foreign key
        // UserID we connect userprofile with user

        public int UserId { get; set; }

        //Navigation properly

        public User User { get; set; }  
    }
}
