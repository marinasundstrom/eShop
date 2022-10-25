using YourBrand.Sales.Domain;
using YourBrand.Sales.Domain.Enums;
using YourBrand.Sales.Domain.Events;

namespace YourBrand.Sales.Domain.Entities;

public class Address : AuditableEntity
{
    public Address()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; private set; } = null!;

    // Street
    public string Thoroughfare { get; set; } = null!;

    // Street number
    public string? Premises { get; set; }

    // Suite
    public string? SubPremises { get; set; }

    public string PostalCode { get; set; } = null!;

    // Town or City
    public string Locality { get; set; } = null!;

    // County
    public string SubAdministrativeArea { get; set; } = null!;

    // State
    public string AdministrativeArea { get; set; } = null!;

    public string Country { get; set; } = null!;
}