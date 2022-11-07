namespace YourBrand.Analytics.Application.Statistics.Commands;

public record class Data(string[] Labels, IEnumerable<Series> Series);
