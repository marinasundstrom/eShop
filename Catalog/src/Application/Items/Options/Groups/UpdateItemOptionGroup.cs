using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Options.Groups;

public record UpdateItemOptionGroup(string ItemId, string OptionGroupId, ApiUpdateItemOptionGroup Data) : IRequest<OptionGroupDto>
{
    public class Handler : IRequestHandler<UpdateItemOptionGroup, OptionGroupDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionGroupDto> Handle(UpdateItemOptionGroup request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .Include(x => x.OptionGroups)
            .FirstAsync(x => x.Id == request.ItemId);

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
