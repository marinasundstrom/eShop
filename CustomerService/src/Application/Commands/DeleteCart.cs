using FluentValidation;
using MediatR;

namespace YourBrand.CustomerService.Application.CustomerService.Commands;

public sealed record DeleteIssue(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteIssue>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<DeleteIssue, Result>
    {
        private readonly IIssueRepository issueRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IIssueRepository issueRepository, IUnitOfWork unitOfWork)
        {
            this.issueRepository = issueRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteIssue request, CancellationToken cancellationToken)
        {
            var issue = await issueRepository.FindByIdAsync(request.Id, cancellationToken);

            if (issue is null)
            {
                return Result.Failure(Errors.CustomerService.IssueNotFound);
            }

            issueRepository.Remove(issue);

            issue.AddDomainEvent(new IssueDeleted(issue.Id));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
