using System.Text.Json;

namespace Site.Client.Items.Item;

public class ProductViewModel 
{
    private IItemsClient itemsClient;
    private SiteItemDto? item;
    private SiteItemDto? variant;
    private IEnumerable<OptionDto>? itemOptions;
    private IEnumerable<AttributeDto>? itemAttributes;

    public ProductViewModel(IItemsClient ItemsClient)
    {
        itemsClient = ItemsClient;
    }

    public ProductViewModel()
    {
    }

    public void SetClient(IItemsClient itemsClient) => this.itemsClient = itemsClient;

    public string Id => Item?.Id ?? string.Empty;

    public string VariantId => Variant?.Id ?? string.Empty;

    public string Name => Item?.Name ?? string.Empty;

    public string Description => Variant?.Description ?? Item?.Description ?? string.Empty;

    public string Image => Variant?.Image ?? Item?.Image ?? string.Empty;

    public decimal Price => Variant?.Price ?? Item?.Price ?? 0;

    public decimal Total => Price
                + OptionGroups.SelectMany(x => x.Options)
                .Where(x => x.IsSelected || x.SelectedValueId is not null)
                .Select(x => x.Price.GetValueOrDefault() + (x.Values.FirstOrDefault(x3 => x3.Id == x?.SelectedValueId)?.Price ?? 0))
                .Sum();

    public decimal? CompareAtPrice => Item?.CompareAtPrice;

    public int? Available => Variant?.Available ?? Item?.Available;

    public async Task Initialize(string id, string? variantId) 
    {
        Item = await itemsClient.GetItemAsync(id);

        await Load();

        if (Item.HasVariants)
        {
            SiteItemDto? item;

            if(variantId is not null) 
            {
                item = await itemsClient.GetItemAsync(variantId);
            }
            else 
            {
                var variants = (await itemsClient.GetItemVariantsAsync(Id, 1, 20, null, null, null)).Items;
                item = await itemsClient.GetItemAsync(variants.First().Id);
            }

            AttributeGroups.ForEach(x => x.Attributes.ForEach(x => x.SelectedValueId = null));

            var attrs = AttributeGroups.SelectMany(x => x.Attributes);

            foreach(var attr in item.VariantAttributes) 
            {
                var x = attrs.FirstOrDefault(x => x.Id == attr.Id);
                x.SelectedValueId = attr.ValueId;
            }

            var selectedAttributeValues = AttributeGroups
                .SelectMany(x => x.Attributes)
                .Where(x => x.ForVariant)
                .Where(x => !x.IsMainAttribute)
                .Where(x => x.SelectedValueId is not null)
                .ToDictionary(x => x.Id, x => x.SelectedValueId);

            Variants.AddRange((await itemsClient.FindItemVariantByAttributes2Async(Id, selectedAttributeValues)));

            SelectVariant(item);
        }

        Updated?.Invoke(this, EventArgs.Empty);
    }

    private async Task Load() 
    {
        itemOptions = Item!.Options;
        itemAttributes = Item.Attributes;

        CreateOptionsVM();
        CreateAttributesVM();
    }

    public List<AttributeGroupVM> AttributeGroups { get; set; } = new List<AttributeGroupVM>();

    public List<OptionGroupVM> OptionGroups { get; set; } = new List<OptionGroupVM>();

    public List<SiteItemDto> Variants { get; set; } = new List<SiteItemDto>();

    public void SelectVariant(SiteItemDto variant)
    {
        if (this.variant?.Id == variant?.Id) return;

        this.variant = variant;

        var attributes = AttributeGroups.SelectMany(x => x.Attributes);

        foreach (var attr in variant.VariantAttributes)
        {
            var selectedAttr = attributes.First(x => x.Id == attr.Id);
            selectedAttr.SelectedValueId = selectedAttr.Values.FirstOrDefault(x => x.Id == attr.ValueId)?.Id;
        }

        Updated?.Invoke(this, EventArgs.Empty);
    }

