using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items;

public record UpdateItemVisibility(string ItemId, ItemVisibility Visibility) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemVisibility>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateItemVisibility request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .FirstAsync(x => x.Id == request.ItemId);

            item.Visibility = request.Visibility == ItemVisibility.Listed ? Domain.Enums.ItemVisibility.Listed : Domain.Enums.ItemVisibility.Unlisted;

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
