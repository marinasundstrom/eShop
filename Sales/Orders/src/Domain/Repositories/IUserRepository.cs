using YourBrand.Orders.Domain.Entities;
using YourBrand.Orders.Domain.Specifications;

namespace YourBrand.Orders.Domain.Repositories;

public interface IUserRepository : IRepository<User, string>
{

}