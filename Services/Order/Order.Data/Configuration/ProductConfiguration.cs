using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domains;

namespace Order.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>

    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasMany(p => p.Categories)
                 .WithMany( p => p.Products)
                 .UsingEntity(j => j.ToTable("ProductCategories"));
        }
    }
}
