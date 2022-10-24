using YourBrand.Sales.Domain.Entities;
using YourBrand.Sales.Domain.Specifications;

namespace YourBrand.Sales.Domain.Repositories;

public interface IUserRepository : IRepository<User>
{
    IQueryable<User> GetAll();
    IQueryable<User> GetAll(ISpecification<User> specification);
    Task<User?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    void Add(User user);
    void Remove(User user);
}