using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Xml.Linq;
using System.Text.Json;

namespace Site.Client.Pages;

partial class ItemPage
{
    SiteItemDto? item;
    IEnumerable<OptionDto>? itemOptions;
    IEnumerable<AttributeDto>? itemAttributes;
    ItemsResultOfItemDto? itemVariantResults;
    List<OptionGroupVM> optionGroups = new List<OptionGroupVM>();

    int quantity = 1;
    bool hasAddedToCart = false;

    private PersistingComponentStateSubscription persistingSubscription;

    [Parameter]
    public string GroupId { get; set; }

    [Parameter]
    public string Id { get; set; }

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
                group.Options.Add(new OptionVM
                {
                    Id = option.Id,
                    Name = option.Name,
                    Description = option.Description,
                    Group = option.Group,
                    OptionType = option.OptionType,
                    Price = option.Price,
                    ItemId = option.ItemId,
                    IsSelected = option.IsSelected,
                    Values = option.Values,
                    SelectedValueId = option.DefaultValue?.Id,
                    MinNumericalValue = option.MinNumericalValue,
                    MaxNumericalValue = option.MaxNumericalValue,
                    NumericalValue = option.DefaultNumericalValue,
                    TextValue = option.DefaultTextValue,
                    TextValueMaxLength = option.TextValueMaxLength,
                    TextValueMinLength = option.TextValueMinLength
                });
            }

            optionGroups.Add(group);
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
            ItemId = item.Id,
            Quantity = quantity,
            Data = Serialize()
        });

        hasAddedToCart = true;
    }

    void IDisposable.Dispose()
    {
        persistingSubscription.Dispose();
    }

    public class OptionGroupVM
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int? Min { get; set; }

        public int? Max { get; set; }

        public List<OptionVM> Options { get; } = new List<OptionVM>();
    }

    public class OptionVM
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public OptionType OptionType { get; set; }

        public OptionGroupDto Group { get; set; } = null!;

        public string? ItemId { get; set; }

        public decimal? Price { get; set; }

        public bool IsSelected { get; set; }

        public IEnumerable<OptionValueDto>? Values { get; set; }

        public string? SelectedValueId { get; set; }

        public int? MinNumericalValue { get; set; }

        public int? MaxNumericalValue { get; set; }

        public int? NumericalValue { get; set; }

        public string? TextValue { get; set; }

        public int? TextValueMinLength { get; set; }

        public int? TextValueMaxLength { get; set; }
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

        NavigationManager.NavigateTo($"/items/{GroupId}/{Id}?d={data}", forceLoad: false, replace: true);
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
}

