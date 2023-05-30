using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Domain.Specifications;

namespace YourBrand.Payments.Infrastructure.Persistence.Repositories;

public sealed class ReceiptRepository : IReceiptRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<Receipt> dbSet;

    public ReceiptRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Receipt>();
    }

    public IQueryable<Receipt> GetAll()
    {
        //return dbSet.Where(new ReceiptsWithStatus(ReceiptStatus.Completed).Or(new ReceiptsWithStatus(ReceiptStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Receipt?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<Receipt?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.ReceiptNo.Equals(orderNo), cancellationToken);
    }

    public IQueryable<Receipt> GetAll(ISpecification<Receipt> specification)
    {
        return dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Receipt item)
    {
        dbSet.Add(item);
    }

    public void Remove(Receipt item)
    {
        dbSet.Remove(item);
    }
}
