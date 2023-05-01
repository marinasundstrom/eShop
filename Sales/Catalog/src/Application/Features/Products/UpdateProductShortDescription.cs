using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductShortDescription(long ProductId, string ShortDescription) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductShortDescription, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductShortDescription request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            item.ShortDescription = request.ShortDescription;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
