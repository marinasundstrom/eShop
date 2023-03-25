using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record CreateProductGroup(ApiCreateProductGroup Data) : IRequest<ProductGroupDto>
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
                .Include(x => x.Parent)
                .FirstOrDefaultAsync(x => x.Id == request.Data.ParentGroupId);

            var itemGroup = new ProductGroup(request.Data.Name)
            {
                Name = request.Data.Name,
                Handle = request.Data.Handle,
                Description = request.Data.Description,
                Parent = parentGroup,
                Path = parentGroup is null ? request.Data.Handle : $"{parentGroup.Path}/{request.Data.Handle}",
                AllowItems = request.Data.AllowItems
            };

            _context.ProductGroups.Add(itemGroup);

            await _context.SaveChangesAsync();

            return itemGroup.ToDto();

        }
    }
}