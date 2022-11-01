using Site.Server.Controllers;

namespace Site.Server;

public static class Mapper 
{
    public static SiteItemDto ToDto(this YourBrand.Catalog.ItemDto item) => new SiteItemDto(item.Id, item.Name, item.Description, item.Group?.ToDto(), item.Image, item.Price, item.CompareAtPrice, item.QuantityAvailable, item.Attributes2, item.Options, item.HasVariants, item.Attributes);

    public static SiteItemGroupDto ToDto(this YourBrand.Catalog.ItemGroupDto dto) => new SiteItemGroupDto(dto.Id, dto.Name, dto.Parent?.ToDto());
}