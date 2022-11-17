using System;
using MediatR;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;

namespace YourBrand.CustomerService.Application.CustomerService.EventHandlers;

public sealed class IssueCreatedEventHandler : IDomainEventHandler<IssueCreated>
{
    private readonly IIssueRepository issueRepository;

    public IssueCreatedEventHandler(IIssueRepository issueRepository)
    {
        this.issueRepository = issueRepository;
    }

    public async Task Handle(IssueCreated notification, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.FindByIdAsync(notification.IssueId, cancellationToken);

        if (issue is null)
            return;
    }
}

