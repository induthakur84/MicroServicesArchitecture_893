using Microsoft.EntityFrameworkCore;
using Order.Domains;

namespace Order.Data
{
    public class OrderDbContext:DbContext
    {

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { } 


        public DbSet<User> Users { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<OrderEntity> OrderEntities { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
        }
        
    }
}
