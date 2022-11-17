namespace YourBrand.CustomerService.Domain.Events;

public sealed record IssueCreated(string IssueId) : DomainEvent;
