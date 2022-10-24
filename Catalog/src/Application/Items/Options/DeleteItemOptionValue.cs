using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Options;

public record DeleteItemOptionValue(string ItemId, string OptionId, string ValueId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemOptionValue>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemOptionValue request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
             .AsSplitQuery()
             .Include(pv => pv.Options)
             .ThenInclude(pv => pv.Values)
             .FirstAsync(p => p.Id == request.ItemId);

            var option = item.Options.First(o => o.Id == request.OptionId);

            var value = option.Values.First(o => o.Id == request.ValueId);

            option.Values.Remove(value);
            _context.OptionValues.Remove(value);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
