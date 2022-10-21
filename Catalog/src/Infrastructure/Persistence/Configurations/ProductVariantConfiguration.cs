using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable("ProductVariants");

        builder
            .HasMany(x => x.ProductVariantOptions)
            .WithOne(x => x.ProductVariant)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
