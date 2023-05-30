using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Domain;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Statuses.Commands;

public record DeleteReceiptStatusCommand(int Id) : IRequest
{
    public class DeleteReceiptStatusCommandHandler : IRequestHandler<DeleteReceiptStatusCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteReceiptStatusCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteReceiptStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.ReceiptStatuses
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (orderStatus is null) throw new Exception();

            context.ReceiptStatuses.Remove(orderStatus);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}