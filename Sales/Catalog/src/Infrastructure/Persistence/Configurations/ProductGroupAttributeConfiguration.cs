using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class ProductGroupAttributeConfiguration : IEntityTypeConfiguration<ProductGroupAttribute>
{
    public void Configure(EntityTypeBuilder<ProductGroupAttribute> builder)
    {
        builder.ToTable("ProductGroupAttributes");
    }
}
