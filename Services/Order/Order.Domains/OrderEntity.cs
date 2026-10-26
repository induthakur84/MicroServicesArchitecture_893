namespace Order.Domains
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
        public string Address { get; set; }


        //foreign key
        //USerid connect the  order table with the usertable


        public int UserId { get; set; }


        //Navigation Properly
        //Each order belongs to onlty one user

        public User User { get; set; }
    }
}