    public async Task UpdateVariant()
    {
        var selectedAttributeValues = AttributeGroups
            .SelectMany(x => x.Attributes)
            .Where(x => x.ForVariant)
            .Where(x => !x.IsMainAttribute)
            .Where(x => x.SelectedValueId is not null)
            .ToDictionary(x => x.Id, x => x.SelectedValueId);

        var items = await itemsClient.FindItemVariantByAttributes2Async(Id, selectedAttributeValues);
        
        Variants.Clear();
        Variants.AddRange(items);

        var selectedAttributes = AttributeGroups
           .SelectMany(x => x.Attributes)
           .Where(x => x.ForVariant)
           .Where(x => x.SelectedValueId is not null);

        variant = await itemsClient.FindItemVariantByAttributesAsync(Id, selectedAttributes.ToDictionary(x => x.Id, x => x.SelectedValueId));
    }

    private void CreateOptionsVM() 
    {
        foreach(var optionGroup in itemOptions
            .Select(x => x.Group ?? new OptionGroupDto())
            .DistinctBy(x => x.Id)) 
        {
            var group = new OptionGroupVM() {
                Id = optionGroup.Id,
                Name = optionGroup.Name,
                Description = optionGroup.Description,
                Min = optionGroup.Min,
                Max = optionGroup.Max
            };

            foreach (var option in itemOptions.Where(x => x.Group?.Id == group.Id))
            {
                var o = new OptionVM
                {
                    Id = option.Id,
                    Name = option.Name,
                    Description = option.Description,
                    Group = option.Group,
                    OptionType = option.OptionType,
                    Price = option.Price,
                    ItemId = option.ItemId,
                    IsSelected = option.IsSelected,
                    SelectedValueId = option.DefaultValue?.Id,
                    MinNumericalValue = option.MinNumericalValue,
                    MaxNumericalValue = option.MaxNumericalValue,
                    NumericalValue = option.DefaultNumericalValue,
                    TextValue = option.DefaultTextValue,
                    TextValueMaxLength = option.TextValueMaxLength,
                    TextValueMinLength = option.TextValueMinLength
                };

                o.Values.AddRange(option.Values.Select(x => new OptionValueVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                }));

                group.Options.Add(o);
            }

            OptionGroups.Add(group);
        }
    }

    private void CreateAttributesVM()
    {
        foreach (var attributeGroup in itemAttributes
            .Select(x => x.Group ?? new AttributeGroupDto())
            .DistinctBy(x => x.Id))
        {
            var group = new AttributeGroupVM()
            {
                Id = attributeGroup.Id,
                Name = attributeGroup.Name
            };

            foreach (var attribute in itemAttributes.Where(x => x.Group?.Id == group.Id))
            {
                var attr = new AttributeVM
                {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    ForVariant = attribute.ForVariant,
                    IsMainAttribute = attribute.IsMainAttribute
                };

                group.Attributes.Add(attr);

                attr.Values.AddRange(attribute.Values.Select(x => new AttributeValueVM
                {
                    Id = x.Id,
                    Name = x.Name
                }));

                attr.SelectedValueId = attr.Values.FirstOrDefault()?.Id;

            }

            AttributeGroups.Add(group);
        }
    }
    
    public event EventHandler Updated;

    public SiteItemDto? Item { get => item; set => item = value; }

    public SiteItemDto? Variant { get => variant; set => variant = value; }

    public void LoadData(IEnumerable<Option> options) 
    {
        foreach (var option in options)
        {
            var o = OptionGroups
                .SelectMany(x => x.Options)
                .FirstOrDefault(x => x.Id == option.Id);

            if (o is not null)
            {
                o.IsSelected = option.IsSelected.GetValueOrDefault();
                o.SelectedValueId = option.SelectedValueId;
                o.NumericalValue = option.NumericalValue;
                o.TextValue = option.TextValue;
            }
        }
    }

    public IEnumerable<Option> GetData()
    {
        return OptionGroups.SelectMany(x => x.Options).Select(x =>
        {
            return new Option
            {
                Id = x.Id,
                Name = x.Name,
                OptionType = (int)x.OptionType,
                ItemId = x.ItemId,
                Price = x.Price,
                IsSelected = x.IsSelected,
                SelectedValueId = x.SelectedValueId,
                NumericalValue = x.NumericalValue,
                TextValue = x.TextValue
            };
        });
    }

    public class Option
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public int OptionType { get; set; }

        public string? ItemId { get; set; }

        public decimal? Price { get; set; }

        public string? TextValue { get; set; }

        public int? NumericalValue { get; set; }

        public bool? IsSelected { get; set; }

        public string? SelectedValueId { get; set; }
    }
}