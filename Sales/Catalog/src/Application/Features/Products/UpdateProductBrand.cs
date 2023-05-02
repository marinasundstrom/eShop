using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductBrand(long ProductId, int BrandId) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductBrand, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductBrand request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(x => x.Id == request.BrandId);

            if (brand is null) 
            {
                return Result.Failure(Errors.Brands.BrandNotFound);
            }

            item.Brand = brand;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
