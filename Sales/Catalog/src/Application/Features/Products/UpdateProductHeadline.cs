using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductHeadline(long ProductId, string Headline) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductHeadline, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductHeadline request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            item.Headline = request.Headline;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
