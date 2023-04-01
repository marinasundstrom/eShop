namespace YourBrand.Portal.Features.Widgets;

public sealed record WidgetDto(Guid Id, string WidgetId, string WidgetAreaId, string? UserId, string? Settings);
