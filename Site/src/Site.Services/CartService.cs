using System;

namespace Site.Services;

public class CartService
{
    private readonly ICartClient cartClient;
    private readonly CartHubClient cartHubClient;
    private bool initialized = false;

    public CartService(ICartClient cartClient, CartHubClient cartHubClient)
    {
        this.cartClient = cartClient;
        this.cartHubClient = cartHubClient;
    }

    public bool IsConnected => cartHubClient.IsConnected;

    public async Task Start(string baseUri, string clientId)
    {
        if (!initialized)
        {
            cartHubClient.CartUpdated += OnCartUpdated;

            await cartHubClient.StartAsync(baseUri, clientId);

            initialized = true;
        }
    }

    private async void OnCartUpdated(object? sender, EventArgs eventArgs)
    {
        await Reload();
    }

    public async Task Stop()
    {
        cartHubClient.CartUpdated -= OnCartUpdated;

        await cartHubClient.StopAsync();
    }

    public async Task DisposeAsync()
    {
        cartHubClient.CartUpdated -= OnCartUpdated;

        await cartHubClient.DisposeAsync();
    }

    public SiteCartDto? Cart { get; private set; }

    public async Task Reload()
    {
        Cart = await cartClient.GetCartAsync();

        if (CartUpdated is null) return;

        await CartUpdated.Invoke();
    }

    public async Task ReconnectAndReload()
    {
        await cartHubClient.RestartAsync();
        await Reload();
    }

    public Func<Task>? CartUpdated;
}
