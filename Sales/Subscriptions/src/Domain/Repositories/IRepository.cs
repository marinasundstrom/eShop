using YourBrand.Subscriptions.Domain.Entities;
using YourBrand.Subscriptions.Domain.Specifications;

namespace YourBrand.Subscriptions.Domain.Repositories;

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
