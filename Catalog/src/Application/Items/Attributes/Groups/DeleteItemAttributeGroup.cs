using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Attributes.Groups;

public record DeleteItemAttributeGroup(string ItemId, string AttributeGroupId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemAttributeGroup>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemAttributeGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .Include(x => x.AttributeGroups)
                .ThenInclude(x => x.Attributes)
                .FirstAsync(x => x.Id == request.ItemId);

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
