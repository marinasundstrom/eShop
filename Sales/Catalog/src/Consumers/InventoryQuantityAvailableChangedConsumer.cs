using MassTransit;
using MediatR;
using YourBrand.Inventory.Contracts;

namespace YourBrand.Catalog.Consumers;

public sealed class QuantityAvailableChangedConsumer : IConsumer<QuantityAvailableChanged>
{
    private readonly IMediator mediator;

    public QuantityAvailableChangedConsumer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task Consume(ConsumeContext<QuantityAvailableChanged> context)
    {
        var message = context.Message;

        await mediator.Send(new Features.Products.UpdateQuantityAvailable(message.Id, context.Message.Quantity));
    }
}