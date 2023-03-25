using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record UpdateParent(long ProductGroupId, long? ParentGroupId) : IRequest<ProductGroupDto>
{
    public class Handler : IRequestHandler<UpdateParent, ProductGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto> Handle(UpdateParent request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ProductGroups
                    .FirstAsync(x => x.Id == request.ProductGroupId);

            if(request.ParentGroupId is null) 
            {
                  itemGroup.Parent = null;
            }
            else 
            {
                var parentGroup = await _context.ProductGroups
                    .FirstOrDefaultAsync(x => x.Id == request.ParentGroupId);

                itemGroup.Parent = parentGroup;
            }              
            
            await _context.SaveChangesAsync();

            return itemGroup.ToDto();
        }
    }
}
