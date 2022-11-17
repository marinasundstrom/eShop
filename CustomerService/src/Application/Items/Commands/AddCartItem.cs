using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.CustomerService.Application.CustomerService.Dtos;

namespace YourBrand.CustomerService.Application.CustomerService.Items.Commands;

public sealed record AddIssueItem(string IssueId, string? ItemId, double Quantity, string? Data) : IRequest<Result<IssueItemDto>>
{
    public sealed class Validator : AbstractValidator<AddIssueItem>
    {
        public Validator()
        {
            RuleFor(x => x.IssueId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.ItemId).NotEmpty().MaximumLength(60);

            RuleFor(x => x.Quantity);
        }
    }

    public sealed class Handler : IRequestHandler<AddIssueItem, Result<IssueItemDto>>
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

        public async Task<Result<IssueItemDto>> Handle(AddIssueItem request, CancellationToken cancellationToken)
        {
            var issue = await issueRepository.FindByIdAsync(request.IssueId, cancellationToken);

            if (issue is null)
            {
                return Result.Failure<IssueItemDto>(Errors.CustomerService.IssueNotFound);
            }

            var issueItem = issue.AddIssueItem(request.ItemId, request.Quantity, request.Data);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(issueItem!.ToDto());
        }
    }
}
