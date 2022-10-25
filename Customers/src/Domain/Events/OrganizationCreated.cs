using YourBrand.Customers.Domain;

namespace YourBrand.Customers.Domain.Events;

public record OrganizationCreated(string OrganizationId) : DomainEvent;