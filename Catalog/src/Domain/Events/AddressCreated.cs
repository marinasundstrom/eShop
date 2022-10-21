namespace Catalog.Domain.Events;

public record AddressCreated(string AddressId) : DomainEvent;