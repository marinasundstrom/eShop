using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Marketing.Application.Common.Interfaces;
using YourBrand.Marketing.Domain;

namespace YourBrand.Marketing.Application.Features.Discounts.Commands;

public record DeleteDiscountCommand(string Id) : IRequest
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand>
    {
        private readonly IApplicationDbContext context;

        public DeleteDiscountCommandHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var discount = await context.Discounts
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (discount is null) throw new Exception();

            context.Discounts.Remove(discount);

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}