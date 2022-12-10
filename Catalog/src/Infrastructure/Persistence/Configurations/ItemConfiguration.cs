using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder
            .HasMany(p => p.Options)
            .WithMany(p => p.Products)
            .UsingEntity<ProductOption>();

        builder
            .HasMany(p => p.Attributes)
            .WithMany(p => p.Products)
            .UsingEntity<ProductAttribute>();

        builder
            .HasOne(x => x.ParentProduct)
            .WithMany(x => x.Variants)
            .HasForeignKey(x => x.ParentProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(x => x.ProductOptions)
            .WithOne(x => x.Product)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(p => p.Group)
            .WithMany(p => p.Products);

        builder
            .HasOne(p => p.Group2)
            .WithMany(p => p.Products2);

        builder
            .HasOne(p => p.Group3)
            .WithMany(p => p.Products3);
    }
}
