using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Domain;

public interface IApplicationDbContext
{
    DbSet<Store> Stores { get; }

    DbSet<ItemGroup> ItemGroups { get; }

    DbSet<Item> Items { get; }

    DbSet<ItemAttribute> ItemAttributes { get; }

    DbSet<AttributeGroup> AttributeGroups { get; }

    DbSet<Entities.Attribute> Attributes { get; }

    DbSet<AttributeValue> AttributeValues { get; }

    DbSet<ItemOption> ItemOptions { get; }

    DbSet<ItemAttributeValue> ItemAttributeValues { get; }

    DbSet<ItemVariantOption> ItemVariantOptions { get; }

    DbSet<OptionGroup> OptionGroups { get; }

    DbSet<Option> Options { get; }

    DbSet<OptionValue> OptionValues { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry Entry(object entity);
}