using Microsoft.AspNetCore.Mvc;
using Site.Shared;
using YourBrand.Sales;

namespace Site.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ILogger<CartsController> _logger;
    private readonly YourBrand.Sales.ICartsClient _cartsClient;

    public CartsController(ILogger<CartsController> logger, YourBrand.Sales.ICartsClient cartsClient)
    {
        _logger = logger;
        _cartsClient = cartsClient;
    }

    [HttpGet]
    public async Task<ItemsResultOfCartDto> GetCarts(YourBrand.Sales.CartStatusDto? status = null, string? assignedTo = null, int page = 1, int pageSize = 10, string? searchString = null, string? sortBy = null, SortDirection? sortDirection = null, CancellationToken cancellationToken = default)
    {
        return await _cartsClient.GetCartsAsync(status, assignedTo, page - 1, pageSize, sortBy, sortDirection, cancellationToken);
    }

    
    [HttpGet("{id}")]
    public async Task<CartDto?> GetCart(string id, CancellationToken cancellationToken = default)
    {
        return await _cartsClient.GetCartByIdAsync(id, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteCartAsync(string id, CancellationToken cancellationToken = default)
    {
        await _cartsClient.DeleteCartAsync(id, cancellationToken);
    }
}
