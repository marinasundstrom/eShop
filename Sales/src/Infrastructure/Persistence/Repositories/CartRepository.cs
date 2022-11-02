using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Domain.Specifications;

namespace YourBrand.Sales.Infrastructure.Persistence.Repositories;

public sealed class CartRepository : ICartRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<Cart> dbSet;

    public CartRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Cart>();
    }

    public IQueryable<Cart> GetAll()
    {
        //return dbSet.Where(new CartsWithStatus(CartStatus.Completed).Or(new CartsWithStatus(CartStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Cart?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<Cart?> FindByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Tag == tag, cancellationToken);
    }

    public IQueryable<Cart> GetAll(ISpecification<Cart> specification)
    {
        return dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Cart item)
    {
        dbSet.Add(item);
    }

    public void Remove(Cart item)
    {
        dbSet.Remove(item);
    }
}
