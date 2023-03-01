using MassTransit;
using MediatR;
using YourBrand.Portal.Contracts;

namespace YourBrand.Portal.Consumers;

public sealed class UpdateStatusConsumer : IConsumer<UpdateStatus>
{
    private readonly IMediator mediator;

    public UpdateStatusConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<UpdateStatus> context)
    {
        var message = context.Message;

        await mediator.Send(new Todos.Commands.UpdateStatus(message.Id, (Todos.Dtos.TodoStatusDto)message.Status));
    }
}