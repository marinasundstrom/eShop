using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record UpdateAllowItems(long ProductGroupId, bool AllowItems) : IRequest<ProductGroupDto>
{
    public class Handler : IRequestHandler<UpdateAllowItems, ProductGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto> Handle(UpdateAllowItems request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ProductGroups
                    .FirstAsync(x => x.Id == request.ProductGroupId);

            itemGroup.AllowItems = request.AllowItems;

            await _context.SaveChangesAsync();

            return itemGroup.ToDto();
        }
    }
}