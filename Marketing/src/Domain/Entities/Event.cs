using System;
using YourBrand.Marketing.Domain.Events;
using YourBrand.Marketing.Domain.Enums;

namespace YourBrand.Marketing.Domain.Entities;

public class Event : Entity<string>
{
#nullable disable

    protected Event() : base() { }

#nullable restore

    public Event(EventType eventType, string data)
    : base(Guid.NewGuid().ToString())
    {
        EventType = eventType;
        Data = data;
    }

    public string ClientId { get; private set; } = "10";

    public EventType EventType { get; private set; }
    
    public DateTimeOffset DateTime { get; private set; } = DateTimeOffset.UtcNow;

    public string Data { get; private set; }
}
