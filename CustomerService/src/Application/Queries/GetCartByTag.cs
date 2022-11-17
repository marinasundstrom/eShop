using FluentValidation;
using MediatR;
using YourBrand.CustomerService.Application.CustomerService.Dtos;

namespace YourBrand.CustomerService.Application.CustomerService.Queries;

public record GetIssueByTag(string Tag) : IRequest<Result<IssueDto>>
{
    public class Validator : AbstractValidator<GetIssueById>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<GetIssueByTag, Result<IssueDto>>
    {
        private readonly IIssueRepository issueRepository;

        public Handler(IIssueRepository issueRepository)
        {
            this.issueRepository = issueRepository;
        }

        public async Task<Result<IssueDto>> Handle(GetIssueByTag request, CancellationToken cancellationToken)
        {
            var issue = await issueRepository.FindByTagAsync(request.Tag, cancellationToken);

            if (issue is null)
            {
                return Result.Failure<IssueDto>(Errors.CustomerService.IssueNotFound);
            }

            return Result.Success(issue.ToDto());
        }
    }
}