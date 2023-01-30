using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Options;

public record DeleteProductOptionValue(string ProductId, string OptionId, string ValueId) : IRequest
{
    public class Handler : IRequestHandler<DeleteProductOptionValue>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteProductOptionValue request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
             .AsSplitQuery()
             .Include(pv => pv.Options)
             .ThenInclude(pv => pv.Values)
             .FirstAsync(p => p.Id == request.ProductId);

            var option = item.Options.First(o => o.Id == request.OptionId);

            var value = option.Values.First(o => o.Id == request.ValueId);

            option.Values.Remove(value);
            _context.OptionValues.Remove(value);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}