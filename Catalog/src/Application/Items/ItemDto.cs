using YourBrand.Catalog.Application.Attributes;
using YourBrand.Catalog.Application.Options;
using YourBrand.Catalog.Application.Items.Groups;

namespace YourBrand.Catalog.Application.Items;

public record class ItemDto(string Id, string Name, string? Description, ItemGroupDto? Group, string? Image, decimal Price, decimal? CompareAtPrice, int? QuantityAvailable, bool HasVariants, ItemVisibility? Visibility, IEnumerable<AttributeDto> Attributes2, IEnumerable<OptionDto> Options, IEnumerable<ItemVariantAttributeDto> Attributes);

