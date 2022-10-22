using MediatR;
using YourBrand.Portal.Application.Common;
using YourBrand.Portal.Application.Services;
using YourBrand.Portal.Application.Todos.Dtos;

namespace YourBrand.Portal.Application.Todos.EventHandlers;

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
