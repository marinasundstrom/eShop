using System;
using YourBrand.Marketing.Domain.ValueObjects;

namespace YourBrand.Marketing.Domain.Entities;

public class Session : Entity<string>
{
#nullable disable

    protected Session() : base() { }

#nullable restore

    public Session(string clientId, DateTimeOffset started)
    : base(Guid.NewGuid().ToString())
    {
        ClientId = clientId;
        Started = started;
        Expires = started.AddMinutes(30);
    }

    public string ClientId { get; private set; }  = default!;

    public Client Client { get; private set; }  = default!;

    public Coordinates? Coordinates  { get; set; }

    public DateTimeOffset Started { get; private set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset Expires { get; set; }
}
