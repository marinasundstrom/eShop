using Microsoft.AspNetCore.SignalR;

namespace Site.Server.Hubs;

public class CartHub : Hub<ICartHubClient>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var httpContext = Context.GetHttpContext();

        if (httpContext is not null)
        {
            if (httpContext.Request.Query.TryGetValue("clientId", out var clientId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"cart-{clientId}");
            }

            var customerIdStr = httpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "CustomerId")?.Value;
            
            if (customerIdStr is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"customer-{customerIdStr}");
            }
        }
    }
}

public interface ICartHubClient 
{
    Task CartUpdated();
}