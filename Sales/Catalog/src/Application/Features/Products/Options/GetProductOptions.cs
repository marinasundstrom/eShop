using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Features.Options;

namespace YourBrand.Catalog.Features.Products.Options;

public record GetProductOptions(string ProductId, string? VariantId) : IRequest<IEnumerable<OptionDto>>
{
    public class Handler : IRequestHandler<GetProductOptions, IEnumerable<OptionDto>>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OptionDto>> Handle(GetProductOptions request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .AsSplitQuery()
                .AsNoTracking()
                .Include(pv => pv.ProductOptions)
                .ThenInclude(pv => pv.Option.Group)
                .Include(pv => pv.ProductOptions)
                .ThenInclude(pv => (pv.Option as ChoiceOption)!.Values)
                .Include(pv => pv.ProductOptions)
                .ThenInclude(pv => (pv.Option as ChoiceOption)!.DefaultValue)
                .FirstAsync(p => p.Id == request.ProductId);

            var options = item.ProductOptions
                .Select(x => x.Option)
                .ToList();

            if (request.VariantId is not null)
            {
                item = await _context.Products
                    .AsSplitQuery()
                    .AsNoTracking()
                    .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => pv.Option.Group)
                    .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => (pv.Option as ChoiceOption)!.Values)
                    .Include(pv => pv.ProductOptions)
                    .ThenInclude(pv => (pv.Option as ChoiceOption)!.DefaultValue)
                    .FirstAsync(p => p.Id == request.ProductId);

                options.AddRange(item.ProductOptions.Select(x => x.Option));
            }

            return options.Select(x => x.ToDto());
        }
    }
}