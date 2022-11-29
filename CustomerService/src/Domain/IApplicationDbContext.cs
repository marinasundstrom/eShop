using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.CustomerService.Domain.Entities;

namespace YourBrand.CustomerService.Domain;

public interface IApplicationDbContext
{
    DbSet<TicketStatus> TicketStatuses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}