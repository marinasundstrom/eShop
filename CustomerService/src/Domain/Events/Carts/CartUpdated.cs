namespace YourBrand.CustomerService.Domain.Events;

public sealed record IssueUpdated(string IssueId) : DomainEvent;