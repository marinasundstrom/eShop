using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Options;

public record DeleteItemOption(string ItemId, string OptionId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItemOption>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteItemOption request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .Include(x => x.Options)
                .FirstAsync(x => x.Id == request.ItemId);

            var option = item.Options
                .First(x => x.Id == request.OptionId);

            item.Options.Remove(option);
            _context.Options.Remove(option);

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
