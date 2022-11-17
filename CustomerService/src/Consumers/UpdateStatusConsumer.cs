using MassTransit;
using MediatR;
using YourBrand.CustomerService.Contracts;

namespace YourBrand.CustomerService.Consumers;

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

        //await mediator.Send(new Application.Orders.Commands.UpdateStatus(message, null!));
    }
}