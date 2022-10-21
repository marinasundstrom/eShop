using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Catalog.Domain.Entities;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductVariantAttributeValueConfiguration : IEntityTypeConfiguration<ProductVariantAttributeValue>
{
    public void Configure(EntityTypeBuilder<ProductVariantAttributeValue> builder)
    {
        builder.ToTable("ProductVariantAttributeValues");

        builder.HasOne(m => m.Value).WithMany(m => m.ProductVariantValues).OnDelete(DeleteBehavior.NoAction);
    }
}
