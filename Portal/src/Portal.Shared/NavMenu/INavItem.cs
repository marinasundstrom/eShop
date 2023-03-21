namespace YourBrand.Portal.NavMenu;

public interface INavItem
{
    string Name { get; }

    string? Icon { get; }

    bool Visible { get; }

    bool RequiresAuthorization { get; }

    IEnumerable<string>? Roles { get; }
}
