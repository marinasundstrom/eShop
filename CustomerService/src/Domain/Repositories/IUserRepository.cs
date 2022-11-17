using YourBrand.CustomerService.Domain.Entities;
using YourBrand.CustomerService.Domain.Specifications;

namespace YourBrand.CustomerService.Domain.Repositories;

public interface IUserRepository : IRepository<User, string>
{

}