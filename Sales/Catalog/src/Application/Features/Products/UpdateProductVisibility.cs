using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public record UpdateProductVisibility(long ProductId, ProductVisibility Visibility) : IRequest
{
    public class Handler : IRequestHandler<UpdateProductVisibility>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductVisibility request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            item.Visibility = request.Visibility == ProductVisibility.Listed ? Domain.Enums.ProductVisibility.Listed : Domain.Enums.ProductVisibility.Unlisted;

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}