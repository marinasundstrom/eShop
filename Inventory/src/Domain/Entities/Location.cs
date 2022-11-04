namespace YourBrand.Inventory.Domain.Entities;

public class Location : Entity<string>, IAuditable
{
    protected Location() { }

    public Location(string id, string inventoryId, string? part1, string? part2, string? part3, string? part4)
    {
        Id = id;
        WarehouseId = inventoryId;
        Aisle = part1;
        Unit = part2;
        Shelf = part3;
        Bin = part4;
    }

    public string WarehouseId { get; } = null!;

    public string? Aisle { get; set; } // Aisle

    public string? Unit { get; set; } // Unit/Rack

    public string? Shelf { get; set; } // Shelf/Row

    public string? Bin { get; set; } // Bin

    public string? CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}
