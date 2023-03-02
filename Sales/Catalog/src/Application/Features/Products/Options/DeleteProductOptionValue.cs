using MediatR;

using Microsoft.EntityFrameworkCore;

namespace YourBrand.Catalog.Features.Products.Options;

public record DeleteProductOptionValue(long ProductId, string OptionId, string ValueId) : IRequest
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
             .ThenInclude(pv => (pv as ChoiceOption)!.Values)
             .FirstAsync(p => p.Id == request.ProductId);

            var option = item.Options.First(o => o.Id == request.OptionId);

            var value = (option as ChoiceOption)!.Values.First(o => o.Id == request.ValueId);

            (option as ChoiceOption)!.Values.Remove(value);
            _context.OptionValues.Remove(value);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}