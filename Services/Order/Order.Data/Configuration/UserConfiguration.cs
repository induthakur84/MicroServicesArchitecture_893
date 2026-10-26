using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domains;

namespace Order.Data.Configuration
{
    public class UserConfigurationn:IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u=>u.Name).IsRequired();
            builder.HasIndex(u => u.Name).IsUnique();
        }
    }
}
