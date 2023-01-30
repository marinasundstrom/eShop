using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products;

public record UpdateProductDetails(string ProductId, ApiUpdateProductDetails Details) : IRequest
{
    public class Handler : IRequestHandler<UpdateProductDetails>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductDetails request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
            .FirstAsync(x => x.Id == request.ProductId);

            var group = await _context.ProductGroups
                .FirstOrDefaultAsync(x => x.Id == request.Details.GroupId);

            item.Name = request.Details.Name;
            item.Description = request.Details.Description;
            item.Group = group;
            item.Price = request.Details.Price;
            item.CompareAtPrice = request.Details.CompareAtPrice;

            await _context.SaveChangesAsync();

            return Unit.Value;
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}