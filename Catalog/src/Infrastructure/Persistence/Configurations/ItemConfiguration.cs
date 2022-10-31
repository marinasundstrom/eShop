using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Items");

        builder
            .HasMany(p => p.Options)
            .WithMany(p => p.Items)
            .UsingEntity<ItemOption>();

        builder
            .HasMany(p => p.Attributes)
            .WithMany(p => p.Items)
            .UsingEntity<ItemAttribute>();

        builder
            .HasOne(x => x.ParentItem)
            .WithMany(x => x.Variants)
            .HasForeignKey(x => x.ParentItemId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(x => x.ItemOptions)
            .WithOne(x => x.Item)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(p => p.Group)
            .WithMany(p => p.Items);

        builder
            .HasOne(p => p.Group2)
            .WithMany(p => p.Items2);

        builder
            .HasOne(p => p.Group3)
            .WithMany(p => p.Items3);
    }
}
