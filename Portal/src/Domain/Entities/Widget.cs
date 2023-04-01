
using System.Text.Json;

namespace YourBrand.Portal.Domain.Entities;

public sealed class Widget : AggregateRoot<Guid> /*, IAuditable */
{
    #nullable disable

    private Widget() 
    {

    }

    #nullable restore

    public Widget(string widgetId, string? userId, JsonDocument? settings)
        : base(Guid.NewGuid())
    {
        WidgetId = widgetId;
        UserId = userId;
        Settings = settings;
    }

    public string WidgetId { get; private set; } = null!;

    public string WidgetAreaId { get; private set; } = default!;

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


public sealed class WidgetArea : AggregateRoot<string> /*, IAuditable */
{
    HashSet<Widget> widgets = new HashSet<Widget>();

    #nullable disable

    private WidgetArea() 
    {

    }

    #nullable restore

    public WidgetArea(string id, string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; set; }

    public IReadOnlyCollection<Widget> Widgets => widgets;

    public void AddWidget(Widget widget) 
    {
        widgets.Add(widget);
    }

    public void RemoveWidget(Widget widget) 
    {
        widgets.Remove(widget);
    }

    /*
    public User CreatedBy { get; set; } = null!;

    public string CreatedById { get; set; } = null!;

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public string? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
    */
}
