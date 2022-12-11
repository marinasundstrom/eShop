using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Subscriptions.Domain;
using YourBrand.Subscriptions.Application;
using YourBrand.Subscriptions.Application.Orders.Dtos;

namespace YourBrand.Subscriptions.Application.Orders.Statuses.Commands;

public record CreateOrderStatusCommand(string Name, bool CreateWarehouse) : IRequest<OrderStatusDto>
{
    public class CreateOrderStatusCommandHandler : IRequestHandler<CreateOrderStatusCommand, OrderStatusDto>
    {
        private readonly IApplicationDbContext context;

        public CreateOrderStatusCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<OrderStatusDto> Handle(CreateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.OrderStatuses.FirstOrDefaultAsync(i => i.Name == request.Name, cancellationToken);

            if (orderStatus is not null) throw new Exception();

            orderStatus = new Domain.Entities.OrderStatus(request.Name);

            context.OrderStatuses.Add(orderStatus);

            await context.SaveChangesAsync(cancellationToken);

            return orderStatus.ToDto();
        }
    }
}
