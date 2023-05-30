using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Payments.Domain;
using YourBrand.Payments.Application.Services;
using YourBrand.Payments.Application.Features.Receipts.Dtos;

namespace YourBrand.Payments.Application.Features.Receipts.Statuses.Queries;

public record GetReceiptStatusQuery(int Id) : IRequest<ReceiptStatusDto?>
{
    class GetReceiptStatusQueryHandler : IRequestHandler<GetReceiptStatusQuery, ReceiptStatusDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetReceiptStatusQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<ReceiptStatusDto?> Handle(GetReceiptStatusQuery request, CancellationToken cancellationToken)
        {
            var orderStatus = await _context
               .ReceiptStatuses
               .AsNoTracking()
               .FirstAsync(c => c.Id == request.Id);

            if (orderStatus is null)
            {
                return null;
            }

            return orderStatus.ToDto();
        }
    }
}
