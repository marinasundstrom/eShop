using YourBrand.Portal.Application.Common;
using YourBrand.Portal.Application.Services;
using YourBrand.Portal.Domain.Entities;

namespace YourBrand.Portal.Application.Todos.EventHandlers;

public sealed class TodoDeletedEventHandler : IDomainEventHandler<TodoDeleted>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDeletedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoDeleted notification, CancellationToken cancellationToken)
    {
        await todoNotificationService.Deleted(notification.TodoId, notification.Title);
    }
}

