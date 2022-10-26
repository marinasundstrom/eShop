using YourBrand.Portal.Application.Common;
using YourBrand.Portal.Application.Services;

namespace YourBrand.Portal.Application.Todos.EventHandlers;

public sealed class TodoAssignedUserEventHandler : IDomainEventHandler<TodoAssignedUserUpdated>
{
    private readonly ITodoRepository todoRepository;
    private readonly IEmailService emailService;
    private readonly ITodoNotificationService todoNotificationService;

    public TodoAssignedUserEventHandler(ITodoRepository todoRepository, IEmailService emailService, ITodoNotificationService todoNotificationService)
    {
        this.todoRepository = todoRepository;
        this.emailService = emailService;
        this.todoNotificationService = todoNotificationService;
    }

    public async Task Handle(TodoAssignedUserUpdated notification, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.FindByIdAsync(notification.TodoId, cancellationToken);

        if (todo is null)
            return;

        if (todo.AssigneeId is not null && todo.LastModifiedById != todo.AssigneeId)
        {
            await emailService.SendEmail(
                todo.Assignee!.Email,
                $"You were assigned to \"{todo.Title}\" [{todo.Id}].",
                $"{todo.LastModifiedBy!.Name} assigned {todo.Assignee.Name} to \"{todo.Title}\" [{todo.Id}].");
        }
    }
}
