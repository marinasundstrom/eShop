using YourBrand.Orders.Domain.Entities;
using YourBrand.Orders.Domain.Specifications;

namespace YourBrand.Orders.Domain.Repositories;

public interface IRepository<T, TId>
    where T : AggregateRoot<TId>
    where TId : notnull
{
    IQueryable<T> GetAll();
    IQueryable<T> GetAll(ISpecification<T> specification);
    Task<T?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);
    void Add(T entity);
    void Remove(T entity);
}
