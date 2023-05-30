using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Queries;

public record GetReceipts(int[]? Status, string? CustomerId, string? SSN, string? AssigneeId, int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<ReceiptDto>>
{
    public class Handler : IRequestHandler<GetReceipts, ItemsResult<ReceiptDto>>
    {
        private readonly IReceiptRepository orderRepository;

        public Handler(IReceiptRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<ItemsResult<ReceiptDto>> Handle(GetReceipts request, CancellationToken cancellationToken)
        {
            var query = orderRepository.GetAll();

            if (request.Status?.Any() ?? false)
            {
                var status = request.Status;
                query = query.Where(x => status.Any(z => z == x.Status.Id));
            }

            if (request.CustomerId is not null)
            {
                query = query.Where(x => x.CustomerId == request.CustomerId);
            }

            if (request.SSN is not null)
            {
                query = query.Where(x => x.BillingDetails!.SSN == request.SSN);
            }

            if (request.AssigneeId is not null)
            {
                query = query.Where(x => x.AssigneeId == request.AssigneeId);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                query = query.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                query = query.OrderByDescending(x => x.Created);
            }

            var orders = await query
                .Include(i => i.Status)
                .Include(i => i.Items)
                .Include(i => i.Assignee)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<ReceiptDto>(orders.Select(x => x.ToDto()), totalCount);
        }
    }
}
