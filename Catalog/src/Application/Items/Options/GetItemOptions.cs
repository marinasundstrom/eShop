using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Domain;

namespace YourBrand.Catalog.Application.Items.Options;

public record GetItemOptions(string ItemId, string? VariantId) : IRequest<IEnumerable<OptionDto>>
{
    public class Handler : IRequestHandler<GetItemOptions, IEnumerable<OptionDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OptionDto>> Handle(GetItemOptions request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ItemOptions)
                .ThenInclude(pv => pv.Option)
                .ThenInclude(pv => pv.Group)
                .Include(pv => pv.ItemOptions)
                .ThenInclude(pv => pv.Option)
                .ThenInclude(pv => pv.Values)
                .Include(pv => pv.ItemOptions)
                .ThenInclude(pv => pv.Option)
                .ThenInclude(o => o.DefaultValue)
                .FirstAsync(p => p.Id == request.ItemId);

            var options = item.ItemOptions
                .Select(x => x.Option)
                .ToList();

            if (request.VariantId is not null)
            {
                item = await _context.Items
                    .AsSplitQuery()
                    .AsNoTracking()
                    .Include(pv => pv.ItemOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => pv.Group)
                    .Include(pv => pv.ItemOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(pv => pv.Values)
                    .Include(pv => pv.ItemOptions)
                    .ThenInclude(pv => pv.Option)
                    .ThenInclude(o => o.DefaultValue)
                    .FirstAsync(p => p.Id == request.ItemId);

                options.AddRange(item.ItemOptions.Select(x => x.Option));
            }

            return options.Select(x => new OptionDto(x.Id, x.Name, x.Description, (Application.OptionType)x.OptionType, x.Group == null ? null : new OptionGroupDto(x.Group.Id, x.Group.Name, x.Group.Description, x.Group.Seq, x.Group.Min, x.Group.Max), x.InventoryItemId, x.Price, x.IsSelected,
                x.Values.Select(x => new OptionValueDto(x.Id, x.Name, x.InventoryItemId, x.Price, x.Seq)),
                x.DefaultValue == null ? null : new OptionValueDto(x.DefaultValue.Id, x.DefaultValue.Name, x.DefaultValue.InventoryItemId, x.DefaultValue.Price, x.DefaultValue.Seq), x.MinNumericalValue, x.MaxNumericalValue, x.DefaultNumericalValue, x.TextValueMinLength, x.TextValueMaxLength, x.DefaultTextValue));
        }
    }
}
