using System;

using YourBrand.Inventory.Application.Features.Warehouses;
using YourBrand.Inventory.Application.Features.Items;

namespace YourBrand.Inventory.Application.Features.Items.Groups;

public record ItemGroupDto(
    string Id,
    string Name);
