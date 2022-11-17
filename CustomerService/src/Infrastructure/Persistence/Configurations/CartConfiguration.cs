using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.CustomerService.Infrastructure.Persistence.Configurations;

public sealed class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("CustomerService");

        builder
            .HasMany(x => x.Items)
            .WithOne(x => x.Issue)
            .OnDelete(DeleteBehavior.Cascade);  // Causes dependent entity to be deleted
    }
}