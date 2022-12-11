using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Products.Attributes.Groups;

public record DeleteProductAttributeGroup(string ProductId, string AttributeGroupId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductAttributeGroup>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductAttributeGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .Include(x => x.AttributeGroups)
                .ThenInclude(x => x.Attributes)
                .FirstAsync(x => x.Id == request.ProductId);

            var attributeGroup = item.AttributeGroups
                .First(x => x.Id == request.AttributeGroupId);

            attributeGroup.Attributes.Clear();

            item.AttributeGroups.Remove(attributeGroup);
            _context.AttributeGroups.Remove(attributeGroup);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
