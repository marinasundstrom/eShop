using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Domain;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Statuses.Commands;

public record UpdateReceiptStatusCommand(int Id, string Name) : IRequest
{
    public class UpdateReceiptStatusCommandHandler : IRequestHandler<UpdateReceiptStatusCommand>
    {
        private readonly IApplicationDbContext context;

        public UpdateReceiptStatusCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateReceiptStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.ReceiptStatuses.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (orderStatus is null) throw new Exception();

            orderStatus.Name = request.Name;

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
