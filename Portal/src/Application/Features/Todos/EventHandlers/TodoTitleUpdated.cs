using MediatR;
using YourBrand.Portal.Common;
using YourBrand.Portal.Services;
using YourBrand.Portal.Todos.Dtos;

namespace YourBrand.Portal.Todos.EventHandlers;

public sealed class TodoTitleUpdatedEventHandler : IDomainEventHandler<TodoTitleUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoTitleUpdatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoTitleUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.TitleUpdated(todo.Id, todo.Title);
    }
}
