using MediatR;
using YourBrand.Orders;
using YourBrand.StoreFront.Application.Common.Models;

namespace YourBrand.StoreFront.Application.Features.UserProfiles;

public sealed record GetOrders(int Page = 1, int PageSize = 10) : IRequest<ItemsResult<OrderDto>>
{
    sealed class Handler : IRequestHandler<GetOrders, ItemsResult<OrderDto>>
    {
        private readonly YourBrand.Customers.ICustomersClient customersClient;
        private readonly YourBrand.Orders.IOrdersClient _ordersClient;
        private readonly ICurrentUserService currentUserService;

        public Handler(
            YourBrand.Customers.ICustomersClient customersClient,
            YourBrand.Orders.IOrdersClient ordersClient,
            ICurrentUserService currentUserService)
        {
            this.customersClient = customersClient;
            this._ordersClient = ordersClient;
            this.currentUserService = currentUserService;
        }

        public async Task<ItemsResult<OrderDto>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;

            var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellationToken);

            var orders = await _ordersClient.GetOrdersAsync(null, null, customer.Ssn, null, request.Page, request.PageSize, null, null, cancellationToken);

            return new ItemsResult<OrderDto>(orders.Items, orders.TotalItems);
        }
    }
}