namespace YourBrand.Catalog.Features.Options;

public record class OptionValueDto(string Id, string Name, string? SKU, decimal? Price, int? Seq);