using YourBrand.Portal.Domain.Entities;

namespace YourBrand.Portal.Domain.Repositories;

public interface IRepository<T>
    where T : IAggregateRoot
{

}
