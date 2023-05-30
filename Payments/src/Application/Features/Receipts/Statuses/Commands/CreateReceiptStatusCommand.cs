using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Domain;
using YourBrand.Payments.Application;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Statuses.Commands;

public record CreateReceiptStatusCommand(string Name, bool CreateWarehouse) : IRequest<ReceiptStatusDto>
{
    public class CreateReceiptStatusCommandHandler : IRequestHandler<CreateReceiptStatusCommand, ReceiptStatusDto>
    {
        private readonly IApplicationDbContext context;

        public CreateReceiptStatusCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ReceiptStatusDto> Handle(CreateReceiptStatusCommand request, CancellationToken cancellationToken)
        {
            var orderStatus = await context.ReceiptStatuses.FirstOrDefaultAsync(i => i.Name == request.Name, cancellationToken);

            if (orderStatus is not null) throw new Exception();

            orderStatus = new Domain.Entities.ReceiptStatus(request.Name);

            context.ReceiptStatuses.Add(orderStatus);

            await context.SaveChangesAsync(cancellationToken);

            return orderStatus.ToDto();
        }
    }
}
