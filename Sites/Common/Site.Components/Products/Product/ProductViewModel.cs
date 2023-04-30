using System.Text.Json;

using YourBrand.StoreFront;

namespace Site.Client.Products.Product;

public class ProductViewModel
{
    private IProductsClient productsClient;
    private SiteProductDto? product;
    private SiteProductDto? variant;
    private IEnumerable<ProductOptionDto>? productOptions;
    private IEnumerable<ProductAttributeDto>? productAttributes;

    public ProductViewModel(IProductsClient productsClient)
    {
        this.productsClient = productsClient;
    }

    public ProductViewModel()
    {
    }

    public void SetClient(IProductsClient productsClient) => this.productsClient = productsClient;

    public string Id => Product?.Handle ?? string.Empty;

    public string VariantId => Variant?.Handle ?? string.Empty;

    public string Name => Product?.Name ?? string.Empty;

    public string Description => Variant?.Description ?? Product?.Description ?? string.Empty;

    public string Image => Variant?.Image ?? Product?.Image ?? string.Empty;

    public string Currency => Product!.RegularPrice!.Currency;

    public decimal Price => Variant?.Price?.Amount ?? Product?.Price?.Amount ?? 0;

    public decimal Total => Price
                + OptionGroups.SelectMany(x => x.Options)
                .Where(x => x.IsSelected || x.SelectedValueId is not null)
                .Select(x => x.Price.GetValueOrDefault() + (x.Values.FirstOrDefault(x3 => x3.Id == x?.SelectedValueId)?.Price ?? 0))
                .Sum();

    public decimal? RegularPrice => Product?.RegularPrice!.Amount;

    public int? Available => Variant?.Available ?? Product?.Available;

    public async Task Initialize(string id, string? variantId)
    {
        Product = await productsClient.GetProductAsync(id);

        await Load();

        if (Product.HasVariants)
        {
            SiteProductDto? product;

            if (variantId is not null)
            {
                product = await productsClient.GetProductAsync(variantId);
            }
            else
            {
                var variants = (await productsClient.GetProductVariantsAsync(Id, 1, 20, null, null, null)).Items;
                product = await productsClient.GetProductAsync(variants.First().Id.ToString());
            }

            AttributeGroups.ForEach(x => x.Attributes.ForEach(x => x.SelectedValueId = null));

            var attrs = AttributeGroups.SelectMany(x => x.Attributes);

            foreach (var attr in product.Attributes.Where(x => x.ForVariant))
            {
                var x = attrs.FirstOrDefault(x => x.Id == attr.Attribute.Id);
                x.SelectedValueId = attr.Value.Id;
            }

            var selectedAttributeValues = AttributeGroups
                .SelectMany(x => x.Attributes)
                .Where(x => x.ForVariant)
                .Where(x => !x.IsMainAttribute)
                .Where(x => x.SelectedValueId is not null)
                .ToDictionary(x => x.Id, x => x.SelectedValueId);

            Variants.AddRange((await productsClient.FindProductVariantByAttributes2Async(Id, selectedAttributeValues)));

            SelectVariant(product);
        }

        Updated?.Invoke(this, EventArgs.Empty);
    }

    private async Task Load()
    {
        productOptions = Variant?.Options ?? Product!.Options;
        productAttributes = Variant?.Attributes ?? Product!.Attributes;

        CreateOptionsVM();
        CreateAttributesVM();
    }

    public List<AttributeGroupVM> AttributeGroups { get; set; } = new List<AttributeGroupVM>();

    public List<OptionGroupVM> OptionGroups { get; set; } = new List<OptionGroupVM>();

    public List<SiteProductDto> Variants { get; set; } = new List<SiteProductDto>();

