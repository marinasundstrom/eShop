using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.CustomerService.Application.CustomerService.Dtos;

namespace YourBrand.CustomerService.Application.CustomerService.Commands;

public sealed record CreateIssue(string? Tag) : IRequest<Result<IssueDto>>
{
    public sealed class Validator : AbstractValidator<CreateIssue>
    {
        public Validator()
        {

        }
    }

    public sealed class Handler : IRequestHandler<CreateIssue, Result<IssueDto>>
    {
        private readonly IIssueRepository issueRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IDomainEventDispatcher domainEventDispatcher;

        public Handler(IIssueRepository issueRepository, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
        {
            this.issueRepository = issueRepository;
            this.unitOfWork = unitOfWork;
            this.domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Result<IssueDto>> Handle(CreateIssue request, CancellationToken cancellationToken)
        {
            var issue = new Issue(request.Tag);

            issueRepository.Add(issue);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            await domainEventDispatcher.Dispatch(new IssueCreated(issue.Id), cancellationToken);

            issue = await issueRepository.GetAll()
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .FirstAsync(x => x.Id == issue.Id, cancellationToken);

            return Result.Success(issue!.ToDto());
        }
    }
}
