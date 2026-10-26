namespace Order.Domains
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description {  get; set; }    


        //one to one relationship

        // means one user can have only one userprfile


        public UserProfile UserProfile { get; set; }



        //one to many relationship
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>(); 
    }
}
