using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Site.Client.Products;

partial class ProductsPage
{
    ProductGroupDto? productGroup;
    IEnumerable<ProductGroupDto>? subGroups;
    ItemsResultOfSiteProductDto? itemResults;

    int pageSize = 10;
    int totalPages = 0;

    private PersistingComponentStateSubscription persistingSubscription;

    [Parameter]
    public string? Path { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int? Page { get; set; } = 1;

    protected override async Task OnInitializedAsync()
    {
        NavigationManager.LocationChanged += OnLocationChanged;

        await LoadData();

        if (!RenderingContext.IsPrerendering)
        {
            _ = ProductGroupViewed();
        }
    }

    private async Task ProductGroupViewed()
    {
        await AnalyticsService.RegisterEvent(new EventData
        {
            EventType = EventType.ProductGroupViewed,
            Data = new Dictionary<string, object>
            {
                { "groupId", productGroup!.Id },
                { "name", GetGroupName() ?? productGroup.Name }
            }
        });
    }

    private string? GetGroupName()
    {
        return subGroups.FirstOrDefault(x => x.Path == Path)?.Name;
    }

    private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (e.Location.Contains("/groups"))
        {
            await LoadData();

            StateHasChanged();

            _ = ProductGroupViewed();
        }
    }

    public async Task LoadData()
    {
        if (Page is null)
        {
            Page = 1;
        }

        persistingSubscription =
            ApplicationState.RegisterOnPersisting(PersistItems);

        Console.WriteLine(Path);

        if (!string.IsNullOrEmpty(Path))
        {
            if (!ApplicationState.TryTakeFromJson<ProductGroupDto>(
                "productGroup", out var restored01))
            { 
                
                productGroup = await ProductsClient.GetProductGroupAsync(Path);
            }
            else
            {
                productGroup = restored01!;
            }
        }

        if (!ApplicationState.TryTakeFromJson<IEnumerable<ProductGroupDto>>(
            "productGroups", out var restored0))
        { 
            var id = productGroup?.Parent?.Id ?? productGroup?.Id;

            subGroups = await ProductsClient.GetProductGroupsAsync(id, true);
        }
        else
        {
            subGroups = restored0!;
        }

        if (!ApplicationState.TryTakeFromJson<ItemsResultOfSiteProductDto>(
            "itemResults", out var restored))
        {
            itemResults = await ProductsClient.GetProductsAsync(Path, Page.GetValueOrDefault(), pageSize, null, null, null);
        }
        else
        {
            itemResults = restored!;
        }

        if (itemResults.TotalItems < pageSize)
        {
            totalPages = 1;
            return;
        }

        totalPages = (int)Math.Ceiling((double)(itemResults.TotalItems / pageSize));
    }

    private Task PersistItems()
    {
        ApplicationState.PersistAsJson("itemResults", itemResults);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        persistingSubscription.Dispose();
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    async Task OnPageChanged(int page)
    {
        /*
        Page = page;
        await LoadData();
        StateHasChanged();
        */
    }

    async Task AddItemToCart(SiteProductDto product)
    {
        await CartClient.AddItemToCartAsync(new AddCartItemDto()
        {
            ProductId = product.Id.ToString(),
            Quantity = 1
         });
    }

    public string SelectedStyle(string path) => NavigationManager.Uri.EndsWith(path) ? "primary" : "secondary";
}
