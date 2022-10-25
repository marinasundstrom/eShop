using MediatR;
using Microsoft.EntityFrameworkCore;
using YourBrand.Sales.Application.Common;
using YourBrand.Sales.Application.Orders.Dtos;
using YourBrand.Sales.Domain.Enums;

namespace YourBrand.Sales.Application.Orders.Queries;

public record GetOrders(OrderStatusDto? Status, string? AssigneeId, int Page = 1, int PageSize = 10, string? SortBy = null, SortDirection? SortDirection = null) : IRequest<ItemsResult<OrderDto>>
{
    public class Handler : IRequestHandler<GetOrders, ItemsResult<OrderDto>>
    {
        private readonly IOrderRepository orderRepository;

        public Handler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task<ItemsResult<OrderDto>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            var query = orderRepository.GetAll();

            if (request.Status is not null)
            {
                query = query.Where(x => x.Status == (OrderStatus)request.Status);
            }

            if (request.AssigneeId is not null)
            {
                query = query.Where(x => x.AssigneeIdId == request.AssigneeId);
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
                .Include(i => i.AssigneeId)
                .Include(i => i.CreatedBy)
                .Include(i => i.LastModifiedBy)
                .AsSplitQuery()
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize).AsQueryable()
                .ToArrayAsync(cancellationToken);

            return new ItemsResult<OrderDto>(orders.Select(x => x.ToDto()), totalCount);
        }
    }
}
