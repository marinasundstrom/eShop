namespace YourBrand.Catalog.Application.Options;

public record class OptionValueDto(string Id, string Name, string? InventoryItemId, decimal? Price, int? Seq);

