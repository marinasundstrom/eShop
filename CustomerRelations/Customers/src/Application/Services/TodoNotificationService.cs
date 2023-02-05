using Microsoft.AspNetCore.SignalR;

using YourBrand.Customers.Application.Hubs;

namespace YourBrand.Customers.Application.Services;

public class TodoNotificationService : ITodoNotificationService
{
    private readonly IHubContext<TodosHub, ITodosHubClient> hubsContext;

    public TodoNotificationService(IHubContext<TodosHub, ITodosHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }


}