using System.ComponentModel.DataAnnotations;

using YourBrand.Inventory.Application;
using YourBrand.Inventory.Application.Common.Models;
using YourBrand.Inventory.Application.Features.Sites;

namespace YourBrand.Inventory.Application.Features.Warehouses;

public record WarehouseDto
(
    string Id,
    string Name,
    SiteDto Site
);
