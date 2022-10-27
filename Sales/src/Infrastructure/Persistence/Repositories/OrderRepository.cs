using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Domain.Specifications;

namespace YourBrand.Sales.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<Order> dbSet;

    public OrderRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Order>();
    }

    public IQueryable<Order> GetAll()
    {
        //return dbSet.Where(new OrdersWithStatus(OrderStatus.Completed).Or(new OrdersWithStatus(OrderStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Order?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.OrderNo.Equals(id), cancellationToken);
    }

    public IQueryable<Order> GetAll(ISpecification<Order> specification)
    {
        return dbSet
            .Include(i => i.Status)
            .Include(i => i.Items)
            .Include(i => i.Assignee)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Order item)
    {
        dbSet.Add(item);
    }

    public void Remove(Order item)
    {
        dbSet.Remove(item);
    }
}
