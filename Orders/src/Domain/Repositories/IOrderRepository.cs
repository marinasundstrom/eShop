using YourBrand.Orders.Domain.Entities;
using YourBrand.Orders.Domain.Specifications;

namespace YourBrand.Orders.Domain.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    IQueryable<Order> GetAll();
    IQueryable<Order> GetAll(ISpecification<Order> specification);
    Task<Order?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Order item);
    void Remove(Order item);
}
