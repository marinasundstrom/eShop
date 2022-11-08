using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using YourBrand.StoreFront.Hubs;
using YourBrand.Sales;
using YourBrand.StoreFront.Application.Services;
using Microsoft.Extensions.Logging;
using MediatR;
using YourBrand.StoreFront.Application.UserProfiles;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Presentation.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("v{version:apiVersion}/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMediator mediator;

    public UserController(
        ILogger<UserController> logger,
        IMediator mediator)
    {
        _logger = logger;
        this.mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<UserProfileDto> GetProfile(CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetProfile(), cancellationToken);
    }

    [HttpGet("addresses")]
    public async Task<IEnumerable<YourBrand.Customers.AddressDto>> GetAddresses(CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetAddresses(), cancellationToken);
    }

    [HttpGet("orders")]
    public async Task<ItemsResult<OrderDto>> GetOrders(int page = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new GetOrders(page, pageSize), cancellationToken);
    }
}
