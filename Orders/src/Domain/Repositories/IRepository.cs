using YourBrand.Orders.Domain.Entities;

namespace YourBrand.Orders.Domain.Repositories;

public interface IRepository<T>
    where T : IAggregateRoot
{

}
