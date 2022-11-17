using FluentValidation;
using MediatR;
using YourBrand.CustomerService.Application.CustomerService.Dtos;

namespace YourBrand.CustomerService.Application.CustomerService.Queries;

public record GetIssueById(string Id) : IRequest<Result<IssueDto>>
{
    public class Validator : AbstractValidator<GetIssueById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<GetIssueById, Result<IssueDto>>
    {
        private readonly IIssueRepository issueRepository;

        public Handler(IIssueRepository issueRepository)
        {
            this.issueRepository = issueRepository;
        }

        public async Task<Result<IssueDto>> Handle(GetIssueById request, CancellationToken cancellationToken)
        {
            var issue = await issueRepository.FindByIdAsync(request.Id, cancellationToken);

            if (issue is null)
            {
                return Result.Failure<IssueDto>(Errors.CustomerService.IssueNotFound);
            }

            return Result.Success(issue.ToDto());
        }
    }
}
