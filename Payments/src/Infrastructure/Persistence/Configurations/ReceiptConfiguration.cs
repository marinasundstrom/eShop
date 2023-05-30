using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Payments.Infrastructure.Persistence.Configurations;

public sealed class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.ToTable("Receipts");

        //builder.HasKey(o => new { o.CompanyId , o.ReceiptNo });

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.Receipt)
            .OnDelete(DeleteBehavior.Cascade); // Causes dependent entity to be deleted

        builder.OwnsOne(x => x.BillingDetails, x => x.OwnsOne(z => z.Address));

        builder.OwnsOne(x => x.ShippingDetails, x => x.OwnsOne(z => z.Address));
    }
}
