using Microsoft.AspNetCore.SignalR;

namespace YourBrand.StoreFront.Application.Features.Carts;

public class CartHubService : ICartHubService
{
    private readonly IHubContext<CartHub, ICartHubClient> cartHubContext;
    private readonly ICurrentUserService currentUserService;

    public CartHubService(
            IHubContext<CartHub, ICartHubClient> cartHubContext,
            ICurrentUserService currentUserService)
    {
        this.cartHubContext = cartHubContext;
        this.currentUserService = currentUserService;
    }

    public async Task UpdateCart()
    {
        var customerId = currentUserService.CustomerNo;
        var clientId = currentUserService.ClientId;

        var hubClient = customerId is not null
            ? cartHubContext.Clients.Group($"customer-{customerId}")
            : cartHubContext.Clients.Group($"cart-{clientId}");

        await hubClient.CartUpdated();
    }
}