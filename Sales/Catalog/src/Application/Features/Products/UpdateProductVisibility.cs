using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductVisibility(long ProductId, ProductVisibility Visibility) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductVisibility, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductVisibility request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            item.Visibility = request.Visibility == ProductVisibility.Listed ? Domain.Enums.ProductVisibility.Listed : Domain.Enums.ProductVisibility.Unlisted;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
