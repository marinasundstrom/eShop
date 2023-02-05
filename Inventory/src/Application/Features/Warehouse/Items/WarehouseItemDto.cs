using System;

using YourBrand.Inventory.Application.Features.Warehouses;
using YourBrand.Inventory.Application.Features.Items;

namespace YourBrand.Inventory.Application.Features.Warehouses.Items;

public record WarehouseItemDto(
    string Id,
    ItemDto Item,
    WarehouseDto Warehouse,
    string Location,
    int QuantityOnHand,
    int QuantityPicked,
    int QuantityReserved,
    int QuantityAvailable,
    int QuantityThreshold);
