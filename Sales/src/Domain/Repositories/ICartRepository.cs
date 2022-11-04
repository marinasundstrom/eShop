using YourBrand.Sales.Domain.Entities;
using YourBrand.Sales.Domain.Specifications;

namespace YourBrand.Sales.Domain.Repositories;

public interface ICartRepository : IRepository<Cart, string>
{
    Task<Cart?> FindByTagAsync(string tag, CancellationToken cancellationToken = default);

    Task DeleteCartItem(string id, string itemId, CancellationToken cancellationToken = default);
}
