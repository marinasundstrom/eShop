using System;

using YourBrand.Inventory.Application.Features.Items.Groups;

namespace YourBrand.Inventory.Application.Features.Items;

public record ItemDto(
    string Id,
    string Name,
    ItemTypeDto Type,
    string? GTIN,
    ItemGroupDto Group,
    string Unit,
    int QuantityAvailable,
    bool? Discontinued);
