using Site.Server.Controllers;

namespace Site.Server;

public static class Mapper 
{
    public static SiteItemGroupDto ToDto(this YourBrand.Catalog.ItemGroupDto dto) => new SiteItemGroupDto(dto.Id, dto.Name, dto.Parent?.ToDto());
}