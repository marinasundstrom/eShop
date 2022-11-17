using YourBrand.CustomerService.Domain.Entities;
using YourBrand.CustomerService.Domain.Specifications;

namespace YourBrand.CustomerService.Domain.Repositories;

public interface IIssueRepository : IRepository<Issue, string>
{
    Task<Issue?> FindByTagAsync(string tag, CancellationToken cancellationToken = default);

    Task DeleteIssueItem(string id, string itemId, CancellationToken cancellationToken = default);
}
