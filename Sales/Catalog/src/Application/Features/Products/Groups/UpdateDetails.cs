using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record UpdateDetails(long ProductGroupId, ApiUpdateProductGroup Data) : IRequest<ProductGroupDto>
{
    public class Handler : IRequestHandler<UpdateDetails, ProductGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto> Handle(UpdateDetails request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ProductGroups
                    .FirstAsync(x => x.Id == request.ProductGroupId);

            var parentGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.ParentGroupId);

            itemGroup.Name = request.Data.Name;
            itemGroup.Handle = request.Data.Handle;
            itemGroup.Description = request.Data.Description;

            await _context.SaveChangesAsync();

            return itemGroup.ToDto();
        }
    }
}
