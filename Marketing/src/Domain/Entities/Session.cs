using System;

namespace YourBrand.Marketing.Domain.Entities;

public class Session : Entity<string>
{
#nullable disable

    protected Session() : base() { }

#nullable restore

    public Session(string clientId, DateTimeOffset startTime)
    : base(Guid.NewGuid().ToString())
    {
        ClientId = clientId;
        StartTime = startTime;
    }

    public string ClientId { get; private set; }  = default!;

    public Client Client { get; private set; }  = default!;

    public DateTimeOffset StartTime { get; private set; } = DateTimeOffset.UtcNow;
}
