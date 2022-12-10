using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Products.Groups;

public record UpdateProductGroup(string ProductGroupId, ApiUpdateProductGroup Data) : IRequest<ProductGroupDto>
{
    public class Handler : IRequestHandler<UpdateProductGroup, ProductGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto> Handle(UpdateProductGroup request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ProductGroups
                    .FirstAsync(x => x.Id == request.ProductGroupId);

            var parentGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.ParentGroupId);

            itemGroup.Name = request.Data.Name;
            itemGroup.Description = request.Data.Description;
            itemGroup.Parent = parentGroup;

            await _context.SaveChangesAsync();

            return itemGroup.ToDto();
        }
    }
}
