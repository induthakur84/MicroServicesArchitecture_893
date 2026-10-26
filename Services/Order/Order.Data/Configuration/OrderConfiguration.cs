using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domains;

namespace Order.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasOne(p=>p.User)
                .WithMany(p=>p.Orders)
                .HasForeignKey(o=>o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
