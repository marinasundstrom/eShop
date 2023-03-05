using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Site.Client.Products;

partial class ProductsPage
{
    ProductGroupDto? productGroup;
    IEnumerable<ProductGroupDto>? itemGroups;
    IEnumerable<ProductGroupDto>? subGroups;
    ItemsResultOfSiteProductDto? itemResults;

    int pageSize = 10;
    int totalPages = 0;

    private PersistingComponentStateSubscription persistingSubscription;

    [Parameter]
    public string? GroupId { get; set; }

    [Parameter]
    public string? Group2Id { get; set; }

    [Parameter]
    public string? Group3Id { get; set; }

    [Parameter]
    [SupplyParameterFromQuery]
    public int? Page { get; set; } = 1;

    protected override async Task OnInitializedAsync()
    {
        NavigationManager.LocationChanged += OnLocationChanged;

        await LoadData();

        if (!RenderingContext.IsPrerendering)
        {
            _ = ItemGroupViewed();
        }
    }

    private async Task ItemGroupViewed()
    {
        await AnalyticsService.RegisterEvent(new EventData
        {
            EventType = EventType.ItemGroupViewed,
            Data = new Dictionary<string, object>
            {
                { "groupId", Group3Id ?? Group2Id ?? GroupId ?? productGroup!.Handle },
                { "name", GetGroupName() ?? productGroup.Name }
            }
        });
    }

    private string? GetGroupName()
    {
        var groupId = Group3Id ?? Group2Id ?? GroupId;
        return subGroups.FirstOrDefault(x => x.Handle == groupId)?.Name;
    }

    private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (e.Location.Contains("/groups"))
        {
            await LoadData();

            StateHasChanged();

            _ = ItemGroupViewed();
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

        if (!ApplicationState.TryTakeFromJson<IEnumerable<ProductGroupDto>>(
            "itemGroups", out var restored0))
        {
            itemGroups = await ProductsClient.GetProductGroupsAsync(null, true);
        }
        else
        {
            itemGroups = restored0!;
        }

        if (GroupId is not null)
        {
            productGroup = itemGroups.FirstOrDefault(x => x.Handle == GroupId);

            subGroups = await ProductsClient.GetProductGroupsAsync(productGroup?.Id, true);
        }
        else
        {
            productGroup = null;
        }

        if (!ApplicationState.TryTakeFromJson<ItemsResultOfSiteProductDto>(
            "itemResults", out var restored))
        {
            itemResults = await ProductsClient.GetProductsAsync(GroupId, Group2Id, Group3Id, Page.GetValueOrDefault(), pageSize, null, null, null);
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

    private string GetPath(ProductGroupDto group)
    {
        var str = new System.Text.StringBuilder();

        str.Append("/groups");

        GetPath(str, group);

        return str.ToString();
    }

    private void GetPath(System.Text.StringBuilder sb, ProductGroupDto group)
    {
        if (group.Parent is not null)
        {
            GetPath(sb, group.Parent);
        }

        sb.Append($"/{group.Handle}");
    }

    async Task AddItemToCart(SiteProductDto product)
    {
        await CartClient.AddItemToCartAsync(new AddCartItemDto()
        {
            ProductId = product.Id.ToString(),
            Quantity = 1
         });
    }

    public string SelectedStyle(string path) => NavigationManager.Uri.Contains(path) ? "primary" : "secondary";
}

