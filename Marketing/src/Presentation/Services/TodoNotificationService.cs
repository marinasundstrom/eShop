using Microsoft.AspNetCore.SignalR;
using YourBrand.Marketing.Application.Services;

namespace YourBrand.Marketing.Presentation.Hubs;

public class TodoNotificationService : ITodoNotificationService
{
    private readonly IHubContext<TodosHub, ITodosHubClient> hubsContext;

    public TodoNotificationService(IHubContext<TodosHub, ITodosHubClient> hubsContext)
    {
        this.hubsContext = hubsContext;
    }

   
}