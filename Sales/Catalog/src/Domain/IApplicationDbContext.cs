using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Domain;

public interface IApplicationDbContext
{
    DbSet<Store> Stores { get; }

    DbSet<Brand> Brands { get; }

    DbSet<ProductGroup> ProductGroups { get; }

    DbSet<Product> Products { get; }

    DbSet<ProductAttribute> ProductAttributes { get; }

    DbSet<AttributeGroup> AttributeGroups { get; }

    DbSet<Entities.Attribute> Attributes { get; }

    DbSet<AttributeValue> AttributeValues { get; }

    DbSet<ProductOption> ProductOptions { get; }

    DbSet<ProductVariantOption> ProductVariantOptions { get; }

    DbSet<OptionGroup> OptionGroups { get; }

    DbSet<Option> Options { get; }

    DbSet<OptionValue> OptionValues { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry Entry(object entity);
}