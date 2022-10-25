using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Inventory.Domain.Entities;

namespace YourBrand.Inventory.Domain;

public interface IApplicationDbContext
{
    DbSet<Site> Sites { get; }

    DbSet<Warehouse> Warehouses { get; }

    DbSet<WarehouseItem> WarehouseItems { get; }

    DbSet<Location> Locations { get; }

    DbSet<ItemGroup> ItemGroups { get; }

    DbSet<Item> Items { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    EntityEntry Entry(object entity);
}