using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Orders.Domain;
using YourBrand.Orders.Application.Services;
using YourBrand.Orders.Application.Features.Orders.Dtos;

namespace YourBrand.Orders.Application.Features.Orders.Statuses.Queries;

public record GetOrderStatusQuery(int Id) : IRequest<OrderStatusDto?>
{
    class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService currentUserService;

        public GetOrderStatusQueryHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            this.currentUserService = currentUserService;
        }

        public async Task<OrderStatusDto?> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var orderStatus = await _context
               .OrderStatuses
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
