using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items;

public record UpdateItemDetails(string ItemId, ApiUpdateItemDetails Details) : IRequest
{
    public class Handler : IRequestHandler<UpdateItemDetails>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateItemDetails request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .FirstAsync(x => x.Id == request.ItemId);

            var group = await _context.ItemGroups
                .FirstOrDefaultAsync(x => x.Id == request.Details.GroupId);

            item.Name = request.Details.Name;
            item.Description = request.Details.Description;
            item.Group = group;
            item.Price = request.Details.Price;

            await _context.SaveChangesAsync();

            return Unit.Value;
        }

        private static string? GetImageUrl(string? name)
        {
            return name is null ? null : $"http://127.0.0.1:10000/devstoreaccount1/images/{name}";
        }
    }
}
