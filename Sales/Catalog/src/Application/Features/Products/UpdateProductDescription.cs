using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductDescription(long ProductId, string Description) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductDescription, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductDescription request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            item.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
