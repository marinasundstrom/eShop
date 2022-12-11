using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Pricing.Domain;
using YourBrand.Pricing.Application.Services;
using YourBrand.Pricing.Application.Orders.Dtos;

namespace YourBrand.Pricing.Application.Orders.Statuses.Commands;

public record UpdateOrderStatusCommand(int Id, string Name) : IRequest
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateOrderStatusCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.OrderStatuses.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (orderStatus is null) throw new Exception();

            orderStatus.Name = request.Name;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
