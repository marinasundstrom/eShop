using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Shops.Domain.Entities;

namespace YourBrand.Shops.Domain;

public interface IApplicationDbContext
{
    DbSet<Shop> Shops { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}