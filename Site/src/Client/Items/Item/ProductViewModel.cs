namespace Site.Client.Items.Item;

public class ProductViewModel 
{
    private readonly IItemsClient itemsClient;
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

    public string Id => Item.Id;

    public string VariantId => Variant.Id;

    public string Name => Variant?.Name ?? Item.Name;

    public string Description => Variant?.Description ?? Item.Description;

    public decimal Price => Variant?.Price ?? Item.Price;

    public decimal? CompareAtPrice => Item.CompareAtPrice;

    public async Task Initialize(string id, string? VariantId) 
    {
        Item = await itemsClient.GetItemAsync(Id);

        await Load();

        if (Item.HasVariants)
        {

        }

        if(VariantId is not null) 
        {
            var item = await itemsClient.GetItemAsync(VariantId);

            AttributeGroups.ForEach(x => x.Attributes.ForEach(x => x.SelectedValueId = null));

            var attrs = AttributeGroups.SelectMany(x => x.Attributes);

            foreach(var attr in item.VariantAttributes) 
            {
                var x = attrs.FirstOrDefault(x => x.Id == attr.Id);
                x.SelectedValueId = attr.ValueId;
            }

            SelectVariant(item);
        }

        Updated?.Invoke(this, EventArgs.Empty);
    }

    private async Task Load() 
    {
        itemOptions = await itemsClient.GetItemOptionsAsync(Id);
        itemAttributes = await itemsClient.GetItemAttributesAsync(Id);

        CreateOptionsVM();
        CreateAttributesVM();
    }

    public List<AttributeGroupVM> AttributeGroups { get; private set; } = new List<AttributeGroupVM>();

    public List<OptionGroupVM> OptionGroups { get; private set; } = new List<OptionGroupVM>();

    public void SelectVariant(SiteItemDto variant)
    {
        if (this.variant?.Id == variant?.Id) return;

        this.variant = variant;

        var attributes = AttributeGroups.SelectMany(x => x.Attributes);

        foreach (var attr in Variant.VariantAttributes)
        {
            var selectedAttr = attributes.First(x => x.Id == attr.Id);
            selectedAttr.SelectedValueId = selectedAttr.Values.FirstOrDefault(x => x.Id == attr.ValueId)?.Id;
        }

        Updated?.Invoke(this, EventArgs.Empty);
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

    public SiteItemDto? Variant { get => Variant; set => Variant = value; }
}