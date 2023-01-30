using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Attributes.Groups;

public record DeleteAttributeGroup(string Id) : IRequest
{
    public class Handler : IRequestHandler<DeleteAttributeGroup>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteAttributeGroup request, CancellationToken cancellationToken)
        {
            var attributeGroup = await _context.AttributeGroups
                .FirstAsync(x => x.Id == request.Id, cancellationToken);

            attributeGroup.Attributes.Clear();

            _context.AttributeGroups.Remove(attributeGroup);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}