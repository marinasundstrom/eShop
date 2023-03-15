using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class ProductGroupConfiguration : IEntityTypeConfiguration<ProductGroup>
{
    public void Configure(EntityTypeBuilder<ProductGroup> builder)
    {
        builder.ToTable("ProductGroups");
        //builder.HasQueryFilter(i => i.Deleted == null);

        builder
            .Property(x => x.Handle)
            .HasMaxLength(150);

        builder
            .Property(x => x.Path)
            .HasMaxLength(150);

        builder.HasIndex(x => x.Handle);
        builder.HasIndex(x => x.Path);
    }
}
