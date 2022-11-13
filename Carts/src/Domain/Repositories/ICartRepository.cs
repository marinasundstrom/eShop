using YourBrand.Carts.Domain.Entities;
using YourBrand.Carts.Domain.Specifications;

namespace YourBrand.Carts.Domain.Repositories;

public interface ICartRepository : IRepository<Cart, string>
{
    Task<Cart?> FindByTagAsync(string tag, CancellationToken cancellationToken = default);

    Task DeleteCartItem(string id, string itemId, CancellationToken cancellationToken = default);
}
