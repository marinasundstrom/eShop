using MediatR;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.Services;

namespace YourBrand.CustomerService.Application.CustomerService.EventHandlers;

public sealed class IssueUpdatedEventHandler : IDomainEventHandler<IssueUpdated>
{
    private readonly IIssueRepository issueRepository;

    public IssueUpdatedEventHandler(IIssueRepository issueRepository)
    {
        this.issueRepository = issueRepository;
    }

    public async Task Handle(IssueUpdated notification, CancellationToken cancellationToken)
    {
        var issue = await issueRepository.FindByIdAsync(notification.IssueId, cancellationToken);

        if (issue is null)
            return;
    }
}
