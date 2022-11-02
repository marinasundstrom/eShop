using System;
using Site.Client;

namespace Site.Services;

public class CartService
{
    private readonly ICartClient cartClient;

    public CartService(ICartClient cartClient)
    {
        this.cartClient = cartClient;
    }

    public async Task<SiteCartDto> GetCartAsync() => await cartClient.GetCartAsync();
}
