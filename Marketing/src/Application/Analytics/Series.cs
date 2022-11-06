namespace YourBrand.Marketing.Application.Analytics.Commands;

public record class Series(string Name, IEnumerable<decimal> Data);