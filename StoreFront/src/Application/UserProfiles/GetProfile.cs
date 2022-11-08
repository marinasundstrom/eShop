using System;
using MediatR;
using Microsoft.Extensions.Logging;
using YourBrand.StoreFront.Application.Analytics;

namespace YourBrand.StoreFront.Application.UserProfiles;

public sealed record GetProfile : IRequest<UserProfileDto>
{
    sealed class Handler : IRequestHandler<GetProfile, UserProfileDto>
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

        public async Task<UserProfileDto> Handle(GetProfile request, CancellationToken cancellationToken)
        {
            var customerId = currentUserService.CustomerNo;

            var customer = await customersClient.GetCustomerAsync(customerId.GetValueOrDefault(), cancellationToken);

            return new UserProfileDto(customer.Id, customer.FirstName, customer.LastName, customer.Ssn, customer.Email, customer.Phone, customer.PhoneMobile);
        }
    }
}
