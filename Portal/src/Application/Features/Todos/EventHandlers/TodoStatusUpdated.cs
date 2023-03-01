using MediatR;
using YourBrand.Portal.Common;
using YourBrand.Portal.Services;
using YourBrand.Portal.Todos.Dtos;

namespace YourBrand.Portal.Todos.EventHandlers;

public sealed class TodoStatusUpdatedEventHandler : IDomainEventHandler<TodoStatusUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly ICurrentUserService currentUserService;
    private readonly IEmailService emailService;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoStatusUpdatedEventHandler(ITodoRepository todoRepository, ICurrentUserService currentUserService, IEmailService emailService, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.currentUserService = currentUserService;
        this.emailService = emailService;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoStatusUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        await todoNotificationService.StatusUpdated(todo.Id, (TodoStatusDto)todo.Status);

        if (todo.AssigneeId is not null && todo.LastModifiedById != todo.AssigneeId)
        {
            await emailService.SendEmail(todo.Assignee!.Email,
                $"Status of \"{todo.Title}\" [{todo.Id}] changed to {notification.NewStatus}.",
                $"{todo.LastModifiedBy!.Name} changed status of \"{todo.Title}\" [{todo.Id}] from {notification.OldStatus} to {notification.NewStatus}.");
        }
    }
}
