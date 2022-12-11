using YourBrand.Pricing.Domain.Entities;
using YourBrand.Pricing.Domain.Specifications;

namespace YourBrand.Pricing.Domain.Repositories;

public interface IOrderRepository : IRepository<Order, string>
{
    Task<Order?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default);
}
