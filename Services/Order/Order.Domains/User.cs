namespace Order.Domains
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;    
        //one to one relationship
        // means one user can have only one userprfile
        public UserProfile? UserProfile { get; set; }
        //one to many relationship
        public ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>(); 
    }
}
