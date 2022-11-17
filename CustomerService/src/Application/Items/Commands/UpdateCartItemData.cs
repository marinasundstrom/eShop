using FluentValidation;
using MediatR;

namespace YourBrand.CustomerService.Application.CustomerService.Items.Commands;

public sealed record UpdateIssueItemData(string IssueId, string IssueItemId, string? Data) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<RemoveIssueItem>
    {
        public Validator()
        {
            RuleFor(x => x.IssueId).NotEmpty();

            RuleFor(x => x.IssueItemId).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<UpdateIssueItemData, Result>
    {
        private readonly IIssueRepository issueRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IIssueRepository issueRepository, IUnitOfWork unitOfWork)
        {
            this.issueRepository = issueRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateIssueItemData request, CancellationToken cancellationToken)
        {
            var issue = await issueRepository.FindByIdAsync(request.IssueId, cancellationToken);

            if (issue is null)
            {
                return Result.Failure(Errors.CustomerService.IssueNotFound);
            }

            var issueItem = issue.Items.FirstOrDefault(x => x.Id == request.IssueItemId);

            if (issueItem is null)
            {
                throw new System.Exception();
            }

            issueItem.UpdateData(request.Data);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
