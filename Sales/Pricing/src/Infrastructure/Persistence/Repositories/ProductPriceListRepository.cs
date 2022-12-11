using Microsoft.EntityFrameworkCore;
using YourBrand.Pricing.Domain.Specifications;

namespace YourBrand.Pricing.Infrastructure.Persistence.Repositories;

public sealed class ProductPriceListRepository : IProductPriceListRepository
{
    readonly ApplicationDbContext context;
    readonly DbSet<ProductPriceList> dbSet;

    public ProductPriceListRepository(ApplicationDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<ProductPriceList>();
    }

    public IQueryable<ProductPriceList> GetAll()
    {
        //return dbSet.Where(new ProductPriceListsWithStatus(ProductPriceListStatus.Completed).Or(new ProductPriceListsWithStatus(ProductPriceListStatus.OnHold))).AsQueryable();

        return dbSet.AsQueryable();
    }

    public async Task<ProductPriceList?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.ProductPrices)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public async Task<ProductPriceList?> FindByNoAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbSet
            .Include(i => i.ProductPrices)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
    }

    public IQueryable<ProductPriceList> GetAll(ISpecification<ProductPriceList> specification)
    {
        return dbSet
            .Include(i => i.ProductPrices)
            .Include(i => i.CreatedBy)
            .Include(i => i.LastModifiedBy)
            .Where(specification.Criteria);
    }

    public void Add(ProductPriceList item)
    {
        dbSet.Add(item);
    }

    public void Remove(ProductPriceList item)
    {
        dbSet.Remove(item);
    }
}
