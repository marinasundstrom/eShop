namespace Catalog.Domain.Events;

public record PersonCreated(string PersonId) : DomainEvent;