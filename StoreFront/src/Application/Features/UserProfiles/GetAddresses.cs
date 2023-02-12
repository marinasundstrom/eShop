using MediatR;

namespace YourBrand.StoreFront.Application.Features.UserProfiles;

public sealed record GetAddresses : IRequest<IEnumerable<AddressDto>>
{
    sealed class Handler : IRequestHandler<GetAddresses, IEnumerable<AddressDto>>
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

        public async Task<IEnumerable<AddressDto>> Handle(GetAddresses request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;

            var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellationToken);

            return new[] { customer.Address.ToDto() };
        }
    }
}
