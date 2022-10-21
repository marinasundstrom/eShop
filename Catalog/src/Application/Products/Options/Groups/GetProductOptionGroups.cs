using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Options;
using Catalog.Domain;

namespace Catalog.Application.Products.Options.Groups;

public record GetProductOptionGroups(string ProductId) : IRequest<IEnumerable<OptionGroupDto>>
{
    public class Handler : IRequestHandler<GetProductOptionGroups, IEnumerable<OptionGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OptionGroupDto>> Handle(GetProductOptionGroups request, CancellationToken cancellationToken)
        {
            var groups = await _context.OptionGroups
            .AsTracking()
            .Include(x => x.Product)
            .Where(x => x.Product!.Id == request.ProductId)
            .ToListAsync();

            return groups.Select(group => new OptionGroupDto(group.Id, group.Name, group.Description, group.Seq, group.Min, group.Max));
        }
    }
}
