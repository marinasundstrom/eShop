using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductDetails(long ProductId, ApiUpdateProductDetails Details) : IRequest<Result>
{
    public sealed class Handler : IRequestHandler<UpdateProductDetails, Result>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(UpdateProductDetails request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstOrDefaultAsync(x => x.Id == request.ProductId);

            if (item is null) 
            {
                return Result.Failure(Errors.Products.ProductNotFound);
            }

            var group = await _context.ProductGroups
                .FirstOrDefaultAsync(x => x.Id == request.Details.GroupId);

            item.Name = request.Details.Name;
            item.Description = request.Details.Description;
            item.Group = group;
            item.Price = request.Details.Price;
            item.RegularPrice = request.Details.RegularPrice;

            await _context.SaveChangesAsync();

            return Result.Success();
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}