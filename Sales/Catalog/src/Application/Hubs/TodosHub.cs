using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace YourBrand.Catalog.Hubs;

[Authorize]
public sealed class TodosHub : Hub<ITodosHubClient>
{
    public override Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext is not null)
        {
            if (httpContext.Request.Query.TryGetValue("productId", out var productId))
            {
                Groups.AddToGroupAsync(this.Context.ConnectionId, $"item-{productId}");
            }
        }

        return base.OnConnectedAsync();
    }
}