using MediatR;

namespace YourBrand.StoreFront.Application.UserProfiles;

public sealed record GetAddresses : IRequest<IEnumerable<YourBrand.Customers.AddressDto>>
{
    sealed class Handler : IRequestHandler<GetAddresses, IEnumerable<YourBrand.Customers.AddressDto>>
    {
        private readonly YourBrand.Customers.ICustomersClient customersClient;
        private readonly YourBrand.Sales.IOrdersClient _ordersClient;
        private readonly ICurrentUserService currentUserService;

        public Handler(
            YourBrand.Customers.ICustomersClient customersClient,
            YourBrand.Sales.IOrdersClient ordersClient,
            ICurrentUserService currentUserService)
        {
            this.customersClient = customersClient;
            this._ordersClient = ordersClient;
            this.currentUserService = currentUserService;
        }

        public async Task<IEnumerable<YourBrand.Customers.AddressDto>> Handle(GetAddresses request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;

            var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellationToken);

            return new[] { customer.Address };
        }
    }
}
