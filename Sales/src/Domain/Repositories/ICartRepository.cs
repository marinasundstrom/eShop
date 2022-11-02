using YourBrand.Sales.Domain.Entities;
using YourBrand.Sales.Domain.Specifications;

namespace YourBrand.Sales.Domain.Repositories;

public interface ICartRepository : IRepository<Cart>
{
    IQueryable<Cart> GetAll();
    IQueryable<Cart> GetAll(ISpecification<Cart> specification);
    Task<Cart?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<Cart?> FindByTagAsync(string tag, CancellationToken cancellationToken = default);
    void Add(Cart item);
    void Remove(Cart item);
}
