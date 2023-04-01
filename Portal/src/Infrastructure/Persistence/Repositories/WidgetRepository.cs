using Microsoft.EntityFrameworkCore;
using YourBrand.Portal.Domain.Specifications;

namespace YourBrand.Portal.Infrastructure.Persistence.Repositories;

public sealed class WidgetRepository : IWidgetRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<Widget> dbSet;

    public WidgetRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Widget>();
    }

    public IQueryable<Widget> GetAll()
    {
        //return dbSet.Where(new WidgetsWithStatus(WidgetStatus.Completed).Or(new WidgetsWithStatus(WidgetStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Widget?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            //.Include(i => i.CreatedBy)
            //.Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public IQueryable<Widget> GetAll(ISpecification<Widget> specification)
    {
        return dbSet
            //.Include(i => i.CreatedBy)
            //.Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Widget item)
    {
        dbSet.Add(item);
    }

    public void Remove(Widget item)
    {
        dbSet.Remove(item);
    }
}
