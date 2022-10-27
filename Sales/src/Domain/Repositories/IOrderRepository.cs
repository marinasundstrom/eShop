using YourBrand.Sales.Domain.Entities;
using YourBrand.Sales.Domain.Specifications;

namespace YourBrand.Sales.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    IQueryable<Order> GetAll();
    IQueryable<Order> GetAll(ISpecification<Order> specification);
    Task<Order?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Order item);
    void Remove(Order item);
}
