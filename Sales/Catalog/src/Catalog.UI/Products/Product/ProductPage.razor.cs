using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;

using YourBrand.Catalog.Products.Product;

namespace YourBrand.Catalog.Products.Product;

partial class ProductPage : ComponentBase
{
    bool loading = false;
    ProductViewModel? productViewModel;

    [Parameter]
    public string Id { get; set; }

    [Parameter]
    public string? VariantId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var pwm = new ProductViewModel(ProductsClient);
        loading = true;
        await pwm.Initialize(Id, VariantId);
        productViewModel = pwm;
        loading = false;
    }
}