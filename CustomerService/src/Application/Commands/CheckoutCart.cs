using FluentValidation;
using MediatR;

namespace YourBrand.CustomerService.Application.CustomerService.Commands;

public sealed record CheckoutIssue(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<ClearIssue>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler : IRequestHandler<CheckoutIssue, Result>
    {
        private readonly IIssueRepository issueRepository;
        private readonly IUnitOfWork unitOfWork;

        public Handler(IIssueRepository issueRepository, IUnitOfWork unitOfWork)
        {
            this.issueRepository = issueRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CheckoutIssue request, CancellationToken cancellationToken)
        {
            var issue = await issueRepository.FindByIdAsync(request.Id, cancellationToken);

            if (issue is null)
            {
                return Result.Failure(Errors.CustomerService.IssueNotFound);
            }

            issue.Checkout();

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
