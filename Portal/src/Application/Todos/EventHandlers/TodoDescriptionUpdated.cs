using MediatR;
using YourBrand.Portal.Application.Common;
using YourBrand.Portal.Application.Services;

namespace YourBrand.Portal.Application.Todos.EventHandlers;

public sealed class TodoDescriptionUpdatedEventHandler : IDomainEventHandler<TodoDescriptionUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoDescriptionUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoDescriptionUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.DescriptionUpdated(todo.Id, todo.Description);
    }
}

