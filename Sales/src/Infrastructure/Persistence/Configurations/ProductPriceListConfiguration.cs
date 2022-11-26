using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Sales.Infrastructure.Persistence.Configurations;

public sealed class ProductPriceListConfiguration : IEntityTypeConfiguration<ProductPriceList>
{
    public void Configure(EntityTypeBuilder<ProductPriceList> builder)
    {
        builder.ToTable("ProductPriceLists");

        builder.HasOne(x => x.CreatedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.LastModifiedBy)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}
