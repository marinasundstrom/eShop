using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Groups;

public record DeleteProductGroup(string ProductGroupId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductGroup>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductGroup request, CancellationToken cancellationToken)
        {
            var itemGroup = await _context.ProductGroups
                .Include(x => x.Products)
                .FirstAsync(x => x.Id == request.ProductGroupId);

            itemGroup.Products.Clear();

            _context.ProductGroups.Remove(itemGroup);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}