using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class ItemVariantOptionConfiguration : IEntityTypeConfiguration<ItemVariantOption>
{
    public void Configure(EntityTypeBuilder<ItemVariantOption> builder)
    {
        builder.ToTable("ItemVariantOption");

        builder
            .HasOne(x => x.Item)
            .WithMany(x => x.ItemVariantOptions)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Item)
            .WithMany(x => x.ItemVariantOptions)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Option)
            .WithMany(x => x.ItemVariantOptions)
            .OnDelete(DeleteBehavior.NoAction);
    }
}