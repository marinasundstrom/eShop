using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.EntityFrameworkCore;


using YourBrand.Payments.Domain.Entities;
using YourBrand.Payments.Domain;
using YourBrand.Payments.Application.Common;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Statuses.Queries;

public record GetReceiptStatusesQuery(int Page = 0, int PageSize = 10, string? SearchString = null, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<ReceiptStatusDto>>
{
    class GetReceiptStatusesQueryHandler : IRequestHandler<GetReceiptStatusesQuery, ItemsResult<ReceiptStatusDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetReceiptStatusesQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<ItemsResult<ReceiptStatusDto>> Handle(GetReceiptStatusesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ReceiptStatus> result = _context
                    .ReceiptStatuses
                    .OrderBy(o => o.Created)
                    .AsNoTracking()
                    .AsQueryable();

            if (request.SearchString is not null)
            {
                result = result.Where(o => o.Name.ToLower().Contains(request.SearchString.ToLower()));
            }

            var totalCount = await result.CountAsync(cancellationToken);

            if (request.SortBy is not null)
            {
                result = result.OrderBy(request.SortBy, request.SortDirection);
            }
            else
            {
                result = result.OrderBy(x => x.Name);
            }

            var items = await result
                .Skip((request.Page) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<ReceiptStatusDto>(items.Select(cp => cp.ToDto()), totalCount);
        }
    }
}
