using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Pricing.Infrastructure.Persistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        //builder.HasKey(o => new { o.CompanyId , o.OrderNo });

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .OnDelete(DeleteBehavior.Cascade); // Causes dependent entity to be deleted

        builder.OwnsOne(x => x.BillingDetails, x => x.OwnsOne(z => z.Address));

        builder.OwnsOne(x => x.ShippingDetails, x => x.OwnsOne(z => z.Address));
    }
}
