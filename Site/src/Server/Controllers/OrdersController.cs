using Microsoft.AspNetCore.Mvc;
using Site.Shared;
using YourBrand.Sales;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly YourBrand.Sales.IOrdersClient _ordersClient;

    public OrdersController(ILogger<OrdersController> logger, YourBrand.Sales.IOrdersClient ordersClient)
    {
        _logger = logger;
        _ordersClient = ordersClient;
    }

    [HttpGet]
    public async Task<ItemsResultOfOrderDto> GetOrders(int[]? status = null, string? assigneeId = null, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _ordersClient.GetOrdersAsync(status, null, null, assigneeId, page - 1, pageSize, sortBy, sortDirection, cancellationToken);
    }

    
    [HttpGet("{id}")]
    public async Task<OrderDto?> GetOrder(int id, CancellationToken cancellationToken = default)
    {
        return await _ordersClient.GetOrderByIdAsync(id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        await _ordersClient.DeleteOrderAsync(id, cancellationToken);
    }
}
