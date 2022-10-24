using YourBrand.Sales.Domain.Entities;

namespace YourBrand.Sales.Domain.Repositories;

public interface IRepository<T>
    where T : IAggregateRoot
{

}
