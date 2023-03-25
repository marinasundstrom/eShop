using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Features.Products.Groups;

namespace YourBrand.Catalog.Features.Products;

public sealed record UpdateProductGroup(long ProductId, long GroupId) : IRequest<ProductGroupDto>
{
    public sealed class Handler : IRequestHandler<UpdateProductGroup, ProductGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto> Handle(UpdateProductGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .Include(x => x.Group)
                .ThenInclude(x => x!.Parent)
                .FirstAsync(x => x.Id == request.ProductId);

            item.Group!.DecrementProductCount();

            var group = await _context.ProductGroups
                .FirstOrDefaultAsync(x => x.Id == request.GroupId, cancellationToken);

            if(group is null) 
            {
                throw new Exception();
            }

            if(!group.AllowItems) 
            {
                throw new Exception();
            }

            item.Group = group;

            item.Group!.IncrementProductCount();

            await _context.SaveChangesAsync(cancellationToken);

            var dto = await _context.ProductGroups
                .Include(x => x.Parent)
                .FirstAsync(x => x.Id == request.GroupId, cancellationToken);

            return dto.ToDto();
        }
    }
}
