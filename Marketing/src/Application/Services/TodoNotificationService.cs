using Microsoft.AspNetCore.SignalR;

using YourBrand.Marketing.Application.Hubs;
using YourBrand.Marketing.Application.Services;

namespace YourBrand.Marketing.Application.Services;

public class TodoNotificationService : ITodoNotificationService
{
    private readonly IHubContext<TodosHub, ITodosHubClient> hubsContext;

    public TodoNotificationService(IHubContext<TodosHub, ITodosHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }


}