using System.Text.Json;

namespace Site.Client.Products.Product;

public class ProductViewModel
{
    private IProductsClient productsClient;
    private SiteProductDto? product;
    private SiteProductDto? variant;
    private IEnumerable<OptionDto>? productOptions;
    private IEnumerable<ProductAttributeDto>? productAttributes;

    public ProductViewModel(IProductsClient productsClient)
    {
        this.productsClient = productsClient;
    }

    public ProductViewModel()
    {
    }

    public void SetClient(IProductsClient productsClient) => this.productsClient = productsClient;

    public string Id => Product?.Id ?? string.Empty;

    public string VariantId => Variant?.Id ?? string.Empty;

    public string Name => Product?.Name ?? string.Empty;

    public string Description => Variant?.Description ?? Product?.Description ?? string.Empty;

    public string Image => Variant?.Image ?? Product?.Image ?? string.Empty;

    public decimal Price => Variant?.Price ?? Product?.Price ?? 0;

    public decimal Total => Price
                + OptionGroups.SelectMany(x => x.Options)
                .Where(x => x.IsSelected || x.SelectedValueId is not null)
                .Select(x => x.Price.GetValueOrDefault() + (x.Values.FirstOrDefault(x3 => x3.Id == x?.SelectedValueId)?.Price ?? 0))
                .Sum();

    public decimal? CompareAtPrice => Product?.CompareAtPrice;

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
                product = await productsClient.GetProductAsync(variants.First().Id);
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
        productOptions = Product!.Options;
        productAttributes = Product.Attributes;

        CreateOptionsVM();
        CreateAttributesVM();
    }

    public List<AttributeGroupVM> AttributeGroups { get; set; } = new List<AttributeGroupVM>();

    public List<OptionGroupVM> OptionGroups { get; set; } = new List<OptionGroupVM>();

    public List<SiteProductDto> Variants { get; set; } = new List<SiteProductDto>();

    public void SelectVariant(SiteProductDto variant)
    {
        if (this.variant?.Id == variant?.Id) return;

        this.variant = variant;

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
    }

    private void CreateOptionsVM()
    {
        foreach (var optionGroup in productOptions
            .Select(x => x.Group ?? new OptionGroupDto())
            .DistinctBy(x => x.Id))
        {
            var group = new OptionGroupVM()
            {
                Id = optionGroup.Id,
                Name = optionGroup.Name,
                Description = optionGroup.Description,
                Min = optionGroup.Min,
                Max = optionGroup.Max
            };

            foreach (var option in productOptions.Where(x => x.Group?.Id == group.Id))
            {
                var o = new OptionVM
                {
                    Id = option.Id,
                    Name = option.Name,
                    Description = option.Description,
                    Group = option.Group,
                    OptionType = option.OptionType,
                    Price = option.Price,
                    ProductId = option.ProductId,
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
        foreach (var attributeGroup in productAttributes
            .Select(x => x.Attribute.Group ?? new AttributeGroupDto())
            .DistinctBy(x => x.Id))
        {
            var group = new AttributeGroupVM()
            {
                Id = attributeGroup.Id,
                Name = attributeGroup.Name
            };

            foreach (var attribute in productAttributes.Where(x => x.Attribute.Group?.Id == group.Id))
            {
                var attr = new AttributeVM
                {
                    Id = attribute.Attribute.Id,
                    Name = attribute.Attribute.Name,
                    ForVariant = attribute.ForVariant,
                    IsMainAttribute = attribute.IsMainAttribute
                };

                group.Attributes.Add(attr);

                attr.Values.AddRange(attribute.Attribute.Values.Select(x => new AttributeValueVM
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