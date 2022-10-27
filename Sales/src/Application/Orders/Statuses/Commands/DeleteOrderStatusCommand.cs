using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Domain;
using YourBrand.Sales.Application.Services;
using YourBrand.Sales.Application.Orders.Dtos;

namespace YourBrand.Sales.Application.Orders.Statuses.Commands;

public record DeleteOrderStatusCommand(int Id) : IRequest
{
    public class DeleteOrderStatusCommandHandler : IRequestHandler<DeleteOrderStatusCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteOrderStatusCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.OrderStatuses
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (orderStatus is null) throw new Exception();

            context.OrderStatuses.Remove(orderStatus);
           
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}