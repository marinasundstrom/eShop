using YourBrand.Payments.Domain.Entities;
using YourBrand.Payments.Domain.Specifications;

namespace YourBrand.Payments.Domain.Repositories;

public interface IReceiptRepository : IRepository<Receipt, string>
{
    Task<Receipt?> FindByNoAsync(int orderNo, CancellationToken cancellationToken = default);
}
