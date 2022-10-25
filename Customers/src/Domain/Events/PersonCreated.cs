using YourBrand.Customers.Domain;

namespace YourBrand.Customers.Domain.Events;

public record PersonCreated(string PersonId) : DomainEvent;