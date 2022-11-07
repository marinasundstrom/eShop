namespace YourBrand.Analytics.Application.Statistics.Commands;

public record class Series(string Name, IEnumerable<decimal> Data);