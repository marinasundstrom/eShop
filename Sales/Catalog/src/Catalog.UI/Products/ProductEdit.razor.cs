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
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
    }

    private async void UploadFiles(InputFileChangeEventArgs e)
    {
        var file = e.GetMultipleFiles().First();
        product!.Image = await ProductsClient.UploadProductImageAsync(product.Id, new FileParameter(file.OpenReadStream(3 *
        1000000), file.Name));

        StateHasChanged();
    }
}

