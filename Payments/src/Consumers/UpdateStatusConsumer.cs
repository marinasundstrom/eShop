using MassTransit;
using MediatR;
using YourBrand.Payments.Contracts;

namespace YourBrand.Payments.Consumers;

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

        //await mediator.Send(new Application.Receipts.Commands.UpdateStatus(message, null!));
    }
}