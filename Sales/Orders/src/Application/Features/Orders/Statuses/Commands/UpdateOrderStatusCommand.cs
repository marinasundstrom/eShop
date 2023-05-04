using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Orders.Domain;
using YourBrand.Orders.Application.Services;
using YourBrand.Orders.Application.Features.Orders.Dtos;

namespace YourBrand.Orders.Application.Features.Orders.Statuses.Commands;

public record UpdateOrderStatusCommand(int Id, string Name, string Handle, string? Description) : IRequest
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
            orderStatus.Handle = request.Handle;
            orderStatus.Description = request.Description;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
