using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using YourBrand.Payments.Domain.Entities;

namespace YourBrand.Payments.Domain;

public interface IApplicationDbContext
{
    DbSet<ReceiptStatus> ReceiptStatuses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}