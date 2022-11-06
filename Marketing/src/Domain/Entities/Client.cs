namespace YourBrand.Marketing.Domain.Entities;

public class Client : Entity<string>
{
#nullable disable

    protected Client() : base() { }

#nullable restore

    public Client(string id, string browser)
    : base(id)
    {
        Browser = browser;
    }
    
    public string Browser { get; private set; } = default!;

    public IReadOnlyCollection<Session> Sessions { get; private set; } = new HashSet<Session>();
}
