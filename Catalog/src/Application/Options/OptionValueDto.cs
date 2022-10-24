namespace YourBrand.Catalog.Application.Options;

public record class OptionValueDto(string Id, string Name, string? ItemId, decimal? Price, int? Seq);

