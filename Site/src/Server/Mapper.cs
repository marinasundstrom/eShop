using Site.Server.Controllers;

namespace Site.Server;

public static class Mapper 
{
    public static SiteItemDto ToDto(this YourBrand.Catalog.ItemDto item, string? description = null) => 
        new SiteItemDto(item.Id, item.Name, !string.IsNullOrEmpty(description) ? description : item.Description, item.Parent?.ToDto2(), item.Group?.ToDto(), item.Image, item.Price, item.CompareAtPrice, item.QuantityAvailable, item.Attributes2, item.Options, item.HasVariants, item.Attributes);

    public static SiteParentItemDto ToDto2(this YourBrand.Catalog.ParentItemDto item) => 
        new SiteParentItemDto(item.Id, item.Name, item.Description, item.Group?.ToDto());

    public static SiteItemGroupDto ToDto(this YourBrand.Catalog.ItemGroupDto dto) => new SiteItemGroupDto(dto.Id, dto.Name, dto.Parent?.ToDto());
}