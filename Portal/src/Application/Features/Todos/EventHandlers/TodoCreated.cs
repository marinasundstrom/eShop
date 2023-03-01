using System;
using MediatR;
using YourBrand.Portal.Common;
using YourBrand.Portal.Services;

namespace YourBrand.Portal.Todos.EventHandlers;

public sealed class TodoCreatedEventHandler : IDomainEventHandler<TodoCreated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoCreatedEventHandler(ITodoRepository todoRepository, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoCreated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.Created(todo.Id, todo.Title);
    }
}

