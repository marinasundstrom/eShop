using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Options.Groups;

public record CreateItemOptionGroup(string ItemId, ApiCreateItemOptionGroup Data) : IRequest<OptionGroupDto>
{
    public class Handler : IRequestHandler<CreateItemOptionGroup, OptionGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionGroupDto> Handle(CreateItemOptionGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .FirstAsync(x => x.Id == request.ItemId);

            var group = new OptionGroup(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                Min = request.Data.Min,
                Max = request.Data.Max
            };

            item.OptionGroups.Add(group);

            await _context.SaveChangesAsync();

            return new OptionGroupDto(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max);
        }
    }
}
