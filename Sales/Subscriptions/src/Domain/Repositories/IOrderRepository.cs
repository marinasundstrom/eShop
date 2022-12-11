using YourBrand.Subscriptions.Domain.Entities;
using YourBrand.Subscriptions.Domain.Specifications;

namespace YourBrand.Subscriptions.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, string>
{
    Task<Order?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default);
}
