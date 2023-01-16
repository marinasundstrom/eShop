using System;
using System.Collections.Generic;

namespace YourBrand.Catalog;

public class ProductEditViewModel
{
    private readonly IProductsClient productsClient;
    private readonly IAttributesClient attributesClient;

    private readonly List<ProductAttributeDto> _attributes = new List<ProductAttributeDto>();

    public ProductEditViewModel(IProductsClient productsClient, IAttributesClient attributesClient)
    {
        this.productsClient = productsClient;
        this.attributesClient = attributesClient;
    }

    public async Task InitializeAsync(string id)
    {
        var product = await productsClient.GetProductAsync(id);

        var productOptions = await productsClient.GetProductOptionsAsync(id, null);
    }

    public IEnumerable<ProductAttributeDto> Attributes => _attributes;


}