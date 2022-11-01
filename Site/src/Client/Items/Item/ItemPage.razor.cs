using System;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using System.Xml.Linq;
using Site.Client.Items.Item;

namespace Site.Client.Items.Item;

partial class ItemPage
{
    ProductViewModel? productViewModel;

    int quantity = 1;
    bool hasAddedToCart = false;

    private PersistingComponentStateSubscription persistingSubscription;

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string? VariantId { get; set; }

    [Parameter]
    [SupplyParameterFromQuery(Name = "d")]
    public string? Data { get; set; }

    protected override async Task OnInitializedAsync()
    {
        persistingSubscription =
            ApplicationState.RegisterOnPersisting(PersistItems);

        if (!ApplicationState.TryTakeFromJson<ProductViewModel>(
            "productViewModel", out var restored))
        {
            productViewModel = new ProductViewModel(ItemsClient);
            await productViewModel.Initialize(Id, VariantId);
        }
        else
        {
            productViewModel = restored!;
            productViewModel.SetClient(ItemsClient);
        }

        if(Data is not null) 
        {
            var str = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Data));
            Deserialize(str);
        }
    }

    private Task PersistItems()
    {
        ApplicationState.PersistAsJson("productViewModel", productViewModel);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        persistingSubscription.Dispose();
    }

    async Task UpdateUrl()
    {
        string data = Serialize();
        data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));

        await JS.InvokeVoidAsync("skipScroll");

        if(productViewModel!.VariantId is not null) 
        {
            NavigationManager.NavigateTo($"/items/{Id}/{productViewModel.VariantId}?d={data}", replace: true);
        }
        else 
        {
            NavigationManager.NavigateTo($"/items/{Id}?d={data}", false, replace: true);
        }
    }

    async Task AddItemToCart()
    {
        await CartsClient.AddItemToCartAsync("test", new AddCartItemDto()
        {
            ItemId = productViewModel?.Variant?.Id ?? productViewModel?.Item?.Id,
            Quantity = quantity,
            Data = Serialize()
        });

        hasAddedToCart = true;
    }
    
    string Serialize() 
    {
        return JsonSerializer.Serialize(productViewModel!.GetData(), new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });
    }

    void Deserialize(string str)
    {
        var options = JsonSerializer.Deserialize<IEnumerable<ProductViewModel.Option>>(str, new JsonSerializerOptions
        {
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        });

        productViewModel!.LoadData(options!);
    }


    /*
    async Task UpdateUrl()
    {
        string data = Serialize();
        data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));

        await JS.InvokeVoidAsync("skipScroll");

        NavigationManager.NavigateTo($"/items/{Id}?d={data}", forceLoad: false, replace: true);
    }
    */
}

