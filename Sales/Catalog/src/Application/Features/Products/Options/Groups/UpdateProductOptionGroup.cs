using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Features.Options;

namespace YourBrand.Catalog.Features.Products.Options.Groups;

public record UpdateProductOptionGroup(long ProductId, string OptionGroupId, ApiUpdateProductOptionGroup Data) : IRequest<OptionGroupDto>
{
    public class Handler : IRequestHandler<UpdateProductOptionGroup, OptionGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionGroupDto> Handle(UpdateProductOptionGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
            .Include(x => x.OptionGroups)
            .FirstAsync(x => x.Id == request.ProductId);

            var optionGroup = item.OptionGroups
                .First(x => x.Id == request.OptionGroupId);

            optionGroup.Name = request.Data.Name;
            optionGroup.Description = request.Data.Description;
            optionGroup.Min = request.Data.Min;
            optionGroup.Max = request.Data.Max;

            await _context.SaveChangesAsync();

            return new OptionGroupDto(optionGroup.Id, optionGroup.Name, optionGroup.Description, optionGroup.Seq, optionGroup.Min, optionGroup.Max);
        }
    }
}