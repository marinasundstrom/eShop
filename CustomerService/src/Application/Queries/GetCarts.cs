using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.CustomerService.Application.Common;
using YourBrand.CustomerService.Application.CustomerService.Dtos;

namespace YourBrand.CustomerService.Application.CustomerService.Queries;

public record GetCustomerService(int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<IssueDto>>
{
    public class Handler : IRequestHandler<GetCustomerService, ItemsResult<IssueDto>>
    {
        private readonly IIssueRepository issueRepository;

        public Handler(IIssueRepository issueRepository)
        {
            this.issueRepository = issueRepository;
        }

        public async Task<ItemsResult<IssueDto>> Handle(GetCustomerService request, CancellationToken cancellationToken)
        {
            var query = issueRepository.GetAll();

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var CustomerService = await query
                .Include(i => i.Items)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<IssueDto>(CustomerService.Select(x => x.ToDto()), totalCount);
        }
    }
}
