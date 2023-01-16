using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using YourBrand.Catalog.Attributes;

namespace YourBrand.Catalog.Products;

partial class ProductEdit : ComponentBase
{
    ProductDto? product;

    [Parameter]
    public string? ProductId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        NavigationManager.LocationChanged += NavigationManager_LocationChanged;

        await LoadAsync();
    }

    private async void NavigationManager_LocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
    {
        await LoadAsync();

        StateHasChanged();
    }

    private async Task LoadAsync()
    {
        product = await ProductsClient.GetProductAsync(ProductId);
        productVariants = (await ProductsClient.GetVariantsAsync(ProductId, 0, 20, null, null, null)).Items;
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
    }

    MudTable<ProductDto> productVariantsTable = default!;

    IEnumerable<ProductDto> productVariants = Enumerable.Empty<ProductDto>();

    ProductDto? selectedProductVariant;
    ProductDto? productVariantBeforeEdit;

    string? searchString;

    private bool FilterProductVariantsFunc(ProductDto productVariant)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        if (productVariant.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            return true;

        return false;
    }

    private void OnSearch(string text)
    {
        searchString = text;
        productVariantsTable.ReloadServerData();
    }

    private async Task ShowCreateVariantDialog()
    {
        var parameter = new DialogParameters()
        {
            { nameof(Variants.CreateProductVariantDialog.ProductId), ProductId }
        };
        await DialogService.ShowAsync<Variants.CreateProductVariantDialog>("Create variant", parameter);
    }
}

