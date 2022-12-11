using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Customers.Domain.Entities;

namespace YourBrand.Customers.Domain;

public interface IApplicationDbContext
{
    DbSet<Customer> Customers { get; }

    DbSet<Person> Persons { get; }

    DbSet<Organization> Organizations { get; }

    DbSet<Address> Addresses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}