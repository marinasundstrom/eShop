using MediatR;
using YourBrand.Portal.Common;
using YourBrand.Portal.Services;

namespace YourBrand.Portal.Todos.EventHandlers;

public sealed class TodoUpdatedEventHandler : IDomainEventHandler<TodoUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.Updated(todo.Id, todo.Title);
    }
}
