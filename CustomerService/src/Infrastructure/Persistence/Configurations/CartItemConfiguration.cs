using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.CustomerService.Infrastructure.Persistence.Configurations;

public sealed class IssueItemConfiguration : IEntityTypeConfiguration<IssueItem>
{
    public void Configure(EntityTypeBuilder<IssueItem> builder)
    {
        builder.ToTable("IssueItems");
    }
}
