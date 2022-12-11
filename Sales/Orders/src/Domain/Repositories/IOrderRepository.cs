using YourBrand.Orders.Domain.Entities;
using YourBrand.Orders.Domain.Specifications;

namespace YourBrand.Orders.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, string>
{
    Task<Order?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default);
}
