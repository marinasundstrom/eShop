using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record CreateProductGroup(string ProductId, ApiCreateProductGroup Data) : IRequest<ProductGroupDto>
{
    public class Handler : IRequestHandler<CreateProductGroup, ProductGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductGroupDto> Handle(CreateProductGroup request, CancellationToken cancellationToken)
        {
            var parentGroup = await _context.ProductGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.ParentGroupId);

            var itemGroup = new ProductGroup(request.Data.Id ?? Guid.NewGuid().ToString(), request.Data.Name)
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                Parent = parentGroup
            };

            _context.ProductGroups.Add(itemGroup);

            await _context.SaveChangesAsync();

            return itemGroup.ToDto();

        }
    }
}