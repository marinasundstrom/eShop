using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Catalog.Features.Options;

namespace YourBrand.Catalog.Features.Products.Options;

public record CreateProductOption(string ProductId, ApiCreateProductOption Data) : IRequest<OptionDto>
{
    public class Handler : IRequestHandler<CreateProductOption, OptionDto>
    {
        private readonly IApplicationDbContext _context;

        public Handler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OptionDto> Handle(CreateProductOption request, CancellationToken cancellationToken)
        {
            var item = await _context.Products
                .FirstAsync(x => x.Id == request.ProductId);

            var group = await _context.OptionGroups
                .FirstOrDefaultAsync(x => x.Id == request.Data.GroupId);

            Option option = default!;

            if (request.Data.OptionType == OptionType.YesOrNo) 
            {
                var selectableOption = new SelectableOption(request.Data.Name);

                selectableOption.IsSelected = request.Data.IsSelected.GetValueOrDefault();
                selectableOption.SKU = request.Data.SKU;
                selectableOption.Price = request.Data.Price;

                option = selectableOption;
            }
            else if(request.Data.OptionType == OptionType.NumericalValue) 
            {
                var numericalValue = new NumericalValueOption(request.Data.Name);

                numericalValue.MinNumericalValue = request.Data.MinNumericalValue;
                numericalValue.MaxNumericalValue = request.Data.MaxNumericalValue;
                numericalValue.DefaultNumericalValue = request.Data.DefaultNumericalValue;

                option = numericalValue;
            }
            else if(request.Data.OptionType == OptionType.TextValue) 
            {
                var textValueOption = new TextValueOption(request.Data.Name);

                textValueOption.TextValueMinLength = request.Data.TextValueMinLength;
                textValueOption.TextValueMaxLength = request.Data.TextValueMaxLength;
                textValueOption.DefaultTextValue = request.Data.DefaultTextValue;

                option = textValueOption;
            }
            else if(request.Data.OptionType == OptionType.Choice) 
            {
                var choiceOption = new ChoiceOption(request.Data.Name);

                foreach (var v in request.Data.Values)
                {
                    var value = new OptionValue(v.Name)
                    {
                        SKU = v.SKU,
                        Price = v.Price
                    };

                    choiceOption!.Values.Add(value);
                }

                choiceOption!.DefaultValueId = choiceOption!.Values.FirstOrDefault(x => x.Id == request.Data.DefaultOptionValueId)?.Id;

                option = choiceOption;
            }

            option.Description = request.Data.Description;
            option.Group = group;

            item.Options.Add(option);

            if(item.HasVariants) 
            {
                var variants = await _context.Products
                    .Where(x => x.ParentProductId == item.Id)
                    .Include(x => x.Options)
                    .ToArrayAsync(cancellationToken);
                
                foreach(var variant in item.Variants) 
                {
                    variant.ProductOptions.Add(new ProductOption() {
                        Option = option,
                        IsInherited = true
                    });
                }
            }

            await _context.SaveChangesAsync();

            return option.ToDto();
        }
    }
}