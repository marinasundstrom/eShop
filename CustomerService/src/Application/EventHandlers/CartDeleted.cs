using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;
using YourBrand.CustomerService.Domain.Entities;

namespace YourBrand.CustomerService.Application.CustomerService.EventHandlers;

public sealed class IssueDeletedEventHandler : IDomainEventHandler<IssueDeleted>
{
    private readonly IIssueRepository issueRepository;

    public IssueDeletedEventHandler(IIssueRepository issueRepository)
    {
        this.issueRepository = issueRepository;
    }

    public async Task Handle(IssueDeleted notification, CancellationToken cancellationToken)
    {
    }
}

