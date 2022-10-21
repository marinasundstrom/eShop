using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Catalog.Domain.Entities;

namespace Catalog.Domain;

public interface IApplicationDbContext
{
    DbSet<ProductGroup> ProductGroups { get; }

    DbSet<Product> Products { get; }

    DbSet<ProductAttribute> ProductAttributes { get; }

    DbSet<AttributeGroup> AttributeGroups { get; }

    DbSet<Entities.Attribute> Attributes { get; }

    DbSet<AttributeValue> AttributeValues { get; }

    DbSet<ProductVariant> ProductVariants { get; }

    DbSet<ProductVariantOption> ProductVariantOptions { get; }

    DbSet<ProductVariantAttributeValue> ProductVariantAttributeValues { get; }

    DbSet<ProductOption> ProductOptions { get; }

    DbSet<OptionGroup> OptionGroups { get; }

    DbSet<Option> Options { get; }

    DbSet<OptionValue> OptionValues { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry Entry(object entity);
}