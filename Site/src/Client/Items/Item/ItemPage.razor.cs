using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.Json;
using Site.Client.Items.Item;

namespace Site.Client.Items.Item;

partial class ItemPage
{
    SiteItemDto? item;
    SiteItemDto? variant;
    IEnumerable<SiteItemDto>? variants;
    IEnumerable<OptionDto>? itemOptions;
    IEnumerable<AttributeDto>? itemAttributes;
    ItemsResultOfItemDto? itemVariantResults;
    List<OptionGroupVM> optionGroups = new List<OptionGroupVM>();
    List<AttributeGroupVM> attributeGroups = new List<AttributeGroupVM>();

    int quantity = 1;
    bool hasAddedToCart = false;

    private PersistingComponentStateSubscription persistingSubscription;

    [Parameter]
    public string GroupId { get; set; }

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string? VariantId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "d")]
    public string? Data { get; set; }

    string? d = null;

    protected override async Task OnInitializedAsync()
    {
        persistingSubscription =
            ApplicationState.RegisterOnPersisting(PersistItems);

        if (!ApplicationState.TryTakeFromJson<SiteItemDto>(
            "item", out var restored))
        {
            item = await ItemsClient.GetItemAsync(Id);
        }
        else
        {
            item = restored!;
        }

        if (!ApplicationState.TryTakeFromJson<IEnumerable<OptionDto>>(
            "itemOptions", out var restored2))
        {
            itemOptions = await ItemsClient.GetItemOptionsAsync(Id);
        }
        else
        {
            itemOptions = restored2!;
        }

        if (!ApplicationState.TryTakeFromJson<IEnumerable<AttributeDto>>(
            "itemAttributes", out var restored3))
        {
            itemAttributes = await ItemsClient.GetItemAttributesAsync(Id);
        }
        else
        {
            itemAttributes = restored3!;
        }

        if (!ApplicationState.TryTakeFromJson<ItemsResultOfItemDto>(
            "itemVariants", out var restored4))
        {
            itemVariantResults = await ItemsClient.GetItemVariantsAsync(Id, 1, 10, null, null, null);
        }
        else
        {
            itemVariantResults = restored4!;
        }

        CreateOptionsVM(itemOptions);
        CreateAttributesVM(itemAttributes);

        if (item.HasVariants)
        {
            // INFO: Duplicated 
            var selectedAttributeValues = attributeGroups
                .SelectMany(x => x.Attributes)
                .Where(x => x.ForVariant)
                .Where(x => !x.IsMainAttribute)
                .Where(x => x.SelectedValueId is not null)
                .ToDictionary(x => x.Id, x => x.SelectedValueId);


            variants = await ItemsClient.FindItemVariantByAttributes2Async(Id, selectedAttributeValues);
          
            //await Update(null!);

            variant = variants.First();
        }

        if(VariantId is not null) 
        {
            var item = await ItemsClient.GetItemAsync(VariantId);

            attributeGroups.ForEach(x => x.Attributes.ForEach(x => x.SelectedValueId = null));

            var attrs = attributeGroups.SelectMany(x => x.Attributes);

            foreach(var attr in item.VariantAttributes) 
            {
                var x = attrs.FirstOrDefault(x => x.Id == attr.Id);
                x.SelectedValueId = attr.ValueId;
            }

            await SelectVariant(item);
        }

        LoadData();
    }

    private void CreateOptionsVM(IEnumerable<OptionDto> itemOptions) 
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

            optionGroups.Add(group);
        }
    }

    private void CreateAttributesVM(IEnumerable<AttributeDto> itemAttributes)
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

            attributeGroups.Add(group);
        }
    }

    private void LoadData() 
    {
        if (Data is not null && Data != d)
        {
            var str = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Data));
            var options2 = Deserialize(str);

            foreach (var option in options2)
            {
                var o = optionGroups
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

            d = Data;
        }
    }

    private Task PersistItems()
    {
        ApplicationState.PersistAsJson("item", item);
        ApplicationState.PersistAsJson("itemOptions", itemOptions);
        ApplicationState.PersistAsJson("itemVariants", itemVariantResults);

        return Task.CompletedTask;
    }

    async Task AddItemToCart()
    {
        await CartsClient.AddItemToCartAsync("test", new AddCartItemDto()
        {
            ItemId = variant?.Id ?? item?.Id,
            Quantity = quantity,
            Data = Serialize()
        });

        hasAddedToCart = true;
    }

    public void Dispose()
    {
        persistingSubscription.Dispose();
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

    async Task UpdateUrl()
    {
        string data = Serialize();
        data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));

        await JS.InvokeVoidAsync("skipScroll");

        NavigationManager.NavigateTo($"/items/{Id}?d={data}", forceLoad: false, replace: true);
    }

    string Serialize()
    {
        return JsonSerializer.Serialize(optionGroups.SelectMany(x => x.Options).Select(x =>
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
        }), new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });
    }

    IEnumerable<Option> Deserialize(string str)
    {
        return JsonSerializer.Deserialize<IEnumerable<Option>>(str, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });
    }

    public decimal Total => item.Price
                + optionGroups.SelectMany(x => x.Options)
                .Where(x => x.IsSelected || x.SelectedValueId is not null)
                .Select(x => x.Price.GetValueOrDefault() + (x.Values.FirstOrDefault(x2 => x2.Id == x?.SelectedValueId)?.Price ?? 0))
                .Sum();

    async Task SelectVariant(SiteItemDto variant)
    {
        if (this.variant?.Id == variant?.Id) return;

        this.variant = variant;

        var attributes = attributeGroups.SelectMany(x => x.Attributes);

        foreach (var attr in variant.VariantAttributes)
        {
            var selectedAttr = attributes.First(x => x.Id == attr.Id);
            selectedAttr.SelectedValueId = selectedAttr.Values.FirstOrDefault(x => x.Id == attr.ValueId)?.Id;
        }

        string data = Serialize();
        data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));

        //await JS.InvokeVoidAsync("skipScroll");

        try 
        {
            NavigationManager.NavigateTo($"/items/{Id}/{variant.Id}?d={data}", replace: true);
        }
        catch {}

        StateHasChanged();
    }

    async Task Update(AttributeVM attribute)
    {
        var selectedAttributes = attributeGroups
           .SelectMany(x => x.Attributes)
           .Where(x => x.ForVariant)
           .Where(x => x.SelectedValueId is not null);

        /*
        variants = await ItemsClient.FindItemVariantByAttributes2Async(Id, selectedAttributes
            .Where(x => !x.IsMainAttribute)
            .ToDictionary(x => x.Id, x => x.SelectedValueId)); */

        try
        {
            variant = await ItemsClient.FindItemVariantByAttributesAsync(Id, selectedAttributes.ToDictionary(x => x.Id, x => x.SelectedValueId));

            try
            {
                string data = Serialize();
                data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));

                NavigationManager.NavigateTo($"/items/{Id}/{variant.Id}?d={Data}", replace: true);
            }
            catch (Exception)
            {

            }
        }
        catch (ApiException exc)
        {
            //DialogService.ShowMessageBox("Variant not found", "Could not find a variant for the selected options. Please try again.");
        }
    }
}

