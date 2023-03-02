using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace YourBrand.Catalog.Products;

partial class ProductAttributesView : ComponentBase
{
    MudTable<ProductAttributeDto> productAttributesTable = default!;
    TableGroupDefinition<ProductAttributeDto> tableGroupDefinition = new TableGroupDefinition<ProductAttributeDto>()
    {
        GroupName = "Group",
        Indentation = false,
        Expandable = true,
        Selector = (e) => e.Attribute.Group?.Name
    };

    ProductAttributeDto? selectedProductAttribute;
    ProductAttributeDto? productAttributeBeforeEdit;

    string? searchString;

    [Parameter]
    [EditorRequired]
    public long ProductId { get; set; } = default!;

    [Parameter]
    public bool HasVariants { get; set; } = false!;

    [Parameter]
    [EditorRequired]
    public IReadOnlyCollection<ProductAttributeDto> ProductAttributes { get; set; } = default!;

    private void BackupItem(object productAttribute)
    {
        productAttributeBeforeEdit = new()
        {
            Attribute = ((ProductAttributeDto)productAttribute).Attribute,
            Value = ((ProductAttributeDto)productAttribute).Value
        };
    }

    private async void ItemHasBeenCommitted(object productAttribute)
    {
        if (productAttribute is ProductAttributeDto pa)
        {
            await ProductsClient.UpdateProductAttributeAsync(ProductId, pa.Attribute.Id, new UpdateProductAttributeDto
            {
                ValueId = pa.Value.Id
            });
        }
    }

    private void ResetItemToOriginalValues(object productAttribute)
    {
        ((ProductAttributeDto)productAttribute).Attribute = productAttributeBeforeEdit!.Attribute;
        ((ProductAttributeDto)productAttribute).Value = productAttributeBeforeEdit.Value;
    }

    private bool FilterAttributesFunc(ProductAttributeDto productAttribute)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        if (productAttribute.Attribute.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            return true;

        return false;
    }

    private async Task<IEnumerable<AttributeValueDto>> SearchAttributeValue(string value, CancellationToken cancellationToken)
    {
        var attribute = await AttributesClient.GetAttributeAsync(selectedProductAttribute!.Attribute.Id, cancellationToken);

        if (value is null)
            return attribute.Values;

        return attribute.Values.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private void OnSearch(string text)
    {
        searchString = text;
        productAttributesTable.ReloadServerData();
    }
}

