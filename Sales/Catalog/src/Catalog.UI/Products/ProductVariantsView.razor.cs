using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace YourBrand.Catalog.Products;

partial class ProductVariantsView : ComponentBase
{
    MudTable<ProductDto> productVariantsTable = default!;

    IEnumerable<ProductDto> productVariants = Enumerable.Empty<ProductDto>();

    ProductDto? selectedProductVariant;
    ProductDto? productVariantBeforeEdit;

    string? searchString;

    [Parameter]
    public long? ProductId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        productVariants = (await ProductsClient.GetVariantsAsync(ProductId.GetValueOrDefault().ToString(), 0, 20, null, null, null)).Items;
    }

    private bool FilterProductVariantsFunc(ProductDto productVariant)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        if (productVariant.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            return true;

        if (productVariant.Sku?.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) ?? false)
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

