namespace YourBrand.Marketing.Application.Analytics.Commands;

public record class Data(string[] Labels, IEnumerable<Series> Series);
