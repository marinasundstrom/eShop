
using System.Text.Json;

namespace YourBrand.Portal.Domain.Entities;

public class Widget : AggregateRoot<Guid> /*, IAuditable */
{
    #nullable disable

    private Widget() 
    {

    }

    #nullable restore

    public Widget(string widgetId, string widgetAreaId, string? userId, JsonDocument? settings)
        : base(Guid.NewGuid())
    {
        WidgetId = widgetId;
        WidgetAreaId = widgetAreaId;
        UserId = userId;
        Settings = settings;
    }

    public string WidgetId { get; private set; } = null!;

    public string WidgetAreaId { get; private set; } = null!;

    public string? UserId { get; private set; } = null!;

    public JsonDocument? Settings { get; private set; } = null!;

    /*
    public User CreatedBy { get; set; } = null!;

    public string CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
    */
}
