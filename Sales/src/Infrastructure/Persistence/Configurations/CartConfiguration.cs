using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Sales.Infrastructure.Persistence.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.Cart)
            .OnDelete(DeleteBehavior.Cascade);  // Causes dependent entity to be deleted
    }
}