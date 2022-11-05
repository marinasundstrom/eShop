using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Marketing.Domain.Entities;

namespace YourBrand.Marketing.Domain;

public interface IApplicationDbContext
{
    DbSet<Contact> Contacts { get; }

    DbSet<Campaign> Campaigns { get; }

    DbSet<Address> Addresses { get; }

    DbSet<Discount> Discounts { get; }

    DbSet<Event> Events { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}