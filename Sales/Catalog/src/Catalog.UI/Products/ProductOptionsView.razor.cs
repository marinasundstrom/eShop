using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace YourBrand.Catalog.Products;

partial class ProductOptionsView : ComponentBase
{
    MudTable<OptionDto> productOptionsTable = default!;

    TableGroupDefinition<OptionDto> tableGroupDefinition = new TableGroupDefinition<OptionDto>()
    {
        GroupName = "Group",
        Indentation = false,
        Expandable = true,
        Selector = (e) => e.Group?.Name
    };

    OptionDto? selectedProductOption;

    string? searchString;

    [Parameter]
    [EditorRequired]
    public string ProductId { get; set; } = default!;

    [Parameter]
    [EditorRequired]
    public IReadOnlyCollection<OptionDto> ProductOptions { get; set; } = default!;

    private bool FilterOptionsFunc(OptionDto productOption)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;

        if (productOption.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
            return true;

        return false;
    }

    private async Task<IEnumerable<OptionValueDto>> SearchOptionValue(string value, CancellationToken cancellationToken)
    {
        /*
        var option = await OptionsClient.GetOptionsAsync(selectedProductOption!.Id, cancellationToken);

        if (value is null)
            return option.Values;

        return option.Values.Where(x => x.Name.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        */

        return Enumerable.Empty<OptionValueDto>();
    }

    private void OnSearch(string text)
    {
        searchString = text;
        productOptionsTable.ReloadServerData();
    }
}