    public async Task SelectVariant(SiteProductDto variant)
    {
        if (this.variant?.Id == variant?.Id) return;

        this.variant = variant;

        await Load();

        var attributes = AttributeGroups.SelectMany(x => x.Attributes);

        foreach (var attr in variant.Attributes.Where(x => x.ForVariant))
        {
            var selectedAttr = attributes.First(x => x.Id == attr.Attribute.Id);
            selectedAttr.SelectedValueId = selectedAttr.Values.FirstOrDefault(x => x.Id == attr.Value.Id)?.Id;
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

        var products = await productsClient.FindProductVariantByAttributes2Async(Id, selectedAttributeValues);

        Variants.Clear();
        Variants.AddRange(products);

        var selectedAttributes = AttributeGroups
           .SelectMany(x => x.Attributes)
           .Where(x => x.ForVariant)
           .Where(x => x.SelectedValueId is not null);

        variant = await productsClient.FindProductVariantByAttributesAsync(Id, selectedAttributes.ToDictionary(x => x.Id, x => x.SelectedValueId));

        await Load();
    }

    private void CreateOptionsVM()
    {
        var groups = productOptions
            .Select(x => x.Option)
            .Select(x => x.Group ?? new OptionGroupDto())
            .DistinctBy(x => x.Id);

        foreach (var optionGroup in groups)
        {
            var group = OptionGroups.FirstOrDefault(x => x.Id == optionGroup.Id);
            if(group is null) 
            {
                group = new OptionGroupVM()
                {
                    Id = optionGroup.Id,
                    Name = optionGroup.Name,
                    Description = optionGroup.Description,
                    Min = optionGroup.Min,
                    Max = optionGroup.Max      
                };
                
                OptionGroups.Add(group);
            }

            foreach (var option in productOptions.Select(x => x.Option).Where(x => x.Group?.Id == group.Id))
            {
                var o = group.Options.FirstOrDefault(x => x.Id == option.Id);
                if(o is null) 
                {
                    o = new OptionVM
                    {
                        Id = option.Id,
                        Name = option.Name,
                        Description = option.Description,
                        Group = option.Group,
                        OptionType = option.OptionType,
                        Price = option.Price,
                        ProductId = option.Sku,
                        IsSelected = option.IsSelected.GetValueOrDefault(),
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
            }

            foreach(var option in group.Options.ToList()) 
            {
                var o1 = productOptions.FirstOrDefault(x => x.Option.Id == option.Id);
                if(o1 is null) 
                {
                    group.Options.Remove(option);
                }
            }
        }

        foreach(var group in OptionGroups.ToList()) 
        {
            var o1 = groups.FirstOrDefault(x => x.Id == group.Id);
            if(o1 is null) 
            {
                OptionGroups.Remove(group);
            }
        }
    }

    private void CreateAttributesVM()
    {
        var groups = productAttributes
            .Select(x => x.Attribute.Group ?? new AttributeGroupDto())
            .DistinctBy(x => x.Id);
            
        foreach (var attributeGroup in groups)
        {
            var group = AttributeGroups.FirstOrDefault(x => x.Id == attributeGroup.Id);
            if(group is null) 
            {
                group = new AttributeGroupVM()
                {
                    Id = attributeGroup.Id,
                    Name = attributeGroup.Name
                };
            
                AttributeGroups.Add(group);
            }

            foreach (var attribute in productAttributes.Where(x => x.Attribute.Group?.Id == group.Id))
            {
                var attr = group.Attributes.FirstOrDefault(x => x.Id == attribute.Attribute.Id);
                if(attr is null) 
                {
                    attr = new AttributeVM
                    {
                        Id = attribute.Attribute.Id,
                        Name = attribute.Attribute.Name,
                        ForVariant = attribute.ForVariant,
                        IsMainAttribute = attribute.IsMainAttribute
                    };

                    attr.Values.AddRange(attribute.Attribute.Values.Select(x => new AttributeValueVM
                    {
                        Id = x.Id,
                        Name = x.Name
                    }));

                    attr.SelectedValueId = attr.Values.FirstOrDefault()?.Id;
                    
                    group.Attributes.Add(attr);
                }
            }

            foreach(var attr in group.Attributes.ToList()) 
            {
                var a = productAttributes.FirstOrDefault(x => x.Attribute.Id == attr.Id);
                if(a is null) 
                {
                    group.Attributes.Remove(attr);
                }
            }
        }

        foreach(var group in AttributeGroups.ToList()) 
        {
            var o1 = groups.FirstOrDefault(x => x.Id == group.Id);
            if(o1 is null) 
            {
                AttributeGroups.Remove(group);
            }
        }
    }

    public event EventHandler Updated;

    public SiteProductDto? Product { get => product; set => product = value; }

    public SiteProductDto? Variant { get => variant; set => variant = value; }

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
                ProductId = x.ProductId,
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

        public string? ProductId { get; set; }

        public decimal? Price { get; set; }

        public string? TextValue { get; set; }

        public int? NumericalValue { get; set; }

        public bool? IsSelected { get; set; }

        public string? SelectedValueId { get; set; }
    }
}