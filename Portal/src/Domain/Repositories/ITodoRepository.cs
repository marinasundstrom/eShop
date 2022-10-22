using YourBrand.Portal.Domain.Entities;
using YourBrand.Portal.Domain.Specifications;

namespace YourBrand.Portal.Domain.Repositories;

public interface ITodoRepository : IRepository<Todo>
{
    IQueryable<Todo> GetAll();
    IQueryable<Todo> GetAll(ISpecification<Todo> specification);
    Task<Todo?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    void Add(Todo item);
    void Remove(Todo item);
}
