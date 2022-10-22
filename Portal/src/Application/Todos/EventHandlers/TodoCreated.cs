using System;
using MediatR;
using YourBrand.Portal.Application.Common;
using YourBrand.Portal.Application.Services;

namespace YourBrand.Portal.Application.Todos.EventHandlers;

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

