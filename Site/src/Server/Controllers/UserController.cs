using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Site.Server.Hubs;
using Site.Shared;
using YourBrand.Sales;
using Site.Server.Services;

namespace Site.Server.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly YourBrand.Customers.ICustomersClient customersClient;
    private readonly YourBrand.Sales.IOrdersClient _ordersClient;
    private readonly ICurrentUserService currentUserService;

    public UserController(
        ILogger<UserController> logger,
        YourBrand.Customers.ICustomersClient customersClient,
        YourBrand.Sales.IOrdersClient ordersClient,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        this.customersClient = customersClient;
        this._ordersClient = ordersClient;
        this.currentUserService = currentUserService;
    }

    [HttpGet("profile")]
    public async Task<UserProfileDto> GetProfile(CancellationToken cancellation)
    {
        var customerId = currentUserService.CustomerNo;

        var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellation);

        return new UserProfileDto(customer.Id, customer.FirstName, customer.LastName, customer.Ssn);
    }

    [HttpGet("addresses")]
    public async Task<IEnumerable<YourBrand.Customers.AddressDto>> GetAddresses(CancellationToken cancellation)
    {
        var customerId = currentUserService.CustomerNo;

        var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellation);

        return new [] { customer.Address };
    }

    [HttpGet("orders")]
    public async Task<ItemsResultOfOrderDto> GetOrders(int page = 1, int pageSize = 10, CancellationToken cancellation = default)
    {
        var customerId = currentUserService.CustomerNo;

        var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellation);

        var orders = await _ordersClient.GetOrdersAsync(null, null, customer.Ssn, null, 1, 10, null, null, cancellation);

        return orders;
    }
}

public record UserProfileDto(int CustomerNo, string FirstName, string LastName, string SSN);