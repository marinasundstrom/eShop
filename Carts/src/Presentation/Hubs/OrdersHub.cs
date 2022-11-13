using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace YourBrand.Carts.Presentation.Hubs;

/*
[Authorize]
public sealed class OrdersHub : Hub<IOrdersHubClient>
{
    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is not null)
        {
            if (httpContext.Request.Query.TryGetValue("itemId", out var itemId))
            {
                Groups.AddToGroupAsync(this.Context.ConnectionId, $"item-{itemId}");
            }
        }

        return base.OnConnectedAsync();
    }
}
*/
