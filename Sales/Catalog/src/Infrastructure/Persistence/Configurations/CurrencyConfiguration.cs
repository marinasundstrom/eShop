using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Catalog.Infrastructure.Persistence.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Domain.Entities.Currency>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Currency> builder)
    {
        builder.ToTable("Currencies");

        builder.HasKey(x => x.Code);
    }
}
