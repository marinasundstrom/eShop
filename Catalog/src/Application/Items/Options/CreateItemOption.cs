using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Domain;
using YourBrand.Catalog.Domain.Entities;

namespace YourBrand.Catalog.Application.Items.Options;

public record CreateItemOption(string ItemId, ApiCreateItemOption Data) : IRequest<OptionDto>
{
    public class Handler : IRequestHandler<CreateItemOption, OptionDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionDto> Handle(CreateItemOption request, CancellationToken cancellationToken)
        {
            var item = await _context.Items
            .FirstAsync(x => x.Id == request.ItemId);

            var group = await _context.OptionGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.GroupId);

            Option option = new(Guid.NewGuid().ToString())
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                ItemId = request.Data.ItemId,
                Group = group,
                IsSelected = request.Data.IsSelected,
                Price = request.Data.Price,
                OptionType = (Domain.Enums.OptionType)request.Data.OptionType
            };

            foreach (var v in request.Data.Values)
            {
                var value = new OptionValue(v.Name)
                {
                    ItemId = v.ItemId,
                    Price = v.Price
                };

                option.Values.Add(value);
            }

            option.DefaultValueId = option.Values.FirstOrDefault(x => x.Id == request.Data.DefaultOptionValueId)?.Id;

            item.Options.Add(option);

            await _context.SaveChangesAsync();

            return new OptionDto(option.Id, option.Name, option.Description, (Application.OptionType)option.OptionType, option.Group == null ? null : new OptionGroupDto(option.Group.Id, option.Group.Name, option.Group.Description, option.Group.Seq, option.Group.Min, option.Group.Max), option.ItemId, option.Price, option.IsSelected,
                option.Values.Select(x => new OptionValueDto(x.Id, x.Name, x.ItemId, x.Price, x.Seq)),
                option.DefaultValue == null ? null : new OptionValueDto(option.DefaultValue.Id, option.DefaultValue.Name, option.DefaultValue.ItemId, option.DefaultValue.Price, option.DefaultValue.Seq), option.MinNumericalValue, option.MaxNumericalValue, option.DefaultNumericalValue, option.TextValueMinLength, option.TextValueMaxLength, option.DefaultTextValue);
        
        }
    }
}
