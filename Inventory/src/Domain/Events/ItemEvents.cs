namespace YourBrand.Inventory.Domain.Events;

public record ItemCreated(string ItemId) : DomainEvent;

public record WarehouseItemCreated(string WarehouseItemId, string ItemId, string WarehouseId) : DomainEvent;

public record WarehouseItemQuantityOnHandUpdated(string WarehouseItemId, string ItemId, string WarehouseId, int Quantity, int OldQuantity) : DomainEvent;

public record WarehouseItemsPicked(string WarehouseItemId, string ItemId, string WarehouseId, int Quantity) : DomainEvent;

public record WarehouseItemsReserved(string WarehouseItemId, string ItemId, string WarehouseId, int Quantity) : DomainEvent;

public record WarehouseItemQuantityAvailableUpdated(string WarehouseItemId, string ItemId, string WarehouseId, int Quantity, int OldQuantity) : DomainEvent;