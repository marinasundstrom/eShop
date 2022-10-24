using YourBrand.Catalog.Application.Items.Groups;

namespace YourBrand.Catalog.Application.Items;

public record class ItemDto(string Id, string Name, string? Description, ItemGroupDto? Group, string? Image, decimal? Price, bool HasVariants, ItemVisibility? Visibility, IEnumerable<ItemVariantAttributeDto> Attributes);

