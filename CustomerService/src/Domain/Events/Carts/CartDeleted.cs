namespace YourBrand.CustomerService.Domain.Events;

public sealed record IssueDeleted(string IssueId) : DomainEvent;