using Microsoft.EntityFrameworkCore;
using YourBrand.CustomerService.Domain.Specifications;

namespace YourBrand.CustomerService.Infrastructure.Persistence.Repositories;

public sealed class IssueRepository : IIssueRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<Issue> dbSet;

    public IssueRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<Issue>();
    }

    public IQueryable<Issue> GetAll()
    {
        //return dbSet.Where(new CustomerServiceWithStatus(CustomerServicetatus.Completed).Or(new CustomerServiceWithStatus(CustomerServicetatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<Issue?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<Issue?> FindByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Tag == tag, cancellationToken);
    }

    public IQueryable<Issue> GetAll(ISpecification<Issue> specification)
    {
        return dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(Issue item)
    {
        dbSet.Add(item);
    }

    public void Remove(Issue item)
    {
        dbSet.Remove(item);
    }

    public async Task DeleteIssueItem(string id, string itemId, CancellationToken cancellationToken = default)
    {
        var issue = await dbSet
            .Include(i => i.Items)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstAsync(x => x.Id.Equals(id), cancellationToken);

        var item = issue.Items.First(x => x.Id == itemId);

        context.Set<IssueItem>().Remove(item);
    }
}
