using YourBrand.Customers.Domain;

namespace YourBrand.Customers.Domain.Events;

public record AddressCreated(string AddressId) : DomainEvent;