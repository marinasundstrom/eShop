namespace YourBrand.StoreFront.Authentication.Services
{
    public interface ICustomerService
    {
        Task AddUser(User user, CancellationToken cancellationToken = default);

        Task<User?> GetUserByCustomerId(int customerId, CancellationToken cancellationToken = default);

        Task UpdateUser(User user, CancellationToken cancellationToken = default);
    }
}